using Timespawn.EntityTween.Math;
using Timespawn.EntityTween.Tweens;
using Unity.Entities;

namespace Timespawn.EntityTween
{
    [UpdateInGroup(typeof(TweenSimulationSystemGroup))]
    internal partial class TweenEaseSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entities
                .WithNone<TweenPause>()
                .ForEach((ref DynamicBuffer<TweenState> tweenBuffer) =>
                {
                    for (int i = 0; i < tweenBuffer.Length; i++)
                    {
                        TweenState tween = tweenBuffer[i];
                        tween.Time += tween.IsReverting ? -deltaTime : deltaTime;

                        float normalizedTime = tween.GetNormalizedTime();

                        
                        tween.EasePercentage = Ease.CalculatePercentage(normalizedTime, tween.EaseType);
                        UnityEngine.Debug.Log($"{tween.EaseType} {tween.EasePercentage}");
                        tweenBuffer[i] = tween;
                    }
                }).ScheduleParallel();
        }
    }
}