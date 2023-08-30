using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Timespawn.EntityTween.Tweens
{
    public unsafe ref struct TweenBuilder
    {




        internal struct BuilderData
        {
            internal NativeList<DelayedMoveTween> moveTweens;
            internal NativeList<DelayedRotationTween> rotTweens;
            internal NativeList<DelayedScaleTween> scaleTweens;


        }

        private bool inited;
        internal BuilderData data;

        public void Dispose()
        {
            data.moveTweens.Dispose();
            data.rotTweens.Dispose();
            data.scaleTweens.Dispose();
        }

        public TweenBuilder(int initListsSize = 1)
        {
            data = default(BuilderData);
            inited = false;

            Init(initListsSize);
        }

        private void Init(int initListsSize = 1)
        {
            if (inited)
                return;

            inited = true;
           

            data.moveTweens = new NativeList<DelayedMoveTween>(initListsSize, Allocator.Persistent);
            data.rotTweens = new NativeList<DelayedRotationTween>(initListsSize, Allocator.Persistent);
            data.scaleTweens = new NativeList<DelayedScaleTween>(initListsSize, Allocator.Persistent);
        }



        public TweenBuilder Move(float duration, float3 start, float3 end, float startTime = 0)
        {
            Init();

            var command = Tween.CreatedDelayedMoveComponentInternal(start, end, duration, startTweenTime: startTime);

            data.moveTweens.Add(command);

            return this;
        }

        public void Build(in EntityManager entityManager, in Entity e)
        {
            Init();

            var bufferMove = entityManager.AddBuffer<DelayedMoveTween>(e);
            bufferMove.AddRange(data.moveTweens.AsArray());

            var bufferRot = entityManager.AddBuffer<DelayedRotationTween>(e);
            bufferRot.AddRange(data.rotTweens.AsArray());

            var bufferScale = entityManager.AddBuffer<DelayedScaleTween>(e);
            bufferScale.AddRange(data.scaleTweens.AsArray());

            Dispose();
        }

        public void Build(in EntityCommandBuffer.ParallelWriter ecb, in int sortKey, in Entity e)
        {
            var bufferMove = ecb.AddBuffer<DelayedMoveTween>(sortKey, e);
            bufferMove.AddRange(data.moveTweens.AsArray());

            var bufferRot = ecb.AddBuffer<DelayedRotationTween>(sortKey, e);
            bufferRot.AddRange(data.rotTweens.AsArray());

            var bufferScale = ecb.AddBuffer<DelayedScaleTween>(sortKey, e);
            bufferScale.AddRange(data.scaleTweens.AsArray());

            Dispose();
        }
        public void Build(in EntityCommandBuffer ecb, in Entity e)
        {

            var bufferMove = ecb.AddBuffer<DelayedMoveTween>(e);
            bufferMove.AddRange(data.moveTweens.AsArray());

            var bufferRot = ecb.AddBuffer<DelayedRotationTween>(e);
            bufferRot.AddRange(data.rotTweens.AsArray());

            var bufferScale = ecb.AddBuffer<DelayedScaleTween>(e);
            bufferScale.AddRange(data.scaleTweens.AsArray());

            Dispose();
        }
    }
}