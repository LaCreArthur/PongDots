using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PaddleMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float yBound = GameManager.main.yBound;

        //// This is the way of creating a JobComponentSystem, that has a system query and
        //// some executions that are then scheduled to work on a bunch of worker threads
        //// But for just moving a paddle this is overkill, it takes more time to schedule
        //// the job on a worker thread than just do the work on the main thread
        // JobHandle myJob = Entities.ForEach((ref Translation trans, in PaddleMovementData data) =>
        // {
        //     trans.Value.y = Mathf.Clamp(trans.Value.y + (data.speed * data.direction * deltaTime), -yBound, yBound);
        // }).Schedule(inputDeps);
        //
        // return myJob;
        Entities.ForEach((ref Translation trans, in PaddleMovementData data) =>
        {
            trans.Value.y = Mathf.Clamp(trans.Value.y + (data.speed * data.direction * deltaTime), -yBound, yBound);
        }).Run();
        
        return default;
    }
}
