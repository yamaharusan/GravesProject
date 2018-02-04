using System;
using UnityEngine;
using System.Collections;

public class gui_debug_3dLine : MonoBehaviour {
	public static gui_debug_3dLine main;

	public GameObject Line3D;

	GameObject[] lineList = new GameObject[1];
	GameObject LineEmpty;

	Color col = Color.green;
	float width = 0.05f;

	int Count = 0;

	// Use this for initialization
	void Start () {
		main = GetComponent<gui_debug_3dLine> ();

		LineEmpty = gameObject;
		//LineEmpty.transform.parent = gameObject.transform;
		//LineEmpty.transform.localPosition = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
		for(int i =0;i<lineList.Length;i++)
			Destroy(lineList[i]);
		
		lineList = new GameObject[1];
		Count = 0;
		col = Color.green;
	}

	public LineRenderer draw(Vector3 a){

		float l = 2.5f;

		col = Color.red;
		draw(a + Vector3.right * l,a - Vector3.right * l);
		col = Color.green;
		draw(a + Vector3.up * l,a - Vector3.up * l);
		col = Color.blue;
		draw(a + Vector3.forward * l,a - Vector3.forward * l);

		return null;
	}

	public LineRenderer draw(Vector3 a,Vector3 b){

		lineList[Count] = Instantiate(Line3D);
		Array.Resize(ref lineList,lineList.Length+1);		
		lineList[Count].transform.parent = LineEmpty.transform;

		LineRenderer line = lineList[Count].GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.SetWidth(width,width);
		line.SetVertexCount(2);
		line.SetPosition(0,a);
		line.SetPosition(1,b);
		line.SetColors(col,col);

		Count++;

		return line;
	}

	public LineRenderer draw(GameObject a,float b){

		lineList[Count] = Instantiate(Line3D);
		Array.Resize(ref lineList,lineList.Length+1);		
		lineList[Count].transform.parent = LineEmpty.transform;
		
		LineRenderer line = lineList[Count].GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.SetWidth(width,width);
		line.SetVertexCount(2);
		line.SetPosition(0,a.transform.position);
		line.SetPosition(1,a.transform.position + Vector3.up*b);
		line.SetColors(col,col);

		return line;
	}

	public LineRenderer draw(Vector3 a,float b){

		lineList[Count] = Instantiate(Line3D);
		Array.Resize(ref lineList,lineList.Length+1);		
		lineList[Count].transform.parent = LineEmpty.transform;

		LineRenderer line = lineList[Count].GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.SetWidth(width,width);
		line.SetVertexCount(2);
		line.SetPosition(0,a);
		line.SetPosition(1,a + Vector3.up*b);
		line.SetColors(col,col);

		Count++;

		return line;
	}

	public LineRenderer draw(GameObject a,GameObject b,float c){
		lineList[Count] = Instantiate(Line3D);
		Array.Resize(ref lineList,lineList.Length+1);		
		lineList[Count].transform.parent = LineEmpty.transform;

		LineRenderer line = lineList[Count].GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.SetWidth(width,width);
		line.SetVertexCount(2);
		line.SetPosition(0,a.transform.position + Vector3.up*c);
		line.SetPosition(1,b.transform.position + Vector3.up*c);
		line.SetColors(col,col);

		Count++;

		return line;
	}

	public void setColor(Color c){
		col = c;
	}
	public void setWidth(float w){
		width = w;
	}
}