Shader "Hidden/Rendering/PaletteSwapBlit4"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        ZWrite Off
        ZTest Always
        Cull Off
        Blend One Zero

        Pass
        {
            Name "PaletteSwapBlit4"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _SrcColor0;
                half4 _SrcColor1;
                half4 _SrcColor2;
                half4 _SrcColor3;
                half4 _DstColor0;
                half4 _DstColor1;
                half4 _DstColor2;
                half4 _DstColor3;
                float _MatchEpsilon;
            CBUFFER_END

            half3 RemapColor(half3 inputColor)
            {
                if (distance(inputColor, _SrcColor0.rgb) <= _MatchEpsilon) return _DstColor0.rgb;
                if (distance(inputColor, _SrcColor1.rgb) <= _MatchEpsilon) return _DstColor1.rgb;
                if (distance(inputColor, _SrcColor2.rgb) <= _MatchEpsilon) return _DstColor2.rgb;
                if (distance(inputColor, _SrcColor3.rgb) <= _MatchEpsilon) return _DstColor3.rgb;
                return inputColor;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord.xy);
                color.rgb = RemapColor(color.rgb);
                return color;
            }
            ENDHLSL
        }
    }
}

