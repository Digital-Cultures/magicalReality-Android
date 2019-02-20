Shader "MagicalReality/clay" {
	Properties {
		//Paper Textures
		_Color ("Tint",Color )=(1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Normal Map",2D)="bump"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		
		//Clay Texture
		_ClayTex ("Clay Albedo (RGB)", 2D) = "white" {}
		_ClayNormalTex("Clay Normal Map",2D)="bump"{}
		//_ClayGlossiness ("Clay Smoothness", Range(0,1)) = 0.5
		_ClayDirection ("Clay Direction",Vector)=(0,1,0,0)
		_ClayAmount("Clay Amount", Range(0,1))=0
		_ClayFade("Clay Fade",Range(0,1))=0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		Cull Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "UnityCG.cginc"

		sampler2D_half _MainTex;
		sampler2D_half _NormalTex;
		sampler2D_half _ClayTex;
		sampler2D_half _ClayNormalTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
			INTERNAL_DATA
		};

		half _Glossiness;
		half _ClayGlossiness;
		float4 _ClayDirection;
		fixed _ClayAmount;
		fixed _ClayFade;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed4 mainTex=tex2D(_MainTex,IN.uv_MainTex);
			fixed3 normalTex = UnpackNormal(tex2D(_NormalTex,IN.uv_MainTex));

			fixed4 clayTex=tex2D(_ClayTex,IN.uv_MainTex);
			fixed3 clayNormalTex = UnpackNormal(tex2D(_ClayNormalTex,IN.uv_MainTex));

			half dotNormal=dot(WorldNormalVector(IN,lerp(normalTex,clayNormalTex,_ClayAmount)),_ClayDirection.xyz)-lerp(1,-1,_ClayAmount);
			dotNormal=saturate(dotNormal/_ClayFade);
			o.Albedo=lerp(mainTex.rgb*_Color.rgb,clayTex.rgb*_Color.rgb,dotNormal);
			o.Alpha=_Color.a;
			o.Normal=lerp(normalTex,clayNormalTex,dotNormal);
			o.Smoothness=_Glossiness;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
