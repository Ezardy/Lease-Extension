Shader "UI Toolkit/Warp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmplitudePx ("Amplitude", Float) = 2.0
        _Frequency ("Frequency", Float) = 2.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Blend One Zero
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _UIE_OUTPUT_LINEAR

            #include "UnityCG.cginc"
            #include "UnityUIEFilter.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                uint rectIndex : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float _AmplitudePx;
            float _Frequency;

            v2f vert(FilterVertexInput v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.rectIndex = GetFilterRectIndex(v);
                return o;
            }

            float2 NormalizeUVs(float2 uv, float4 rect)
            {
                return (uv - rect.xy) / rect.zw;
            }

            float2 MapToUVRect(float2 uv, float4 rect)
            {
                return uv * rect.zw + rect.xy;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 rect = GetFilterUVRect(i.rectIndex);
                float2 uv = NormalizeUVs(i.uv, rect);

                // Distance in pixels from the element's edges.
                float2 sizePx =
                    rect.zw * _MainTex_TexelSize.zw;

                float2 edgePx = min(uv, 1.0 - uv) * sizePx;
                float edgeDistancePx = min(edgePx.x, edgePx.y);

                // Two overlapping waves in each direction.
                float phase = 6.2831853 * _Frequency;

                float waveX =
                    sin((uv.y * 1.15 + uv.x * 0.35) * phase)
                    + 0.35 *
                    sin((uv.y * 2.70 - uv.x * 0.80) * phase);

                float waveY =
                    cos((uv.x * 1.05 - uv.y * 0.25) * phase)
                    + 0.35 *
                    sin((uv.x * 2.40 + uv.y * 0.65) * phase);

                float2 wave = float2(waveX, waveY) / 1.35;

                // Fade displacement near the boundary to avoid
                // obvious edge-clamping artifacts.
                float edgeFade = smoothstep(
                    0.0,
                    max(_AmplitudePx * 1.5, 1.0),
                    edgeDistancePx
                );

                float2 offsetPx =
                    wave * _AmplitudePx * edgeFade;

                // Convert pixel displacement to local UV units.
                float2 offsetUV =
                    offsetPx * _MainTex_TexelSize.xy / rect.zw;

                uv = saturate(uv + offsetUV);

                fixed4 col = tex2D(
                    _MainTex,
                    MapToUVRect(uv, rect)
                );

                #if _UIE_OUTPUT_LINEAR
                    col.rgb = GammaToLinearSpace(col.rgb);
                #endif

                return col;
            }

            ENDCG
        }
    }
}