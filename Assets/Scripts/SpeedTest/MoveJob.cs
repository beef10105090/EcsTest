using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

[Unity.Burst.BurstCompile]
public struct MoveJob : IJobParallelForTransform
{


    public NativeArray<float> SpeedList;
    public NativeArray<float> DirList;

    public void Execute(int index, TransformAccess transform)
    {
        float Dir = 0;
        float Speed = 0;
        if(index < DirList.Length)
        {
            Dir = DirList[index];
            Speed = SpeedList[index];
            Vector3 vec = new Vector3(Mathf.Cos(Dir) * Speed, Mathf.Sin(Dir) * Speed);
            //bool mirror = false;
            if (transform.position.x + vec.x > 10 || transform.position.x + vec.x < -10)
            {
                vec.x = -vec.x;
                Dir = Mathf.Atan2(vec.y, vec.x);
                DirList[index] = Dir;
                //mirror = true;
            }
            if (transform.position.y + vec.y > 10 || transform.position.y + vec.y < -10)
            {
                vec.y = -vec.y;
                Dir = Mathf.Atan2(vec.y, vec.x);
                DirList[index] = Dir;
                //mirror = true;
            }
            transform.position += vec;
        }

    }
}
