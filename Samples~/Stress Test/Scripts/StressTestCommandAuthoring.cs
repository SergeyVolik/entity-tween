using Timespawn.EntityTween.Math;
using Unity.Entities;
using UnityEngine;

namespace Timespawn.EntityTween.Samples.StressTest
{
  
   

    public class StressTestCommandAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public uint Count;

        [Header("Move")]
        public float MoveDuration;
        public EaseType MoveEaseType;
        public ushort MoveEaseExponent;
        public bool MoveIsPingPong;
        public ushort MoveLoopCount;
        public float StartMoveRadius;
        public float EndMoveRadius;

        [Header("Rotate")]
        public float RotateDuration;
        public EaseType RotateEaseType;
        public ushort RotateEaseExponent;
        public bool RotateIsPingPong;
        public ushort RotateLoopCount;
        public float MinRotateDegree;
        public float MaxRotateDegree;

        [Header("Scale")]
        public float ScaleDuration;
        public EaseType ScaleEaseType;
        public ushort ScaleEaseExponent;
        public bool ScaleIsPingPong;
        public ushort ScaleLoopCount;
        public float MinStartScale;
        public float MaxStartScale;
        public float MinEndScale;
        public float MaxEndScale;

        class Baker : Baker<StressTestCommandAuthoring>
        {
            public override void Bake(StressTestCommandAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(e, new StressTestCommand
                {
                    Count = authoring.Count,
                    EndMoveRadius = authoring.EndMoveRadius,
                    MaxEndScale = authoring.MaxEndScale,
                    MaxRotateDegree = authoring.MaxRotateDegree,
                    MaxStartScale = authoring.MaxStartScale,
                    MinEndScale = authoring.MinEndScale,
                    MinRotateDegree = authoring.MinRotateDegree,
                    MinStartScale = authoring.MinStartScale,
                    MoveDuration = authoring.MoveDuration,
                    MoveEaseExponent = authoring.MoveEaseExponent,
                    MoveEaseType = authoring.MoveEaseType,
                    MoveIsPingPong = authoring.MoveIsPingPong,
                    MoveLoopCount = authoring.MoveLoopCount,
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                    RotateDuration = authoring.RotateDuration,
                    RotateEaseExponent = authoring.RotateEaseExponent,
                    RotateEaseType = authoring.RotateEaseType,
                    RotateIsPingPong = authoring.RotateIsPingPong,
                    RotateLoopCount = authoring.RotateLoopCount,
                    ScaleDuration = authoring.ScaleDuration,
                    ScaleEaseExponent = authoring.ScaleEaseExponent,
                    ScaleEaseType = authoring.ScaleEaseType,
                    ScaleIsPingPong = authoring.ScaleIsPingPong,
                    ScaleLoopCount = authoring.ScaleLoopCount,
                    StartMoveRadius = authoring.StartMoveRadius,


                });
            }
        }
    }
    public struct StressTestCommand : IComponentData
    {
        public Entity Prefab;
        public uint Count;


        public float MoveDuration;
        public EaseType MoveEaseType;
        public ushort MoveEaseExponent;
        public bool MoveIsPingPong;
        public ushort MoveLoopCount;
        public float StartMoveRadius;
        public float EndMoveRadius;


        public float RotateDuration;
        public EaseType RotateEaseType;
        public ushort RotateEaseExponent;
        public bool RotateIsPingPong;
        public ushort RotateLoopCount;
        public float MinRotateDegree;
        public float MaxRotateDegree;


        public float ScaleDuration;
        public EaseType ScaleEaseType;
        public ushort ScaleEaseExponent;
        public bool ScaleIsPingPong;
        public ushort ScaleLoopCount;
        public float MinStartScale;
        public float MaxStartScale;
        public float MinEndScale;
        public float MaxEndScale;
    }
}