using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

partial struct BulletSpawnSystem : ISystem
{
    private Random _random;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _random = new Random(999);
        state.RequireForUpdate<BulletSpawnData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawnData = SystemAPI.GetSingleton<BulletSpawnData>();

        if (spawnData.NextSpawnTime > SystemAPI.Time.ElapsedTime)
        {
            return;
        }
        
        NativeArray<Entity> bulletEntities = 
            state.EntityManager.Instantiate(spawnData.BulletPrefab, spawnData.BulletsPerSpawn, Allocator.Temp);

        foreach (var bulletEntity in bulletEntities)
        {
            var velocity = _random.NextFloat3Direction() * spawnData.BulletSpeed;
            var movement = new Movement { Velocity = velocity };
            state.EntityManager.SetComponentData(bulletEntity, movement);   
        }
        
        spawnData.NextSpawnTime = SystemAPI.Time.ElapsedTime + spawnData.TimeBetweenSpawns;
        SystemAPI.SetSingleton(spawnData);
    }
}
