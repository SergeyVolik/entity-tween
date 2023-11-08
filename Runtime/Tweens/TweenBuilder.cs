using Timespawn.EntityTween.Math;
using Unity.Entities;
using Unity.Mathematics;

namespace Timespawn.EntityTween.Tweens
{
    public unsafe ref struct TweenBuilder
    {
        DelayedMoveTween moveTween;
        DelayedRotationTween rotTween;
        DelayedScaleTween scaleTween;

        TweenType type;

        private enum TweenType
        {
             None = 0,
             Rot,
             Scale,
             Move
        }

        public TweenBuilder CreateMoveTween(
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

            moveTween = Tween.CreateMoveCommand(Entity.Null, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTweenTime: startTweenTime, curve: curve);
            type = TweenType.Move;
            return this;
        }

        public TweenBuilder CreateScaleTween(in float3 start,
         in float3 end,
         in float duration,
         in EaseType easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f)
        {

            scaleTween = Tween.CreateScaleCommand(Entity.Null, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTweenTime);
            type = TweenType.Scale;

            return this;
        }

        public TweenBuilder CreateRotateTween(
            in quaternion start,
           in quaternion end,
           in float duration,
           in EaseType easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           in float startTime = 0.0f)
        {


            rotTween = Tween.CreateRotationCommand(Entity.Null, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTime);
            type = TweenType.Rot;



            return this;
        }

        public TweenBuilder BindCurrent(in EntityManager entityManager, in Entity e)
        {
            var delayeEntity = entityManager.CreateEntity();


            switch (type)
            {

                case TweenType.Rot:
                    rotTween.targetEntity = e;
                    entityManager.AddComponent<DelayedRotationTween>(delayeEntity);
                    entityManager.SetComponentData(delayeEntity, rotTween);
                    break;
                case TweenType.Scale:

                    scaleTween.targetEntity = e;
                    entityManager.AddComponent<DelayedScaleTween>(delayeEntity);
                    entityManager.SetComponentData(delayeEntity, scaleTween);
                    break;
                case TweenType.Move:
                    moveTween.targetEntity = e;
                    entityManager.AddComponent<DelayedMoveTween>(delayeEntity);
                    entityManager.SetComponentData(delayeEntity, moveTween);
                    break;
                default:
                    break;
            }

            return this;

        }

        public TweenBuilder BindCurrent(in EntityCommandBuffer.ParallelWriter ecb, in int sortKey, in Entity e)
        {

            var delayeEntity = ecb.CreateEntity(sortKey);

            switch (type)
            {
             
                case TweenType.Rot:
                    rotTween.targetEntity = e;
                    ecb.AddComponent(sortKey, delayeEntity, rotTween);
                  
                    break;
                case TweenType.Scale:
                    scaleTween.targetEntity = e;
                    ecb.AddComponent(sortKey, delayeEntity, scaleTween);

                    break;
                case TweenType.Move:
                    moveTween.targetEntity = e;
                    ecb.AddComponent(sortKey, delayeEntity, moveTween);

                    break;
                default:
                    break;
            }

            return this;


        }

        public TweenBuilder BindCurrent(in EntityCommandBuffer ecb, in Entity e)
        {
            var delayeEntity = ecb.CreateEntity();

            switch (type)
            {

                case TweenType.Rot:
                    rotTween.targetEntity = e;
                    ecb.AddComponent(delayeEntity, rotTween);
                    break;
                case TweenType.Scale:
                    scaleTween.targetEntity = e;
                    ecb.AddComponent(delayeEntity, scaleTween);
                    break;
                case TweenType.Move:
                    moveTween.targetEntity = e;
                    ecb.AddComponent(delayeEntity, moveTween);
                    break;
                default:
                    break;
            }     
            return this;
        }   
    }
}