using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndScreenController :MonoBehaviour {
	//public Sprite endScreen;
	GameObject endScreen;
	public EndScreenController(){
		endScreen = GameObject.Find ("endScreen");
	}

	public void Enable(){
		endScreen.SetActive(true);
	}

	public void Disable(){
		endScreen.SetActive (false);
	}
}
