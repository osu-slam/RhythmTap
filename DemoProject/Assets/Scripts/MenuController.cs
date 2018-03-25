using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public static float bpm = 85;
	public static bool impromptu;
	public static int gameNum;

	public AudioClip[] rhythm0audioClips;
	public AudioClip[] rhythm1audioClips;
	public AudioClip[] rhythm2audioClips;
	public AudioClip[] rhythm3audioClips;
	public AudioClip[] rhythm4audioClips;

	public Text Username_field;
	public Dropdown dp0;
	public static AudioClip audioClip;

	// Use this for initialization
	void Start () {
		bpm = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void StartGame0(){
		gameNum = 0;

		switch (dp0.value) {
		case 0:
			bpm = 65;
			break;
		case 1:
			bpm = 75;
			break;
		default:
			bpm = 85;
			break;
		}
		impromptu = false;
		audioClip = rhythm0audioClips [dp0.value];
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame1(){
		gameNum = 1;
		bpm = 85;
		impromptu = false;
		audioClip = rhythm1audioClips [0];
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame2(){
		gameNum = 2;
		bpm = 85;
		impromptu = false;
		audioClip = rhythm2audioClips [0];
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame3(){
		gameNum = 3;
		bpm = 85;
		impromptu = false;
		audioClip = rhythm3audioClips [0];
		SceneManager.LoadScene("MainScene");
	}
    public void StartGame4(){
		gameNum = 4;
        bpm = 85;
        impromptu = false;
		audioClip = rhythm4audioClips [0];
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
