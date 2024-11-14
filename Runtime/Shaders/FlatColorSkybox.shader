Shader "Skybox/Unlit" {
Properties {
    _Tint ("Tint Color", Color) = (0, 0, 0, 1)
    [Gamma] _Exposure ("Exposure", Range(0, 8)) = 0.5
}

SubShader {
    Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
    Cull Off ZWrite Off

    CGINCLUDE
    #include "UnityCG.cginc"

    half4 _Tint;
    half _Exposure;
    
    struct appdata_t {
            float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        struct v2f {
            float4 vertex : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
        };
    
    v2f vert (appdata_t v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex = UnityObjectToClipPos(v.vertex);
        return o;
    }
    half4 skybox_frag (v2f i)
    {
        half3 c = _Tint.rgb * unity_ColorSpaceDouble.rgb;
        c *= _Exposure;
        return half4(c, 1);
    }
    ENDCG

    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 2.0
        half4 frag (v2f i) : SV_Target { return skybox_frag(i); }
        ENDCG
    }
}
}