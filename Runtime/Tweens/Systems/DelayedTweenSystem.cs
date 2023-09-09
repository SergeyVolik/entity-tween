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
    internal partial class DelayedTweenMoveSystem : AddComponentWithDelay<DelayedMoveTween, TweenMoveCommand> { }
    internal partial class DelayedTweenScaleSystem : AddComponentWithDelay<DelayedScaleTween, TweenScaleCommand> { }
    internal partial class DelayedTweenRotationSystem : AddComponentWithDelay<DelayedRotationTween, TweenRotationCommand> { }

    public interface IDelayTime
    {
        public float GetActivationTime();
      
    }

    public interface IEntityLink
    {
        public Entity GetEntityRef();

    }

    public interface IDelayedRemoveComponentDataCommand : IComponentData, IDelayTime, IEntityLink
    {
       
    }

    public interface IDelayedEnableDisableComponentDataCommand : IComponentData, IDelayTime, IEntityLink
    {
        public bool GetEnableState();
    }

    public interface IDelayedAddComponentDataCommand<TComponent> : IComponentData, IDelayTime, IEntityLink where TComponent : unmanaged, IComponentData
    {      
        public TComponent GetCommand();

        
    }


    internal struct DelayedMoveTween : IDelayedAddComponentDataCommand<TweenMoveCommand>
    {
        public float startTime;
        public bool StartFromEntityPos;
        public TweenMoveCommand command;
      
        public float GetActivationTime() => startTime;
        public TweenMoveCommand GetCommand() => command;
        public Entity targetEntity;
        public Entity GetEntityRef() => targetEntity;
    }

    internal struct DelayedRotationTween : IDelayedAddComponentDataCommand<TweenRotationCommand>
    {
        public float startTime;
        public TweenRotationCommand command;

        public float GetActivationTime() => startTime;
        public TweenRotationCommand GetCommand() => command;

        public Entity targetEntity;
        public Entity GetEntityRef() => targetEntity;
    }

    public struct DelayedScaleTween : IDelayedAddComponentDataCommand<TweenScaleCommand>
    {
        public float startTime;
        public TweenScaleCommand command;

        public float GetActivationTime() => startTime;
        public TweenScaleCommand GetCommand() => command;

        public Entity targetEntity;
        public Entity GetEntityRef() => targetEntity;
    }
    public abstract partial class RemoveComponentWithDelaySystem<TDelayedActivator, TToRemove> : SystemBase
        where TDelayedActivator : unmanaged, IComponentData, IDelayedRemoveComponentDataCommand
        where TToRemove : unmanaged, IComponentData
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


            Dependency = new RemoveComponentJob()
            {
                ecb = ecb,
                typeHandle = GetComponentTypeHandle<TDelayedActivator>(isReadOnly: false),
                entityHandle = SystemAPI.GetEntityTypeHandle(),
                elapsedTime = (float)elapsedTime,
            }.ScheduleParallel(query, Dependency);
        }


        internal partial struct RemoveComponentJob : IJobChunk
        {
            public float elapsedTime;


            internal ComponentTypeHandle<TDelayedActivator> typeHandle;

            internal EntityCommandBuffer.ParallelWriter ecb;

            [ReadOnly] internal EntityTypeHandle entityHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {

                var components = chunk.GetNativeArray(ref typeHandle);
                NativeArray<Entity> entities = chunk.GetNativeArray(entityHandle);


                for (int i = 0; i < entities.Length; i++)
                {
                    var compData = components[i];

                    if (compData.GetActivationTime() <= elapsedTime)
                    {
                        var e = compData.GetEntityRef();

                        ecb.RemoveComponent<TToRemove>(unfilteredChunkIndex, e);

                        ecb.DestroyEntity(unfilteredChunkIndex, entities[i]);
                    }

                }
            }
        }
    }

    public abstract partial class EnableDisabletWithDelaySystem<TDelayedActivator, TTarget> : SystemBase
        where TDelayedActivator : unmanaged, IComponentData, IDelayedEnableDisableComponentDataCommand
        where TTarget : unmanaged, IComponentData, IEnableableComponent
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


            Dependency = new EnableDisableComponentJob()
            {
                ecb = ecb,
                typeHandle = GetComponentTypeHandle<TDelayedActivator>(isReadOnly: false),
                entityHandle = SystemAPI.GetEntityTypeHandle(),
                elapsedTime = (float)elapsedTime,
            }.ScheduleParallel(query, Dependency);
        }


        internal partial struct EnableDisableComponentJob : IJobChunk
        {
            public float elapsedTime;


            internal ComponentTypeHandle<TDelayedActivator> typeHandle;

            internal EntityCommandBuffer.ParallelWriter ecb;

            [ReadOnly] internal EntityTypeHandle entityHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {

                var components = chunk.GetNativeArray(ref typeHandle);
                NativeArray<Entity> entities = chunk.GetNativeArray(entityHandle);


                for (int i = 0; i < entities.Length; i++)
                {
                    var compData = components[i];

                    if (compData.GetActivationTime() <= elapsedTime)
                    {
                        var e = compData.GetEntityRef();

                        ecb.SetComponentEnabled<TTarget>(unfilteredChunkIndex, e, compData.GetEnableState());

                        ecb.DestroyEntity(unfilteredChunkIndex, entities[i]);
                    }

                }
            }
        }
    }

    public abstract partial class AddComponentWithDelay<TDelayedActivator, TCommand> : SystemBase
        where TDelayedActivator : unmanaged, IComponentData, IDelayedAddComponentDataCommand<TCommand>
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
                typeHandle = GetComponentTypeHandle<TDelayedActivator>(isReadOnly: false),
                entityHandle = SystemAPI.GetEntityTypeHandle(),
                elapsedTime = (float)elapsedTime,
            }.ScheduleParallel(query, Dependency);
        }


        internal partial struct AddComponentJob : IJobChunk
        {
            public float elapsedTime;


            internal ComponentTypeHandle<TDelayedActivator> typeHandle;

            internal EntityCommandBuffer.ParallelWriter ecb;

            [ReadOnly] internal EntityTypeHandle entityHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                
                var components = chunk.GetNativeArray(ref typeHandle);
                NativeArray<Entity> entities = chunk.GetNativeArray(entityHandle);


                for (int i = 0; i < entities.Length; i++)
                {
                    var compData = components[i];

                    if (compData.GetActivationTime() <= elapsedTime)
                    {
                        var command = compData.GetCommand();

                        ecb.AddComponent(unfilteredChunkIndex, compData.GetEntityRef(), command);

                        ecb.DestroyEntity(unfilteredChunkIndex, entities[i]);
                    }

                }
            }
        }
    }
}
