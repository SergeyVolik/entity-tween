using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Timespawn.EntityTween.Math;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Timespawn.EntityTween.Tweens
{
    public unsafe ref struct TweenBuilder
    {


        DelayedMoveTween moveTween;
        DelayedRotationTween rotTween;
        DelayedScaleTween scaleTween;
        TweenType type;
        private DynamicBuffer<DelayedRotationTween> rotBuffer;
        private DynamicBuffer<DelayedScaleTween> scaleBuffer;
        private DynamicBuffer<DelayedMoveTween> moveBuffer;
        private Entity lastEntity;
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
         in EaseDesc easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f,
         in BlobAssetReference<CurveECS> curve = default)
        {

            moveTween = Tween.CreateMoveCommand(start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTweenTime: startTweenTime, curve: curve);
            type = TweenType.Move;
            return this;
        }

        public TweenBuilder CreateScaleTween(in float3 start,
         in float3 end,
         in float duration,
         in EaseDesc easeDesc = default,
         in bool isPingPong = false,
         in int loopCount = 1,
         in float startDelay = 0.0f,
         in float startTweenTime = 0.0f)
        {

            scaleTween = Tween.CreateScaleCommand(start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTweenTime);
            type = TweenType.Scale;

            return this;
        }

        public TweenBuilder CreateRotateTween(
            in quaternion start,
           in quaternion end,
           in float duration,
           in EaseDesc easeDesc = default,
           in bool isPingPong = false,
           in int loopCount = 1,
           in float startDelay = 0.0f,
           in float startTime = 0.0f)
        {


            rotTween = Tween.CreateRotationCommand(start, end, duration, easeDesc, isPingPong, loopCount, startDelay, startTime: startTime);
            type = TweenType.Rot;



            return this;
        }


        public TweenBuilder BindCurrent(in EntityManager entityManager, in Entity e)
        {
            CheckEntity(e);
            switch (type)
            {

                case TweenType.Rot:
                    rotBuffer = GetBuffer(e, entityManager, rotBuffer);
                    rotBuffer.Add(rotTween);
                    break;
                case TweenType.Scale:

                    scaleBuffer = GetBuffer(e, entityManager, scaleBuffer);
                    scaleBuffer.Add(scaleTween);
                    break;
                case TweenType.Move:
                    moveBuffer = GetBuffer(e, entityManager, moveBuffer);
                    moveBuffer.Add(moveTween);
                    break;
                default:
                    break;
            }

            return this;

        }

        public TweenBuilder BindCurrent(in EntityCommandBuffer.ParallelWriter ecb, in int sortKey, in Entity e)
        {
            CheckEntity(e);
            switch (type)
            {
             
                case TweenType.Rot:
                    rotBuffer = GetBuffer(e, sortKey, ecb, rotBuffer);
                    rotBuffer.Add(rotTween);
                    break;
                case TweenType.Scale:

                    scaleBuffer = GetBuffer(e, sortKey, ecb, scaleBuffer);
                    scaleBuffer.Add(scaleTween);
                    break;
                case TweenType.Move:
                    moveBuffer = GetBuffer(e, sortKey, ecb, moveBuffer);
                    moveBuffer.Add(moveTween);
                    break;
                default:
                    break;
            }

            return this;


        }

       

        public TweenBuilder BindCurrent(in EntityCommandBuffer ecb, in Entity e)
        {
            CheckEntity(e);

            switch (type)
            {

                case TweenType.Rot:
                    rotBuffer = GetBuffer(e, ecb, rotBuffer);
                    rotBuffer.Add(rotTween);
                    break;
                case TweenType.Scale:

                    scaleBuffer = GetBuffer(e, ecb, scaleBuffer);
                    scaleBuffer.Add(scaleTween);
                    break;
                case TweenType.Move:
                    moveBuffer = GetBuffer(e, ecb, moveBuffer);
                    moveBuffer.Add(moveTween);
                    break;
                default:
                    break;
            }

            lastEntity = e;
            return this;


        }

        private void CheckEntity(Entity e)
        {
            if (lastEntity != e)
            {
                rotBuffer = default;
                scaleBuffer = default;
                moveBuffer = default;

            }
        }

        private DynamicBuffer<TElem> GetBuffer<TElem>(Entity e, in EntityCommandBuffer ecb, DynamicBuffer<TElem> current) where TElem : unmanaged, IBufferElementData
        {
            
            if (e == lastEntity && current.IsCreated)
            {
                return current;
            }

            return ecb.AddBuffer<TElem>(e);
        }

        private DynamicBuffer<TElem> GetBuffer<TElem>(Entity e, in EntityManager manager, DynamicBuffer<TElem> current) where TElem : unmanaged, IBufferElementData
        {
            if (e == lastEntity && current.IsCreated)
            {
                return current;
            }

            return manager.AddBuffer<TElem>(e);
        }

        private DynamicBuffer<TElem> GetBuffer<TElem>(Entity e, in int sortKey, in EntityCommandBuffer.ParallelWriter ecb, DynamicBuffer<TElem> current) where TElem : unmanaged, IBufferElementData
        {
            if (e == lastEntity && current.IsCreated)
            {
                return current;
            }

            return ecb.AddBuffer<TElem>(sortKey, e);
        }
    }
}