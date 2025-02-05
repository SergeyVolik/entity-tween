﻿using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.TweenTranslationDestroySystem.DestroyJob))]
[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.TweenRotationDestroySystem.DestroyJob))]
[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.TweenScaleDestroySystem.DestroyJob))]


namespace Timespawn.EntityTween.Tweens
{
    [UpdateInGroup(typeof(TweenDestroySystemGroup))]
    internal  abstract partial class TweenDestroySystem<TTweenInfo> : SystemBase 
        where TTweenInfo : unmanaged, IComponentData, ITweenId
    {
        [BurstCompile]
        internal struct DestroyJob : IJobChunk
        {
            [ReadOnly] public EntityTypeHandle EntityType;
            [ReadOnly] public ComponentTypeHandle<TTweenInfo> InfoType;

            [NativeDisableContainerSafetyRestriction] public BufferTypeHandle<TweenState> TweenBufferType;
            [NativeDisableContainerSafetyRestriction] public BufferTypeHandle<TweenDestroyCommand> DestroyCommandType;

            public EntityCommandBuffer.ParallelWriter ParallelWriter;

          

            public void Execute(in ArchetypeChunk chunk, int chunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(EntityType);
                NativeArray<TTweenInfo> infos = chunk.GetNativeArray(ref InfoType);
                BufferAccessor<TweenState> tweenBuffers = chunk.GetBufferAccessor(ref TweenBufferType);
                BufferAccessor<TweenDestroyCommand> destroyBuffers = chunk.GetBufferAccessor(ref DestroyCommandType);
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity entity = entities[i];

                    bool shouldDestroy = false;
                    DynamicBuffer<TweenDestroyCommand> destroyBuffer = destroyBuffers[i];
                    for (int j = destroyBuffer.Length - 1; j >= 0; j--)
                    {
                        TweenDestroyCommand command = destroyBuffer[j];
                        if (infos[i].GetTweenId() == command.Id)
                        {
                            shouldDestroy = true;
                            destroyBuffer.RemoveAt(j);
                        }
                    }

                    if (!shouldDestroy)
                    {
                        // Shouldn't go here
                        continue;
                    }

                    DynamicBuffer<TweenState> tweenBuffer = tweenBuffers[i];
                    for (int j = tweenBuffer.Length - 1; j >= 0; j--)
                    {
                        TweenState tween = tweenBuffer[j];
                        if (infos[i].GetTweenId() == tween.Id)
                        {
                            tweenBuffer.RemoveAt(j);
                            ParallelWriter.RemoveComponent<TTweenInfo>(chunkIndex, entity);
                            break;
                        }
                    }

                    if (tweenBuffer.IsEmpty)
                    {
                        ParallelWriter.RemoveComponent<TweenState>(chunkIndex, entity);
                    }

                    if (destroyBuffer.IsEmpty)
                    {
                        ParallelWriter.RemoveComponent<TweenDestroyCommand>(chunkIndex, entity);
                    }
                }
            }
        }

        private EntityQuery TweenInfoQuery;

        protected override void OnCreate()
        {
            RequireForUpdate<EnableTweensT>();

            TweenInfoQuery = GetEntityQuery(
                ComponentType.ReadOnly<TTweenInfo>(),
                ComponentType.ReadOnly<TweenState>(),
                ComponentType.ReadOnly<TweenDestroyCommand>());
        }

        protected override void OnUpdate()
        {
            EndSimulationEntityCommandBufferSystem endSimECBSystem = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();

            DestroyJob job = new DestroyJob
            {
                EntityType = GetEntityTypeHandle(),
                InfoType = GetComponentTypeHandle<TTweenInfo>(true),
                TweenBufferType = GetBufferTypeHandle<TweenState>(),
                DestroyCommandType = GetBufferTypeHandle<TweenDestroyCommand>(),
                ParallelWriter = endSimECBSystem.CreateCommandBuffer().AsParallelWriter(),
            };

            Dependency = job.ScheduleParallel(TweenInfoQuery, Dependency);
            endSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }

    internal partial class TweenTranslationDestroySystem : TweenDestroySystem<TweenMove> {}
    internal partial class TweenRotationDestroySystem : TweenDestroySystem<TweenRotation> {}
    internal partial class TweenScaleDestroySystem : TweenDestroySystem<TweenScale> {}

}