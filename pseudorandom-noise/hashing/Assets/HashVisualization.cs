using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Ex;

using static Unity.Mathematics.math;

public class HashVisualization : MonoBehaviour
{
    [SerializeField, Range(1, 512)] int resolution = 16;
    [SerializeField, Range(-2f, 2f)] float verticalOffset = 1f;
    [SerializeField] Mesh mesh;
    [SerializeField] Material mat;
    [SerializeField] int seed;
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
            resolution = resolution,
            invResolution = 1f / resolution,
            hash = SmallXXHash.Seed(seed),
        }.ScheduleParallel(hashes.Length, resolution, default).Complete();
        
        hashesBuf.SetData(hashes);

        rp = new(mat)
        {
            worldBounds = new Bounds(Vector3.zero, Vector3.one),
            matProps = new MaterialPropertyBlock()
        };
        rp.matProps.SetBuffer("_Hashes", hashesBuf);
        rp.matProps.SetVector("_Config", new Vector4(resolution, 1f / resolution, verticalOffset / resolution));
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
        public int resolution;
        public float invResolution;
        public SmallXXHash hash;

        public void Execute(int i)
        {
            float epsilon = 1e-5f; // to get rid off the floating point percision errors. that cause by floor function
            int v = (int)floor(invResolution * i + epsilon);
            int u = i - resolution * v;

            hashes[i] = hash.Eat(u).Eat(v);
        }
    }
}