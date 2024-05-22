Shader "Custom/Toon/ToonLightEdit"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        [HDR]_BaseColor ("Color", Color) = (0.5,0.5,0.5,1)
        _OpacityMap ("Opacity Map", 2D) = "white" {}
        _Opacity ("Opacity", Range(0, 1)) = 1

        _ShadowStep ("ShadowStep", Range(0, 1)) = 0.5
        _ShadowStepSmooth ("ShadowStepSmooth", Range(0, 1)) = 0.04

        _SpecularStep ("SpecularStep", Range(0, 1)) = 0.6
        _SpecularStepSmooth ("SpecularStepSmooth", Range(0, 1)) = 0.05
        [HDR]_SpecularColor ("SpecularColor", Color) = (1,1,1,1)

        _RimStep ("RimStep", Range(0, 1)) = 0.65
        _RimStepSmooth ("RimStepSmooth",Range(0,1)) = 0.4
        [HDR]_RimColor ("RimColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "UniversalForward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_OpacityMap); SAMPLER(sampler_OpacityMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float _ShadowStep;
            float _ShadowStepSmooth;
            float _SpecularStep;
            float _SpecularStepSmooth;
            float4 _SpecularColor;
            float _RimStepSmooth;
            float _RimStep;
            float4 _RimColor;
            float _Opacity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 normalWS : TEXCOORD1; // xyz: normal, w: viewDir.x
                float4 tangentWS : TEXCOORD2; // xyz: tangent, w: viewDir.y
                float4 bitangentWS : TEXCOORD3; // xyz: bitangent, w: viewDir.z
                float3 viewDirWS : TEXCOORD4;
                float4 shadowCoord : TEXCOORD5; // shadow receive
                float4 fogCoord : TEXCOORD6;
                float3 positionWS : TEXCOORD7;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                float3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.uv = input.uv;
                output.normalWS = float4(normalInput.normalWS, viewDirWS.x);
                output.tangentWS = float4(normalInput.tangentWS, viewDirWS.y);
                output.bitangentWS = float4(normalInput.bitangentWS, viewDirWS.z);
                output.viewDirWS = viewDirWS;
                output.fogCoord = ComputeFogFactor(output.positionCS.z);
                output.shadowCoord = TransformWorldToShadowCoord(input.positionOS.xyz);
                return output;
            }

            half remap(half x, half t1, half t2, half s1, half s2)
            {
                return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float2 uv = input.uv;
                float3 N = normalize(input.normalWS.xyz);
                float3 T = normalize(input.tangentWS.xyz);
                float3 B = normalize(input.bitangentWS.xyz);
                float3 V = normalize(input.viewDirWS.xyz);
                float3 L = normalize(_MainLightPosition.xyz);
                float3 H = normalize(V+L);

                float NV = dot(N,V);
                float NH = dot(N,H);
                float NL = dot(N,L);

                NL = NL * 0.5 + 0.5;

                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                float4 opacityMap = SAMPLE_TEXTURE2D(_OpacityMap, sampler_OpacityMap, uv);
                float opacity = _Opacity * opacityMap.r;

                // return NH;
                float specularNH = smoothstep((1-_SpecularStep * 0.05) - _SpecularStepSmooth * 0.05, (1-_SpecularStep* 0.05) + _SpecularStepSmooth * 0.05, NH) ;
                float shadowNL = smoothstep(_ShadowStep - _ShadowStepSmooth, _ShadowStep + _ShadowStepSmooth, NL);

                //shadow
                float shadow = MainLightRealtimeShadow(input.shadowCoord);

                //rim
                float rim = smoothstep((1-_RimStep) - _RimStepSmooth * 0.5, (1-_RimStep) + _RimStepSmooth * 0.5, 0.5 - NV);

                //diffuse
                float3 diffuse = _MainLightColor.rgb * baseMap * _BaseColor * shadowNL * shadow * opacity;

                //specular
                float3 specular = _SpecularColor * shadow * shadowNL * specularNH * opacity;

                //ambient
                float3 ambient = rim * _RimColor + SampleSH(N) * _BaseColor * baseMap * opacity;

                float3 finalColor = diffuse + ambient + specular;
                finalColor = MixFog(finalColor, input.fogCoord);

                // Alpha test
                clip(opacity - 0.5);

                return float4(finalColor, opacity);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
