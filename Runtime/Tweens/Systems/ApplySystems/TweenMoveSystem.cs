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


                            if (tween.Curve.IsValid())
                            {
                             
                                var valueX = tween.Curve.curveX.Value.GetValueAtTime(tween.EasePercentage);
                                var valueY = tween.Curve.curveY.Value.GetValueAtTime(tween.EasePercentage);
                                var valueZ = tween.Curve.curveZ.Value.GetValueAtTime(tween.EasePercentage);


                                var x = math.lerp(tweenInfo.Start.x, tweenInfo.End.x, valueX);
                                var y = math.lerp(tweenInfo.Start.y, tweenInfo.End.y, valueY);
                                var z = math.lerp(tweenInfo.Start.z, tweenInfo.End.z, valueZ);

                                translation.Position = new float3(x, y, z);

                                break;
                            }

                            var pos = math.lerp(tweenInfo.Start, tweenInfo.End, tween.EasePercentage);
                            translation.Position = pos;

                            break;
                        }
                    }
                }).ScheduleParallel(Dependency);
        }
    }


}