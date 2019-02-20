Shader "MagicalReality/Inflate" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		[Toggle] _Emission ("Emission",Int)=0
		_EmitTex("Emit texture",2D)="white"{}
		_EmitColor ("Emit Color",Color)=(1,1,1,1)
		_Extrusion ("Extrusion Amount", Range(-1,1))=0
	}
	SubShader {
		Tags { 
			"RenderType"="Transparent"
			"Queue" = "Geometry"
		}
		Cull Off
		LOD 200


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmitTex;
		};

		sampler2D_half _MainTex;
		float4 _Color;
		fixed _Emission;
		sampler2D_half _EmitTex;
		float4 _EmitColor;
		fixed _Extrusion;

        void vert (inout appdata_full v) {
	        v.vertex.xyz += v.normal * _Extrusion;
        }
		

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e= tex2D(_EmitTex,IN.uv_EmitTex) * _EmitColor*_Emission;
			o.Albedo = c.rgb+e.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
