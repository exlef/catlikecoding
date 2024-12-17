Shader "Custom/shad"
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
            CGPROGRAM

            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST; // this variable stores tiling and offset values from material. it is being filled up automatically by unity so naming of it matters.

            struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

            struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                // this scales and moves uv(s) around
                //i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // it has a builtin method that does this
                i.uv = TRANSFORM_TEX(v.uv, _MainTex); // this is a macro #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw) so it still needed to have _MainTex_ST to work properly
                return i;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET
            {
                return tex2D(_MainTex, i.uv) * _Tint;
			}

            ENDCG
		}
	}
}
