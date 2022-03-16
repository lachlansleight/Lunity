Shader "Lunity/UnlitTransparentTexture_Blended" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1, 1, 1, 1)
	}
	
	SubShader {
    Tags {Queue=Transparent}
    Blend SrcAlpha OneMinusSrcAlpha
	ZWrite On
		Pass {			
			CGPROGRAM
			#pragma vertex vert  
	        #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
	        
	        uniform float4 _Color;
	        uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
	        
	        struct vIn {
	        	float4 vertex : POSITION;
	        	float4 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
	        };
	        struct v2f {
	        	float4 pos : SV_POSITION;
	        	float4 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
	        };
	        
	        v2f vert(vIn i) {
	        	v2f o;
	        	
	        	UNITY_SETUP_INSTANCE_ID(i);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    
	        	o.pos = UnityObjectToClipPos(i.vertex);
	        	o.uv = i.texcoord;
	        	return o;
	        }
	        
	        float4 frag(v2f input) : COLOR {
	        	float4 col = tex2D(_MainTex, input.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw);
	        	col *= _Color;
	        	return col;
	        }

			
			ENDCG
		}
	} 
	FallBack "Unlit"
}