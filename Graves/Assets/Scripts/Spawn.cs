using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject obj;
    public int rate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.frameCount % rate == 0)
        {
            GameObject o = Instantiate(obj);
            o.transform.position = transform.position + (Vector3.right * Random.Range(-8f,8f));
            o.GetComponent<Graves.Character.Base>().MyPosition = o.transform.position;
        }
	}
}
