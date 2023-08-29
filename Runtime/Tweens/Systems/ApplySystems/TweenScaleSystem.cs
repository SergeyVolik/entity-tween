using Timespawn.EntityTween.Tweens;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Timespawn.EntityTween
{
    [UpdateInGroup(typeof(TweenApplySystemGroup))]
    internal partial class TweenScaleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Dependency = Entities
                .WithNone<TweenPause>()
                .ForEach((ref LocalTransform scale, in DynamicBuffer<TweenState> tweenBuffer, in TweenScale tweenInfo) =>
                {
                    for (int i = 0; i < tweenBuffer.Length; i++)
                    {
                        TweenState tween = tweenBuffer[i];
                        if (tween.Id == tweenInfo.Id)
                        {
                            
                            scale.Scale = math.lerp(tweenInfo.Start.x, tweenInfo.End.y, tween.EasePercentage);
                            break;
                        }
                    }
                }).ScheduleParallel(Dependency);
        }
    }
}