Shader "Custom/DaltonismeFilter"
{
    Properties
    {
        [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
        _Mode ("Mode (0:Normal, 1:Protan, 2:Deuteran, 3:Tritan)", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "DaltonismePass"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            Texture2D _BlitTexture;
            // On utilise la macro officielle d'Unity pour éviter les conflits de Sampler
            SAMPLER(sampler_BlitTexture); 
            float _Mode;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Échantillonnage corrigé avec le bon sampler
                half4 color = _BlitTexture.Sample(sampler_BlitTexture, input.uv);
                float3 rgb = color.rgb;
                float3 finalRgb = rgb;

                if (_Mode == 1.0) // Protanopie
                {
                    finalRgb.r = rgb.r * 0.56667 + rgb.g * 0.43333 + rgb.b * 0.00000;
                    finalRgb.g = rgb.r * 0.55833 + rgb.g * 0.44167 + rgb.b * 0.00000;
                    finalRgb.b = rgb.r * 0.00000 + rgb.g * 0.24167 + rgb.b * 0.75833;
                }
                else if (_Mode == 2.0) // Deutéranopie
                {
                    finalRgb.r = rgb.r * 0.62500 + rgb.g * 0.37500 + rgb.b * 0.00000;
                    finalRgb.g = rgb.r * 0.70000 + rgb.g * 0.30000 + rgb.b * 0.00000;
                    finalRgb.b = rgb.r * 0.00000 + rgb.g * 0.30000 + rgb.b * 0.70000;
                }
                else if (_Mode == 3.0) // Tritanopie
                {
                    finalRgb.r = rgb.r * 0.95000 + rgb.g * 0.05000 + rgb.b * 0.00000;
                    finalRgb.g = rgb.r * 0.00000 + rgb.g * 0.43333 + rgb.b * 0.56667;
                    finalRgb.b = rgb.r * 0.00000 + rgb.g * 0.47500 + rgb.b * 0.52500;
                }

                return half4(finalRgb, color.a);
            }
            ENDHLSL
        }
    }
}