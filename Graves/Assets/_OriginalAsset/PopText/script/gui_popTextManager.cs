using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui_popTextManager : MonoBehaviour {
	public static gui_popTextManager main;

	public GameObject popText; 

	// Use this for initialization
	void Start () {
		main = GetComponent<gui_popTextManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public gui_popText print(string str,Vector2 t){
		gui_popText text = Instantiate(popText).GetComponent<gui_popText>();
		text.Init();

		text.rect.SetParent(transform);

		text.text.text = str;
		text.position = (Vector3)t;
		
		return text;
	}

	public gui_popText print(string str,Vector3 t){
		gui_popText text = Instantiate(popText).GetComponent<gui_popText>();
		text.Init();

		text.rect.SetParent(transform);

		text.text.text = str;
		text.setTarGet(t);
		
		return text;
	}

	public gui_popText print(string str,Transform t){
		gui_popText text = Instantiate(popText).GetComponent<gui_popText>();
		text.Init();

		text.rect.SetParent(transform);

		text.text.text = str;
		text.setTarGet(t);
		
		return text;
	}
}
