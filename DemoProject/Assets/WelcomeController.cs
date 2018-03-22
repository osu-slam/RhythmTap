using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeController : MonoBehaviour {
	public static string name = "default";
	public Text Username_field;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setName(){
		WelcomeController.name = Username_field.text.ToString();
	}
}
