Shader "Hidden/desaturate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NVColor("Night vision Colour",Color)=(1,1,1,1)
		_Sensitivity("Sensitivity",Range(0,1))=0
		_Contrast("Contrast",Range(0,2))=1 
		_Desaturation("Desaturation",Range(0,1))=0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D_half _MainTex;
			float4 _NVColor;
			fixed _Sensitivity;
			fixed _Contrast;
			fixed _Desaturation;
			float half_PI=3.14159/2;
			float lerpFactor=0;

			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float lum= Luminance(col.rgb);
				//float lum= sqrt(pow(col.r,2)+pow(col.g,2)+pow(col.b,2));
				lum=saturate(lum);
				//lum-=_Sensitivity;
				fixed4 bw=(lum*4)*_Sensitivity;
				bw=pow(bw,_Contrast);
				/*if(_Desaturation<1){
					lerpFactor=_Desaturation*saturate(sin((_Desaturation/2)*(_Time.w+half_PI)));
				}
				else{
					lerpFactor=1;
				}
				float4 finalColor=lerp(col,bw,lerpFactor);
				*/
				float4 finalColor=lerp(col,bw,_Desaturation);
				return finalColor;
			}
			ENDCG
		}
	}
}
