Shader "MagicalReality/fadeIntoSmoke" {
	Properties {
		//Text textures
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal Map",2D)="bump"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Color("Color RGBA", Color)=(1,1,1,1)

		//Smoke Texture
		_SmokeTex ("Albedo (RGB)", 2D) = "white" {}
		_SmokeColor("Smoke Colour",Color)=(1,1,1,1)
		_SmokeDirection("Smoke Direction",Vector)=(0,1,0,0)
		_SmokeAmount("Smoke Amount",Range(0,1))=0
		_SmokeFade("Smoke Fade",Range(0,1))=0


	}
	SubShader {
		Tags{"Queue"="Transparent" "RenderType"="Transparent"}
		ZWrite  Off
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light type
		#pragma surface surf Standard alpha:blend  
		#include "UnityCG.cginc"

		sampler2D_half _MainTex;
		sampler2D_half _Normal;
		half _Glossiness;
		sampler2D_half _SmokeTex;
		float4 _SmokeColor;  
		float4 _SmokeDirection;
		half _SmokeAmount;
		half _SmokeFade;
		const fixed3 blanco={1,1,1};

		struct Input {
			float2 uv_MainTex: TEXCOORD0;
			float2 uv2_SmokeTex;
			float3 worldNormal;
			INTERNAL_DATA 
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			half4 mainTex = tex2D (_MainTex, IN.uv_MainTex);
			fixed3 normalTex=UnpackNormal(tex2D(_Normal,IN.uv_MainTex));
			half4 smokeTex=tex2D(_SmokeTex,IN.uv2_SmokeTex);
			half dotNormal=dot(WorldNormalVector(IN,lerp(normalTex,blanco,_SmokeAmount)),_SmokeDirection.xyz)-lerp(1,-1,_SmokeAmount);
			dotNormal=saturate(dotNormal/_SmokeFade);
			
			o.Albedo = lerp(mainTex.rgb,smokeTex.rgb+_SmokeColor.rgb,dotNormal);
			o.Normal=lerp(normalTex,blanco,dotNormal);
			o.Smoothness = lerp(_Glossiness,0,_SmokeAmount);
			o.Alpha = lerp(mainTex.a,smokeTex.a,dotNormal);
			clip (lerp(mainTex.a,smokeTex.a,dotNormal) - 0.1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
