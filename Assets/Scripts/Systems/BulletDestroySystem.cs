using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial struct BulletDestroySystem : ISystem
{
    EntityQuery m_BulletQuery;
    
    public void OnCreate(ref SystemState state)
    {
        m_BulletQuery = state.EntityManager.CreateEntityQuery(typeof(Bullet), typeof(LocalTransform));
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // destroy bullets that are out of bounds- absolute x-40, y-40, z-40
        if (m_BulletQuery.CalculateChunkCount() == 0)
        {
            return;
        }

        var commandBufferSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var commandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        var entities = m_BulletQuery.ToEntityArray(Allocator.TempJob);
        var localTransforms = m_BulletQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        
        for (var i = 0; i < entities.Length; i++)
        {
            var localTransform = localTransforms[i];
            if (localTransform.Position.x < -40 || localTransform.Position.x > 40 ||
                localTransform.Position.y < -40 || localTransform.Position.y > 40 ||
                localTransform.Position.z < -40 || localTransform.Position.z > 40)
            {
                commandBuffer.DestroyEntity(entities[i]);
            }
        }
    }
}
