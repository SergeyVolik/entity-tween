using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.DelayedTweenMoveSystem.AddComponentJob))]
[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.DelayedTweenScaleSystem.AddComponentJob))]
[assembly: RegisterGenericJobType(typeof(Timespawn.EntityTween.Tweens.DelayedTweenRotationSystem.AddComponentJob))]



namespace Timespawn.EntityTween.Tweens
{
    internal partial class DelayedTweenMoveSystem : AddComponentWithDelay<DelayedMoveTween, TweenTranslationCommand> { }
    internal partial class DelayedTweenScaleSystem : AddComponentWithDelay<DelayedScaleTween, TweenScaleCommand> { }
    internal partial class DelayedTweenRotationSystem : AddComponentWithDelay<DelayedRotationTween, TweenRotationCommand> { }

    internal interface IDelayedCommand<TTweenCommand> : IBufferElementData where TTweenCommand : unmanaged, IComponentData
    {
        public float GetActivationTime();
        public TTweenCommand GetCommand();
    }


    internal struct DelayedMoveTween : IDelayedCommand<TweenTranslationCommand>, IBufferElementData
    {
        public float startTime;
        public bool StartFromEntityPos;
        public TweenTranslationCommand command;

        public float GetActivationTime() => startTime;
        public TweenTranslationCommand GetCommand() => command;
    }

    internal struct DelayedRotationTween : IDelayedCommand<TweenRotationCommand>, IBufferElementData
    {
        public float startTime;
        public TweenRotationCommand command;

        public float GetActivationTime() => startTime;
        public TweenRotationCommand GetCommand() => command;
    }

    public struct DelayedScaleTween : IDelayedCommand<TweenScaleCommand>, IBufferElementData
    {
        public float startTime;
        public TweenScaleCommand command;

        public float GetActivationTime() => startTime;
        public TweenScaleCommand GetCommand() => command;
    }


    internal abstract partial class AddComponentWithDelay<TDelayedActivator, TCommand> : SystemBase
        where TDelayedActivator : unmanaged, IBufferElementData, IDelayedCommand<TCommand>
        where TCommand : unmanaged, IComponentData
    {
        private EntityQuery query;

        protected override void OnCreate()
        {
            query = GetEntityQuery(ComponentType.ReadWrite<TDelayedActivator>());
        }

        protected override void OnUpdate()
        {
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged).AsParallelWriter();


            Dependency = new AddComponentJob()
            {
                ecb = ecb,
                bufferHandle = GetBufferTypeHandle<TDelayedActivator>(isReadOnly: false),
                entityHandle = SystemAPI.GetEntityTypeHandle(),
                elapsedTime = (float)elapsedTime,
                //ltwHandle = SystemAPI.GetComponentTypeHandle<LocalToWorld>(isReadOnly: true)
            }.ScheduleParallel(query, Dependency);
        }


        internal partial struct AddComponentJob : IJobChunk
        {
            public float elapsedTime;


            internal BufferTypeHandle<TDelayedActivator> bufferHandle;

            //[ReadOnly]
            //internal ComponentTypeHandle<LocalToWorld> ltwHandle;

            internal EntityCommandBuffer.ParallelWriter ecb;

            [ReadOnly] internal EntityTypeHandle entityHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var bufferAccesor = chunk.GetBufferAccessor(ref bufferHandle);
                NativeArray<Entity> entities = chunk.GetNativeArray(entityHandle);
                // NativeArray<LocalToWorld> entitiesWorldPos = chunk.GetNativeArray(ref ltwHandle);

                for (int i = 0; i < entities.Length; i++)
                {
                    int index = -1;
                    var buffer = bufferAccesor[i];

                    for (int j = 0; j < buffer.Length; j++)
                    {

                        if (buffer[j].GetActivationTime() <= elapsedTime)
                        {
                            index = j;

                            var command = buffer[j].GetCommand();

                            //if (buffer[index].StartFromEntityPos)
                            //{

                            //    command.Start = entitiesWorldPos[i].Position;


                            //}

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
