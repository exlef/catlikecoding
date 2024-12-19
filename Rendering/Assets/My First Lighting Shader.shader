Shader "Custom/My First Lighting Shader"
{
    Properties
    {
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
	}

    SubShader
    {
		Pass
        {
            Tags {
				"LightMode" = "ForwardBase"
			}
            
            CGPROGRAM

            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

            #include "UnityStandardBRDF.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct VertexData {
				float4 position : POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

            struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
			};
			
            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return i;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                i.normal = normalize(i.normal); // After producing correct normals in the vertex program, they are passed through the interpolator. Unfortunately, linearly interpolating between different unit-length vectors does not result in another unit-length vector. It will be shorter.
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                return DotClamped(lightDir, i.normal);
			}

            ENDCG
		}
	}
}
