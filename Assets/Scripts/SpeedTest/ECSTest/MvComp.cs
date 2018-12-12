using System;
using Unity.Entities;
using Unity.Mathematics;

namespace SpeedTest.ECSTest
{
    [Serializable]
    public struct MvCompData : IComponentData
    {
        public float Speed;
        public float Dir;
    }

    [UnityEngine.DisallowMultipleComponent]
    public class MvComp : ComponentDataWrapper<MvCompData>
    {
    }
}

