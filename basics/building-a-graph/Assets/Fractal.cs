using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;

public class Fractal : MonoBehaviour
{

	struct FractalPart {
		public float3 direction, worldPosition;
		public quaternion rotation, worldRotation;
        public float spinAngle;
	}

    [Range(1, 8)] public int depth = 4;
    [SerializeField] Mesh mesh;

	[SerializeField] Material material;

    static float3[] directions = {
		up(), right(), left(), forward(), back()
	};

	static quaternion[] rotations = {
		quaternion.identity,
		quaternion.RotateZ(-0.5f * PI), quaternion.RotateZ(0.5f * PI),
		quaternion.RotateX(0.5f * PI), quaternion.RotateX(-0.5f * PI)
	};

    NativeArray<FractalPart>[] parts;
	NativeArray<float3x4>[] matrices;
    ComputeBuffer[] matricesBuffers;
    static MaterialPropertyBlock propertyBlock;

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct UpdateFractalLevelJob : IJobFor {

        public float spinAngleDelta;
        public float scale;

        [ReadOnly] public NativeArray<FractalPart> parents;
        public NativeArray<FractalPart> parts;

        [WriteOnly] public NativeArray<float3x4> matrices;

        public void Execute (int i)
        {
			FractalPart parent = parents[i / 5];
			FractalPart part = parts[i];
			part.spinAngle += spinAngleDelta;
			part.worldRotation = mul(parent.worldRotation,
				mul(part.rotation, quaternion.RotateY(part.spinAngle))
			);
			part.worldPosition =
				parent.worldPosition +
				mul(parent.worldRotation, 1.5f * scale * part.direction);
			parts[i] = part;

			float3x3 r = float3x3(part.worldRotation) * scale;
			matrices[i] = float3x4(r.c0, r.c1, r.c2, part.worldPosition);
		}
	}

    void OnEnable()
    {
		parts = new NativeArray<FractalPart>[depth];
		matrices = new NativeArray<float3x4>[depth];
        matricesBuffers = new ComputeBuffer[depth];
        int stride = 12 * sizeof(float);

        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
			parts[i] = new NativeArray<FractalPart>(length, Allocator.Persistent);
            matrices[i] = new NativeArray<float3x4>(length, Allocator.Persistent);
            matricesBuffers[i] = new ComputeBuffer(length, stride);
		}

        parts[0][0] = CreatePart(0);
        for (int li = 1; li < parts.Length; li++) // level index
        {
			NativeArray<FractalPart> levelParts = parts[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi += 5) // fractal part index
            {
                for (int ci = 0; ci < 5; ci++) // child index
                {
					levelParts[fpi + ci] = CreatePart(ci);
				}
			}
		}

        propertyBlock ??= new MaterialPropertyBlock();
    }

    void OnDisable()
    {
        for (int i = 0; i < matricesBuffers.Length; i++)
        {
			matricesBuffers[i].Release();
            parts[i].Dispose();
			matrices[i].Dispose();
		}
        parts = null;
		matrices = null;
		matricesBuffers = null;
    }

    void OnValidate ()
    {
		if (parts != null && enabled)
        {
			OnDisable();
			OnEnable();
		}
	}

    void Update()
    {
		float spinAngleDelta =  0.125f * PI * Time.deltaTime;

        FractalPart rootPart = parts[0][0];
        rootPart.spinAngle += spinAngleDelta;
        rootPart.worldRotation = mul(transform.rotation, mul(rootPart.rotation, quaternion.RotateY(rootPart.spinAngle)));
        rootPart.worldPosition = transform.position;
        float objectScale = transform.lossyScale.x;
        float3x3 r = float3x3(rootPart.worldRotation) * objectScale;
		matrices[0][0] = float3x4(r.c0, r.c1, r.c2, rootPart.worldPosition);
        parts[0][0] = rootPart;

        float scale = objectScale;
        JobHandle jobHandle = default;
        for (int li = 1; li < parts.Length; li++)
        {
            scale *= 0.5f;
            jobHandle = new UpdateFractalLevelJob {
				spinAngleDelta = spinAngleDelta,
				scale = scale,
				parents = parts[li - 1],
				parts = parts[li],
				matrices = matrices[li]
            }.ScheduleParallel(parts[li].Length, 1, jobHandle);
		}
        jobHandle.Complete();

        var bounds = new Bounds(rootPart.worldPosition, 3f * objectScale * Vector3.one);
		for (int i = 0; i < matricesBuffers.Length; i++) {
			ComputeBuffer buffer = matricesBuffers[i];
			buffer.SetData(matrices[i]);
			propertyBlock.SetBuffer("_Matrices", buffer);
			Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, buffer.count, propertyBlock);
		}
    }

    FractalPart CreatePart(int childIndex)
    {
        return new FractalPart {
			direction = directions[childIndex],
			rotation = rotations[childIndex],
		};
    }
}
