using Timespawn.EntityTween.Math;
using Unity.Entities;
using Unity.Mathematics;

namespace Timespawn.EntityTween.Tweens
{
    public unsafe ref struct TweenSequenceBuilder
    {
        EntityCommandBuffer m_ECB;

        public TweenSequenceBuilder(EntityCommandBuffer ecb)
        {
            m_ECB = ecb;
        }

        public TweenSequenceBuilder CreateMoveTween(
            Entity target,
         in float3 start,
         in float3 end,
         in float duration,
         in EaseType easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f,
         in CurvesXYZ curve = default)
        {
            var delayeEntity = m_ECB.CreateEntity();
            var moveTween = Tween.CreateMoveCommand(target, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTweenTime: startTweenTime, curve: curve);
            m_ECB.AddComponent(delayeEntity, moveTween);

            return this;
        }

        public TweenSequenceBuilder CreateScaleTween(
            Entity target, 
            in float3 start,
         in float3 end,
         in float duration,
         in EaseType easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f)
        {
            var delayeEntity = m_ECB.CreateEntity();
            var scaleTween = Tween.CreateScaleCommand(Entity.Null, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTweenTime);
            m_ECB.AddComponent(delayeEntity, scaleTween);

            return this;
        }

        public TweenSequenceBuilder CreateRotateTween(
             Entity target,
            in quaternion start,
           in quaternion end,
           in float duration,
           in EaseType easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           in float startTime = 0.0f)
        {
            var delayeEntity = m_ECB.CreateEntity();
            var rotTween = Tween.CreateRotationCommand(Entity.Null, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTime);
            m_ECB.AddComponent(delayeEntity, rotTween);

            return this;
        }   
    }
}