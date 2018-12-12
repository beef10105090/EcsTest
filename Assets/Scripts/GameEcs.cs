using System;
using System.Collections;
using System.Collections.Generic;
using SpeedTest.ECSTest;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GameEcs : MonoBehaviour {

    EntityManager _manager;
    [SerializeField]
    GameObject _ecsPrefab;
    int _num = 1000;
    // Use this for initialization
    void Start () {
        _manager = World.Active.GetOrCreateManager<EntityManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            createEntity(_num);
        }

        _fame = (int)(1 / Time.deltaTime);
    }

    int _fame;

    private void OnGUI()
    {
        GUI.TextField(new Rect(0, 0, 150, 40), _fame + "");
    }

    private void createEntity(int num)
    {
        NativeArray<Entity> entities = new NativeArray<Entity>(num, Allocator.Temp);
        _manager.Instantiate(_ecsPrefab, entities);

        for (int i = 0; i < num; i++)
        {
            float xVal = UnityEngine.Random.Range(-10, 10);
            float yVal = UnityEngine.Random.Range(-10, 10);
            _manager.SetComponentData(entities[i], new Unity.Transforms.Position { Value = new float3(xVal, yVal, 0) });
            _manager.SetComponentData(entities[i], new MvCompData { Speed = UnityEngine.Random.Range(0.5f, 1), 
                Dir = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI)
            });
        }
        entities.Dispose();

    }
}
