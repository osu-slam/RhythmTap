using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropDownEffect : MonoBehaviour {
	public Dropdown mydropdown;
	public int test = 0;
	// Use this for initialization
	public void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		
	}



	public void dropDownInput()
	{

		if (mydropdown.value == 0) {
			MenuController.bpm = 100;
			test = 0;

		} else if (mydropdown.value == 1) {
			MenuController.bpm = 120;
			test = 1;
		} else if(mydropdown.value == 2) {
			MenuController.bpm = 180;
			test = 2;
		}
	}
}
