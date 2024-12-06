Shader "ExampleShader"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            StructuredBuffer<uint> _Hashes; 
            float4 _Config;

            struct v2f
            { 
                float4 pos : SV_POSITION;
                uint instanceID : TEXCOORD1;
            };

            float4x4 CreateMatrixFromPosition(float4 position)
            {
                float4x4 translationMatrix = float4x4(
                    1, 0, 0, position.x,
                    0, 1, 0, position.y,
                    0, 0, 1, position.z,
                    0, 0, 0, 1
                );

                return translationMatrix;
            }

            float4x4 CreateMatrixFrom2DPosition(float2 position, float z)
            {
                float4x4 translationMatrix = float4x4(
                    1, 0, 0, position.x,
                    0, 1, 0, position.y,
                    0, 0, 1, z,
                    0, 0, 0, 1
                );

                return translationMatrix;
            }

 
            v2f vert(appdata_base vData, uint instanceID : SV_InstanceID)
            {
                v2f o;

                o.instanceID = instanceID;
                
                float v = floor(_Config.y * instanceID);
                float u = instanceID - _Config.x * v;

                float xPos =  _Config.y * (u + 0.5) - 0.5;
                float zPos = _Config.y * (v + 0.5) - 0.5;
                float s = _Config.y; // scale
                float4x4 instanceMatrix = float4x4(
                    s, 0, 0, xPos,
                    0, s, 0, 0,
                    0, 0, s, zPos,
                    0, 0, 0, 1
                );

                o.pos = mul(UNITY_MATRIX_VP, mul(instanceMatrix, vData.vertex));
                return o; 
            }

            float4 frag(v2f i) : SV_Target
            {
                uint hash = _Hashes[i.instanceID];
                return _Config.y * _Config.y * hash;
            }
            ENDCG
        }
    }
}