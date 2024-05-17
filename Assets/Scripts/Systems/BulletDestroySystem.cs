using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletDestroySystem : ISystem
{
    EntityQuery _bulletQuery;
    
    public void OnCreate(ref SystemState state)
    {
        _bulletQuery = state.EntityManager.CreateEntityQuery(typeof(Bullet), typeof(LocalTransform));
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (_bulletQuery.CalculateChunkCount() == 0)
        {
            return;
        }

        var commandBufferSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var commandBuffer = commandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        var entities = _bulletQuery.ToEntityArray(Allocator.TempJob);
        var localTransforms = _bulletQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        
        for (var i = 0; i < entities.Length; i++)
        {
            var localTransform = localTransforms[i];
            if (math.abs(localTransform.Position.x) >= 40 ||
                math.abs(localTransform.Position.y) >= 40 ||
                math.abs(localTransform.Position.z) >= 40)
            {
                // DestroyEntity() 사용시 해당 엔티티가 즉시 파괴 -> 엔티티 컬렉션을 순회하면서 엔티티 파괴는 불가능
                // state.EntityManager.DestroyEntity(entities[i]);
                
                // CommandBuffer.DestroyEntity()를 사용하면 프레임의 끝으로 미루어 파괴 가능
                commandBuffer.DestroyEntity(entities[i]);
            }
        }
    }
}
