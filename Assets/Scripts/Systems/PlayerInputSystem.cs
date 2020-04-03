using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem] // tell the thread we are not doing asynchronous work 
public class PlayerInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref PaddleMovementData moveData, in PaddleInputData inputData) =>
        {
            moveData.direction = 0;
            // because we reference Input.GetKey, this has to be on the main thread !
            moveData.direction += Input.GetKey(inputData.upKey) ? 1 : 0;
            moveData.direction -= Input.GetKey(inputData.downKey) ? 1 : 0;
        }).Run();

        return default; // we are not using inputDeps for anything
    }
}
