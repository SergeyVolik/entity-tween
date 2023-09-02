using System;
using System.Runtime.CompilerServices;
using Timespawn.EntityTween.Math;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


[assembly: InternalsVisibleTo("Timespawn.EntityTween.Tests")]

namespace Timespawn.EntityTween.Tweens
{




    public static class Tween
    {
        public const byte Infinite = TweenState.LOOP_COUNT_INFINITE;

        public static void Move(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Move(entityManager, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Move(commandBuffer, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Move(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void Move(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void DelayedMove(
          in EntityCommandBuffer commandBuffer,

          in Entity entity,
          in float3 start,
          in float3 end,
          in float duration,
          in EaseDesc easeDesc = default,
          in bool isPingPong = false,
          in int loopCount = 1,
          in float startDelay = 0.0f,
          in float startTweenTime = 0.0f,
          in bool startFromEntityPos = false,
          in BlobAssetReference<CurveECS> curve = default)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            var buffer = commandBuffer.AddBuffer<DelayedMoveTween>(entity);


            buffer.Add(CreateMoveCommand(start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTweenTime, startFromEntityPos, curve));

        }

        internal static DelayedMoveTween CreateMoveCommand(
         in float3 start,
         in float3 end,
         in float duration,
         in EaseDesc easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f,
         in bool startFromEntityPos = false,
         BlobAssetReference<CurveECS> curve = default)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                throw new Exception("easeDesc param is invalid");
            }

            return new DelayedMoveTween
            {
                command = CreateMoveCommandInternal(start, end, duration, easeDesc, isPingPong, loopCount, startDelay, curve),
                StartFromEntityPos = startFromEntityPos,
                startTime = startTweenTime

            };

        }

        internal static TweenMoveCommand CreateMoveCommandInternal(
           in float3 start,
           in float3 end,
           in float duration,
           in EaseDesc easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           BlobAssetReference<CurveECS> curve = default)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay, curve: curve);
            return new TweenMoveCommand(tweenParams, start, end);
        }


        public static void Move(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityManager entityManager,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(entityManager, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(commandBuffer, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityManager entityManager,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Scale(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(entityManager, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(commandBuffer, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenScaleCommand(tweenParams, start, end));
        }

        internal static DelayedScaleTween CreateScaleCommand(
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f,
            in float startTime = 0.0f)
        {
            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            return new DelayedScaleTween
            {
                command = new TweenScaleCommand(tweenParams, start, end),
                startTime = startTime
            };
        }

        internal static DelayedRotationTween CreateRotationCommand(
           in quaternion start,
           in quaternion end,
           in float duration,
           in EaseDesc easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           in float startTime = 0.0f)
        {
            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            return new DelayedRotationTween
            {
                command = new TweenRotationCommand(tweenParams, start, end),
                startTime = startTime
            };
        }

        public static void Scale(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenScaleCommand(tweenParams, start, end));
        }

        public static void Scale(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenScaleCommand(tweenParams, start, end));
        }

#if UNITY_TINY_ALL_0_31_0 || UNITY_2D_ENTITIES

        public static void Tint(
            in EntityManager entityManager, 
            in Entity entity, 
            in float4 start, 
            in float4 end, 
            in TweenParams tweenParams)
        {
            Tint(entityManager, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Tint(
            in EntityCommandBuffer commandBuffer, 
            in Entity entity, 
            in float4 start, 
            in float4 end, 
            in TweenParams tweenParams)
        {
            Tint(commandBuffer, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Tint(
            in EntityCommandBuffer.ParallelWriter parallelWriter, 
            in int sortKey, 
            in Entity entity, 
            in float4 start, 
            in float4 end, 
            in TweenParams tweenParams)
        {
            Tint(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, new EaseDesc(tweenParams.EaseType, tweenParams.EaseExponent), tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Tint(
            in EntityManager entityManager,
            in Entity entity,
            in float4 start, 
            in float4 end, 
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false, 
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenTintCommand(tweenParams, start, end));
        }

        public static void Tint(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float4 start, 
            in float4 end, 
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false, 
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenTintCommand(tweenParams, start, end));
        }

        public static void Tint(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float4 start, 
            in float4 end, 
            in float duration,
            in EaseDesc easeDesc = default,
            in bool isPingPong = false, 
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {
            if (!CheckParams(easeDesc.Exponent, loopCount))
            {
                return;
            }

            TweenParams tweenParams = new TweenParams(duration, easeDesc.Type, easeDesc.Exponent, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenTintCommand(tweenParams, start, end));
        }

#endif

        public static void Pause(in EntityManager entityManager, in Entity entity)
        {
            entityManager.AddComponent<TweenPause>(entity);
        }

        public static void Pause(in EntityCommandBuffer commandBuffer, in Entity entity)
        {
            commandBuffer.AddComponent<TweenPause>(entity);
        }

        public static void Pause(in EntityCommandBuffer.ParallelWriter parallelWriter, in int sortKey, in Entity entity)
        {
            parallelWriter.AddComponent<TweenPause>(sortKey, entity);
        }

        public static void Resume(in EntityManager entityManager, in Entity entity)
        {
            entityManager.AddComponent<TweenResumeCommand>(entity);
        }

        public static void Resume(in EntityCommandBuffer commandBuffer, in Entity entity)
        {
            commandBuffer.AddComponent<TweenResumeCommand>(entity);
        }

        public static void Resume(in EntityCommandBuffer.ParallelWriter parallelWriter, in int sortKey, in Entity entity)
        {
            parallelWriter.AddComponent<TweenResumeCommand>(sortKey, entity);
        }

        public static void Stop(in EntityManager entityManager, in Entity entity)
        {
            entityManager.AddComponent<TweenStopCommand>(entity);
        }

        public static void Stop(in EntityCommandBuffer commandBuffer, in Entity entity)
        {
            commandBuffer.AddComponent<TweenStopCommand>(entity);
        }

        public static void Stop(in EntityCommandBuffer.ParallelWriter parallelWriter, in int sortKey, in Entity entity)
        {
            parallelWriter.AddComponent<TweenStopCommand>(sortKey, entity);
        }

        private static bool CheckParams(in int easeExponent, in int loopCount)
        {
            if (easeExponent < byte.MinValue || easeExponent > byte.MaxValue)
            {
                return false;
            }

            if (loopCount < byte.MinValue || loopCount > byte.MaxValue)
            {
                return false;
            }

            return true;
        }
    }
}