// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Modified by Lachlan to just be a flat color

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Diffuse Color" {
Properties {
    _Color ("Albedo (RGB)", Color) = (1, 1, 1, 1)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

fixed3 _Color;

//can't have an empty input struct, even though we don't need any vertex inputs
struct Input {
    float thanksUnity;
};

void surf (Input IN, inout SurfaceOutput o) {
    o.Albedo = _Color;
    o.Alpha = 1;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}