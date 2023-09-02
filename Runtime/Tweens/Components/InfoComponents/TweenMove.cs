using Unity.Entities;
using Unity.Mathematics;


namespace Timespawn.EntityTween.Tweens
{
   
    public struct TweenMove : IComponentData, ITweenId, ITweenInfo<float3>
    {
        public int Id;
        public float3 Start;
        public float3 End;
        public BlobAssetReference<CurveECS> Curve;
        public TweenMove(in int id, in float3 start, in float3 end, BlobAssetReference<CurveECS> curve = default)
        {
            Id = id;
            Start = start;
            End = end;
            Curve = curve;
        }

        public bool HasCurve()
        {
            return Curve.IsCreated;
        }

        public void SetTweenId(in int id)
        {
            Id = id;
        }

        public int GetTweenId()
        {
            return Id;
        }

        public void SetTweenInfo(in float3 start, in float3 end)
        {
            Start = start;
            End = end;
        }

        public float3 GetTweenStart()
        {
            return Start;
        }

        public float3 GetTweenEnd()
        {
            return End;
        }
    }
}