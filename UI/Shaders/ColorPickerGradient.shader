Shader "Lunity/UI/ColorPickerGradient"
{
    Properties
    {
        _Hue ("Hue", Range(0.0, 1.0)) = 0.0
        _Saturation ("Saturation", Range(0.0, 1.0)) = 1.0
        _Value ("Value", Range(0.0, 1.0)) = 1.0
        _ColorMode ("Color Mode", Vector) = (0, 0, 0, 0)
        //x = hue (0 = constant, 1 = uvX, 2 = uvY)
        //y = Saturation, etc
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float3 hsv2rgb(float3 hsv)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
                
                return hsv.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), hsv.y);
            }

            float _Hue;
            float _Saturation;
            float _Value;
            float3 _ColorMode;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 hsv = 0;
            
                if(_ColorMode.x == 0) hsv.x = _Hue;
                else if(_ColorMode.x == 1) hsv.x = i.uv.x;
                else if(_ColorMode.x == 2) hsv.x = i.uv.y;
                
                if(_ColorMode.y == 0) hsv.y = _Saturation;
                else if(_ColorMode.y == 1) hsv.y = i.uv.x;
                else if(_ColorMode.y == 2) hsv.y = i.uv.y;
                
                if(_ColorMode.z == 0) hsv.z = _Value;
                else if(_ColorMode.z == 1) hsv.z = i.uv.x;
                else if(_ColorMode.z == 2) hsv.z = i.uv.y;
                
                return float4(hsv2rgb(hsv), 1);
                
            }
            ENDCG
        }
    }
}
