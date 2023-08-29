using Timespawn.EntityTween.Tweens;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Timespawn.EntityTween
{
    [UpdateInGroup(typeof(TweenApplySystemGroup))]
    internal partial class TweenTranslationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Dependency = Entities
                .WithNone<TweenPause>()
                .ForEach((ref LocalTransform translation, in DynamicBuffer<TweenState> tweenBuffer, in TweenTranslation tweenInfo) =>
                {
                    for (int i = 0; i < tweenBuffer.Length; i++)
                    {
                        TweenState tween = tweenBuffer[i];
                        if (tween.Id == tweenInfo.Id)
                        {
                            translation.Position = math.lerp(tweenInfo.Start, tweenInfo.End, tween.EasePercentage);
                            break;
                        }
                    }
                }).ScheduleParallel(Dependency);
        }
    }


}