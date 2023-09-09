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
            Move(entityManager, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Move(commandBuffer, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Move(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Move(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = EaseType.Linear,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {


            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void Move(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = EaseType.Linear,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {


            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void DelayedMove(
          in EntityCommandBuffer commandBuffer,
          in Entity entity,
          in float3 start,
          in float3 end,
          in float duration,
          in EaseType easeDesc = EaseType.Linear,
          in bool isPingPong = false,
          in int loopCount = 1,
          in float startDelay = 0.0f,
          in float startTweenTime = 0.0f,
          in bool startFromEntityPos = false,
          in CurvesXYZ curve = default)
        {
            var timerEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent(timerEntity, CreateMoveCommand(entity, start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTweenTime, startFromEntityPos, curve));

        }

        internal static DelayedMoveTween CreateMoveCommand(
         in Entity targetEntity,
         in float3 start,
         in float3 end,
         in float duration,
         in EaseType easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f,
         in bool startFromEntityPos = false,
         CurvesXYZ curve = default)
        {


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
           in EaseType easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           CurvesXYZ curve = default)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            return new TweenMoveCommand(tweenParams, start, end);
        }


        public static void Move(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {


            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenMoveCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityManager entityManager,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(entityManager, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(commandBuffer, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in TweenParams tweenParams)
        {
            Rotate(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Rotate(
            in EntityManager entityManager,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {


            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Rotate(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in quaternion start,
            in quaternion end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenRotationCommand(tweenParams, start, end));
        }

        public static void Scale(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(entityManager, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(commandBuffer, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in TweenParams tweenParams)
        {
            Scale(parallelWriter, sortKey, entity, start, end, tweenParams.Duration, tweenParams.EaseType, tweenParams.IsPingPong, tweenParams.LoopCount, tweenParams.StartDelay);
        }

        public static void Scale(
            in EntityManager entityManager,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {


            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            entityManager.AddComponentData(entity, new TweenScaleCommand(tweenParams, start, end));
        }

        internal static DelayedScaleTween CreateScaleCommand(
            in Entity targetEntity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f,
            in float startTime = 0.0f)
        {
            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            return new DelayedScaleTween
            {
                command = new TweenScaleCommand(tweenParams, start, end),
                startTime = startTime,
                targetEntity = targetEntity
            };
        }

        internal static DelayedRotationTween CreateRotationCommand(
           in Entity targetEntity,
           in quaternion start,
           in quaternion end,
           in float duration,
           in EaseType easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           in float startTime = 0.0f)
        {
            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            return new DelayedRotationTween
            {
                command = new TweenRotationCommand(tweenParams, start, end),
                startTime = startTime,
                targetEntity = targetEntity
            };
        }

        public static void Scale(
            in EntityCommandBuffer commandBuffer,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            commandBuffer.AddComponent(entity, new TweenScaleCommand(tweenParams, start, end));
        }

        public static void Scale(
            in EntityCommandBuffer.ParallelWriter parallelWriter,
            in int sortKey,
            in Entity entity,
            in float3 start,
            in float3 end,
            in float duration,
            in EaseType easeDesc = default,
            in bool isPingPong = false,
            in int loopCount = 1,
            in float startDelay = 0.0f)
        {

            TweenParams tweenParams = new TweenParams(duration, easeDesc, isPingPong, loopCount, startDelay);
            parallelWriter.AddComponent(sortKey, entity, new TweenScaleCommand(tweenParams, start, end));
        }

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


    }
}