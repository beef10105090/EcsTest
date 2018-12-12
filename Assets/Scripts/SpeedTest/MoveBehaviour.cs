using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {
    public float Speed = 1;
    public float Dir = 0.3f;

	// Use this for initialization
	void Awake () {
        Speed = Random.Range(0.5f, 1);
        Dir = Random.Range(-Mathf.PI, Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = new Vector3(Mathf.Cos(Dir) * Speed, Mathf.Sin(Dir) * Speed);
        //bool mirror = false;
		if(transform.position.x + vec.x > 10 || transform.position.x + vec.x < -10)
        {
            vec.x = -vec.x;
            Dir = Mathf.Atan2(vec.y, vec.x);
            //mirror = true;
        }
        if (transform.position.y + vec.y > 10 || transform.position.y + vec.y < -10)
        {
            vec.y = -vec.y;
            Dir = Mathf.Atan2(vec.y, vec.x);
            //mirror = true;
        }
        transform.position += vec;
    }
}
