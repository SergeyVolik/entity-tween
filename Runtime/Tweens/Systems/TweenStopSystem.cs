﻿using Unity.Entities;

namespace Timespawn.EntityTween.Tweens
{
    [UpdateInGroup(typeof(TweenSimulationSystemGroup))]
    [UpdateAfter(typeof(TweenStateSystem))]
    [UpdateBefore(typeof(TweenDestroySystemGroup))]
    internal partial class TweenStopSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<EnableTweensT>();
        }
        protected override void OnUpdate()
        {
           var destroyBufferFromEntity = SystemAPI.GetBufferLookup<TweenDestroyCommand>(true);

            EndSimulationEntityCommandBufferSystem endSimECBSystem = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
            EntityCommandBuffer.ParallelWriter parallelWriter = endSimECBSystem.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithReadOnly(destroyBufferFromEntity)
                .WithAll<TweenStopCommand>()
                .ForEach((int entityInQueryIndex, Entity entity, ref DynamicBuffer<TweenState> tweenBuffer) =>
                {
                    for (int i = 0; i < tweenBuffer.Length; i++)
                    {
                        TweenState tween = tweenBuffer[i];
                        if (!destroyBufferFromEntity.HasBuffer(entity))
                        {
                            parallelWriter.AddBuffer<TweenDestroyCommand>(entityInQueryIndex, entity);
                        }

                        parallelWriter.AppendToBuffer(entityInQueryIndex, entity, new TweenDestroyCommand(tween.Id));
                    }

                    parallelWriter.RemoveComponent<TweenStopCommand>(entityInQueryIndex, entity);

                    if (SystemAPI.HasComponent<TweenPause>(entity))
                    {
                        parallelWriter.RemoveComponent<TweenPause>(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();

            endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}