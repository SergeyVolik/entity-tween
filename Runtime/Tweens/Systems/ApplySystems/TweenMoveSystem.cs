using Timespawn.EntityTween.Tweens;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Timespawn.EntityTween
{

    [UpdateInGroup(typeof(TweenApplySystemGroup))]
    internal partial class TweenMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<EnableTweensT>();
        }
        protected override void OnUpdate()
        {
            Dependency = Entities
                .WithNone<TweenPause>()
                .ForEach((ref LocalTransform translation, in DynamicBuffer<TweenState> tweenBuffer, in TweenMove tweenInfo) =>
                {
                    for (int i = 0; i < tweenBuffer.Length; i++)
                    {
                        TweenState tween = tweenBuffer[i];
                        if (tween.Id == tweenInfo.Id)
                        {         

                            var pos = math.lerp(tweenInfo.Start, tweenInfo.End, tween.EasePercentage);
                            translation.Position = pos;

                            break;
                        }
                    }
                }).ScheduleParallel(Dependency);
        }
    }


}