Shader "Lunity/Outline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_Color ("Color (RGB)", Color) = (1, 1, 1, 1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0, 1)) = .1
		_FlashSpeed ("Flash Speed", Float) = 1
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
uniform float4 _Color;
uniform float _FlashSpeed;
 
v2f vert(appdata v) {
	// just make a copy of incoming vertex data but scaled according to normal direction
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline * 0.067;
	o.color = _OutlineColor;
	return o;
}
ENDCG
 
	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True"}
		Cull Back
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		 
		sampler2D _MainTex;
		 
		struct Input {
			float2 uv_MainTex;
		};
		 
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Emission = lerp(0, 0.2, (sin(_Time.y * _FlashSpeed * 6.28) + 1.0) * 0.5);
			o.Alpha = c.a;
		}
		ENDCG

		Pass {
			Name "OUTLINE"
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True"}
			Cull Front
			ZWrite Off
			//ZTest Less
			//Offset 1, 1
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
	}
 
	Fallback "Diffuse"
}