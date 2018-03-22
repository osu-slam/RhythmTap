using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public static float bpm;
	public static bool impromptu;
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
		bpm = 60;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame1(){
		bpm = 30;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame2(){
		bpm = 120;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame3(){
		bpm = 180;
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame5(){
		bpm = 0;
		impromptu = true;
		SceneManager.LoadScene("MainScene");
	
	}

    public void StartGame6()
    {
        bpm = 100;
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
