using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gui_debug_window : MonoBehaviour {
	public static gui_debug_window main;
	string textLine = "";
	
	public Text text;
	
	// Use this for initialization
	void Start () {
		main = GetComponent<gui_debug_window> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = textLine;
		textLine = "";
	}
	
	public void drawLine(string str){
		textLine += str + "\n";
	}
}
