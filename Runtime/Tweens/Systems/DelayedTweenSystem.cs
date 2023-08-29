using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Timespawn.EntityTween.Tweens
{
    internal struct DelayedMoveTween : IBufferElementData
    {
        public float startTime;
        public bool StartFromEntityPos;
        public TweenTranslationCommand command;
    }

    public partial struct DelayedTweenSystem : ISystem
    {
        private EntityQuery query;

        public void OnCreate(ref SystemState state)
        {
            query = SystemAPI.QueryBuilder().WithAll<DelayedMoveTween>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();


            state.Dependency = new DelayedMoveTweenJob()
            {
                ecb = ecb,
                bufferHandle = SystemAPI.GetBufferTypeHandle<DelayedMoveTween>(isReadOnly: false),
                entityHandle = SystemAPI.GetEntityTypeHandle(),
                elapsedTime = (float)elapsedTime,
                ltwHandle = SystemAPI.GetComponentTypeHandle<LocalToWorld>(isReadOnly: true)
            }.ScheduleParallel(query, state.Dependency);
        }

        public partial struct DelayedMoveTweenJob : IJobChunk
        {
            public float elapsedTime;


            internal BufferTypeHandle<DelayedMoveTween> bufferHandle;

            [ReadOnly]
            internal ComponentTypeHandle<LocalToWorld> ltwHandle;

            internal EntityCommandBuffer.ParallelWriter ecb;

            [ReadOnly] internal EntityTypeHandle entityHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var bufferAccesor = chunk.GetBufferAccessor<DelayedMoveTween>(ref bufferHandle);
                NativeArray<Entity> entities = chunk.GetNativeArray(entityHandle);
                NativeArray<LocalToWorld> entitiesWorldPos = chunk.GetNativeArray(ref ltwHandle);

                for (int i = 0; i < entities.Length; i++)
                {
                    int index = -1;
                    var buffer = bufferAccesor[i];

                    for (int j = 0; j < buffer.Length; j++)
                    {

                        if (buffer[j].startTime <= elapsedTime)
                        {
                            index = j;

                            var command = buffer[j].command;

                            if (buffer[index].StartFromEntityPos)
                            {

                                command.Start = entitiesWorldPos[i].Position;


                            }

                            ecb.AddComponent(unfilteredChunkIndex, entities[i], command);

                            break;
                        }
                    }

                    if (index != -1)
                    {
                        buffer.RemoveAtSwapBack(index);
                    }

                }
            }
        }
    }
}
