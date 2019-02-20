Shader "Magical Reality/nightFilter"
{
	Properties {
		_Color("Night color",Color)=(0.3,0.5,1,1)
		_LightColor("Light color",Color)=(0.3,0.5,1,1)
		_LampColor("Lamps color",Color)=(1,0.8,0.3,1)
		_Scale("Texture Scale",Range(0.8,1.2))=1
	}

    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
            fixed _Scale;
            v2f vert(appdata_base v) {
                v2f o;
                // use UnityObjectToClipPos from UnityCG.cginc to calculate 
                // the clip-space of the vertex
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.grabPos.yw*=_Scale;
                return o;
            }

            sampler2D _BackgroundTexture;
            half4 _Color;
            half4 _LampColor;
            half4 _LightColor;
            

            half4 frag(v2f i) : SV_TARGET
            {

                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
                half4 highlights;
                float lum= Luminance(bgcolor.rgb);
				lum=saturate(lum);
                float brightness=i.grabPos.y/4;
                //float brightness=0.6;
                float4 c;
               // bgcolor.rgb=clamp(bgcolor.rgb,0,brightness);
                c.rgb=bgcolor.rgb*lerp(_Color,_LightColor,brightness);
                c.a=_LightColor.a;
                c.rgb=lerp(c.rgb,_LampColor,step(0.9,lum));//(lum,brightness)); //brightness>lum
                //highlights.rgb=step(0.9,lum)*_LampColor;//+(_LampColor*lerp(lum,0.01,step(lum,0.8)));
                //c.rgb=lerp(_LampColor,c.rgb,1-lum);
                return c;
            }
            ENDCG
        }

    }

}
