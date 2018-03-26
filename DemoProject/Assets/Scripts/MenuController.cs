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
	public Dropdown dp1;
	public Dropdown dp2;
	public Dropdown dp3;
	public Dropdown dp4;
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
		switch (dp1.value) {
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
		audioClip = rhythm1audioClips [dp1.value];
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame2(){
		gameNum = 2;
		switch (dp2.value) {
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
		audioClip = rhythm2audioClips [dp2.value];
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame3(){
		gameNum = 3;
		switch (dp3.value) {
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
		audioClip = rhythm3audioClips [dp3.value];
		SceneManager.LoadScene("MainScene");
	}
    public void StartGame4(){
		gameNum = 4;
		switch (dp4.value) {
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
		audioClip = rhythm4audioClips [dp4.value];
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
