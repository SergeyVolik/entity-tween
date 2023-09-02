using Timespawn.EntityTween.Tweens;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Timespawn.EntityTween
{
    [UpdateInGroup(typeof(TweenApplySystemGroup))]
    internal partial class TweenMoveSystem : SystemBase
    {
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
                        
                            if (tween.Curve.IsCreated)
                            {
                                var dif = tweenInfo.End - tweenInfo.Start;
                                var value = tween.Curve.Value.GetValueAtTime(tween.EasePercentage);
                                pos.y = math.lerp(tweenInfo.Start.y, tweenInfo.End.y, value);
                            }

                            translation.Position = pos;
                            break;
                        }
                    }
                }).ScheduleParallel(Dependency);
        }
    }


}