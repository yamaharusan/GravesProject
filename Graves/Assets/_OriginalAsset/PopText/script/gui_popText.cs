using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gui_popText : MonoBehaviour {
	public static List<gui_popText> list = new List<gui_popText> ();
	static Camera mainCamera;
	StdLib lib = new StdLib();

	[System.NonSerialized]public bool isHoming = false;

	[System.NonSerialized]public Text text = null;
	[System.NonSerialized]public RectTransform rect = null;
	Transform target_transform = null;
	Vector3 target_position = Vector3.zero;

	[System.NonSerialized]public Vector3 position = Vector3.zero;
	[System.NonSerialized]public Vector3 direction = Vector3.zero;
	Vector3 _direction = Vector3.zero;
	[System.NonSerialized]public float lifeSpan = 1f;

	int seed = 0;

	// Use this for initialization
	void Start () {
		if (!mainCamera) 
		{
			mainCamera = Camera.main;
		}
	}
	
	// Update is called once per frame
	void Update () {
		lifeSpan -= Time.deltaTime;
		if (lifeSpan > 0f) {
			//if(isHoming && (Time.frameCount + seed) % 5 == 0){
				if(target_transform != null){
					position = lib.move(position,(Vector3)RectTransformUtility.WorldToScreenPoint(mainCamera,target_transform.position),3f);
				}else if(target_position != Vector3.zero){
					position = lib.move(position,(Vector3)RectTransformUtility.WorldToScreenPoint(mainCamera,target_position),3f);
				}
			//}

			_direction += direction * Time.deltaTime;
			rect.position = position + _direction;

		} else {
			Destroy(gameObject);
		}
	}

	void OnDestroy(){
		list.Remove (this);
	}

	public void Init(){
		list.Add(this);
		text = GetComponent<Text>();
		rect = GetComponent<RectTransform> ();

		seed = Random.Range(0,60);

		if (!mainCamera) {
			mainCamera = Camera.main;
		}
	}

	public void setTarGet(Transform tar){
		target_transform = tar;
		position = (Vector3)RectTransformUtility.WorldToScreenPoint(mainCamera,target_transform.position);

		rect.position = position;

		isHoming = true;
	}

	public void setTarGet(Vector3 tar){
		target_position = tar;
		position = (Vector3)RectTransformUtility.WorldToScreenPoint(mainCamera,target_position);

		rect.position = position;

		isHoming = true;
	}
}
