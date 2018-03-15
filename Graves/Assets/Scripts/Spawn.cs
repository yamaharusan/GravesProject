using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.frameCount % 300 == 0)
        {
            GameObject o = Instantiate(obj);
            o.transform.position = transform.position;
        }
	}
}
