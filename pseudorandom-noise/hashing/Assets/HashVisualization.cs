using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public class HashVisualization : MonoBehaviour
{
    [SerializeField, Range(1, 512)] int resolution = 16;
    [SerializeField] Mesh mesh;
    [SerializeField] Material mat;
    RenderParams rp;
    NativeArray<uint> hashes;
    ComputeBuffer hashesBuf;

    void OnEnable()
    {
        int length = resolution * resolution;
        hashes = new NativeArray<uint>(length, Allocator.Persistent);
        hashesBuf = new ComputeBuffer(length, sizeof(uint));

        new HashJob {
            hashes = hashes,
        }.ScheduleParallel(hashes.Length, resolution, default).Complete();
        
        hashesBuf.SetData(hashes);

        rp = new(mat)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one),
            matProps = new MaterialPropertyBlock()
        };
        rp.matProps.SetBuffer("_Hashes", hashesBuf);
        rp.matProps.SetVector("_Config", new Vector4(resolution, 1f / resolution));
        Debug.Log(new Vector4(resolution, 1f / resolution));
    }

    void Update()
    {
        Graphics.RenderMeshPrimitives(rp, mesh, 0, hashes.Length);
    }

    void OnDisable()
    {
        hashes.Dispose();
        hashesBuf.Release();
        hashesBuf = null;
    }

    void OnValidate()
    {
        if (hashesBuf != null && enabled)
        {
            OnDisable();
            OnEnable();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }


    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct HashJob : IJobFor
    {
        [WriteOnly] public NativeArray<uint> hashes;

        public void Execute(int i)
        {
            hashes[i] = (uint)i;
        }
    }
}