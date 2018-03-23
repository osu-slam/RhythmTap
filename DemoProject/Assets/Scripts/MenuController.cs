using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public static float bpm = 85;
	public static bool impromptu;
	public static int gameNum;
	public Text Username_field;


	Dropdown dp;
	// Use this for initialization
	void Start () {
		bpm = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void StartGame0(){
		gameNum = 0;
		bpm = 85;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame1(){
		gameNum = 1;
		bpm = 85;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame2(){
		gameNum = 2;
		bpm = 85;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame3(){
		gameNum = 3;
		bpm = 85;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
    public void StartGame4(){
		gameNum = 4;
        bpm = 85;
        impromptu = false;
        SceneManager.LoadScene("MainScene");

    }

    public void QuitGame(){
		Application.Quit();
	}

	public void BackToWelcome(){
		SceneManager.LoadScene("Welcome Scene");
	}

	public void StartMenu(){

		SceneManager.LoadScene("Menu");
	}
	public void StartMenu1(){
		
		SceneManager.LoadScene("Menu");
	}


	public void onValueChanged(){

	}
}
