//adapted from NormalVR gradient skybox

Shader "Lunity/Gradient Skybox" {
	Properties {
		[Header(Gradient Colors)]
		_ColorT ("Top Color", Color) = (1,1,1,1)
		_ColorB ("Bottom Color", Color) = (1,1,1,1)
		
		[Header(Additional Settings)]
		[Toggle(DITHER)] _Dither ("Add Screenspace Dither", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Background" "Queue"="Background" "DisableBatching"="True" "IgnoreProjector"="True" "PreviewType"="Skybox" }
		Fog { Mode Off }
		ZWrite Off
		Cull Back

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature DITHER
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_instancing
			#include "UnityCG.cginc"

			fixed3 _ColorT;
			fixed3 _ColorB;
	
			#ifdef DITHER
			float3 ScreenSpaceDither(float2 screenpos)
			{
				float3 dither = dot(float2(171.0, 231.0), screenpos + _Time.yy).xxx;
				dither.rgb = frac(dither / float3(103.0, 71.0, 97.0)) - float3(0.5, 0.5, 0.5);
				return (dither / 255.0);
			}
			#endif
			
			struct appdata {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings {
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings vert(appdata v) {
				Varyings o;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varyings, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}
			
			fixed4 frag (Varyings i) : SV_Target {
				float3 n = normalize(i.texcoord);

				fixed3 color = lerp(_ColorB, _ColorT, saturate(n.y * 0.5 + 0.5));
				
				#if DITHER
				color += ScreenSpaceDither(i.vertex.xy);
				#endif
				
				//return fixed4(_ColorB, 1);
				return fixed4(color, 1);
			}
			ENDCG
		}
	}
}
