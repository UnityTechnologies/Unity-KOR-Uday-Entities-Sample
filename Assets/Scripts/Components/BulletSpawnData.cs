using Unity.Entities;

public struct BulletSpawnData : IComponentData
{
    public Entity BulletPrefab;
    public int BulletsPerSpawn;
    public double TimeBetweenSpawns;
    public double LastSpawnTime;
    public double BulletSpeed;
}
