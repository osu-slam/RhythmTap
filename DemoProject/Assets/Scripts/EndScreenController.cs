using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndScreenController :MonoBehaviour {
	//public Sprite endScreen;
	Image image;
	public EndScreenController(){
		image = GameObject.Find ("endScreen").GetComponent<Image> ();
	}

	public void Enable(){
		image.enabled = true;
	}

	public void Disable(){
		image.enabled = false;
	}
}
