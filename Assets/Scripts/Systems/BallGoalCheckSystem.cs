using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class BallGoalCheckSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities
            .WithAll<BallTag>()
            // .WithStructuralChanges() 
            // we can use EntityManager because we say .WithStructuralChanges(),
            // that create a special version of the data that allows us to modify it without invalidating the structural data
            // But it is only ok for very simple jobs like this one but for more complex games, it can create some slow down
            // because it create a specialised copy of the data 
            .WithoutBurst() // we cannot reference GameManager with Burst, no disable it
            .ForEach((Entity entity, in Translation trans) =>
            {
                float3 pos = trans.Value;
                float bound = GameManager.main.xBound;

                if (pos.x >= bound)
                {
                    GameManager.main.PlayerScored(0);
                    // EntityManager.DestroyEntity(entity);
                    ecb.DestroyEntity(entity);
                }
                else if (pos.x <= -bound)
                {
                    GameManager.main.PlayerScored(1);
                    // EntityManager.DestroyEntity(entity);
                    ecb.DestroyEntity(entity);
                }
            }).Run();

        // say to the EntityManager to do all the command that we recorded 
        ecb.Playback(EntityManager);
        ecb.Dispose();
        
        return default;
    }
}
