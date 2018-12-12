using System.Collections;
using System.Collections.Generic;
using SpeedTest.ECSTest;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : JobComponentSystem
{
    [Unity.Burst.BurstCompile]
    struct MovementJob : IJobProcessComponentData<Position, MvCompData>
    {

        public void Execute(ref Position position,  ref MvCompData speed)
        {
            float3 value = position.Value;

            Vector3 vec = new Vector3(Mathf.Cos(speed.Dir) * speed.Speed, Mathf.Sin(speed.Dir) * speed.Speed);
            //bool mirror = false;
            if (value.x + vec.x > 10 || value.x + vec.x < -10)
            {
                vec.x = -vec.x;
                speed.Dir = Mathf.Atan2(vec.y, vec.x);
                //mirror = true;
            }
            if (value.y + vec.y > 10 || value.y + vec.y < -10)
            {
                vec.y = -vec.y;
                speed.Dir = Mathf.Atan2(vec.y, vec.x);
                //mirror = true;
            }

            position.Value = value + new float3(vec.x, vec.y, 0);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MovementJob moveJob = new MovementJob
        {
        };
        JobHandle moveHandle = moveJob.Schedule(this, inputDeps);

        return moveHandle;
    }
}

