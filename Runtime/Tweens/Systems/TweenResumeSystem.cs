using Unity.Entities;

namespace Timespawn.EntityTween.Tweens
{
    [UpdateInGroup(typeof(TweenSimulationSystemGroup))]
    [UpdateAfter(typeof(TweenStateSystem))]
    internal partial class TweenResumeSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            var ecs = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
            //EntityCommandBuffer.ParallelWriter parallelWriter = endSimECBSystem.CreateCommandBuffer(World.Unmanaged).AsParallelWriter();
            //ComponentLookup<TweenPause> pauseFromEntity = GetComponentLookup<TweenPause>();

            //Entities
            //    .WithReadOnly(pauseFromEntity)
            //    .WithAll<TweenResumeCommand>()
            //    .ForEach((int entityInQueryIndex, Entity entity) =>
            //    {
            //        if (pauseFromEntity.HasComponent(entity))
            //        {
            //            parallelWriter.RemoveComponent<TweenPause>(entityInQueryIndex, entity);
            //        }

            //        parallelWriter.RemoveComponent<TweenResumeCommand>(entityInQueryIndex, entity);
            //    }).ScheduleParallel();


        }
    }
}