Shader "Custom/shad"
{
    SubShader  // subshader is used to target different platforms for example we can have different subshader for pc or mobile in this shader
    {
		Pass // every shader should at least have one pass. each pass object gets drawn. multiple pass means we'll draw this multiple times
        {
            CGPROGRAM

            #pragma vertex MyVertexProgram // every shader needs a vertex function
			#pragma fragment MyFragmentProgram // every shader needs a fragment shader

            #include "UnityCG.cginc"
			
            // float4 position : POSITION this one is vertex position in object space
            // : SV_POSITION this indicates that the return value of this function is vertex position in display space
            float4 MyVertexProgram(float4 position : POSITION) : SV_POSITION
            {
                // The 4 by 4 MVP matrix is defined in UnityShaderVariables as UNITY_MATRIX_MVP. We can use the mul function to multiply it with the vertex position. This will correctly project our sphere onto the display. You can also move, rotate, and scale it and the image will change as expected.
                //return mul(UNITY_MATRIX_MVP, position); updated with the line below. same logic, different syntax.
                return UnityObjectToClipPos(position);
			}

            // float4 position : SV_POSITION this indicates that the input parameter is position in display space
            // : SV_TARGET this indicates that the returnin value will be written to default target which is the frame buffer
			float4 MyFragmentProgram(float4 position : SV_POSITION) : SV_TARGET
            {
                return 0;
			}

            ENDCG
		}
	}
}
