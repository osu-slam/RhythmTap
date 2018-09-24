using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;

public class MenuController : MonoBehaviour {
	public static float bpm = 55;
	public static bool impromptu;
	public static int gameNum;
	public static bool debug = false;

	public Text Username_field;
	public Dropdown phraseDropdown;
	public Dropdown dp0;
	//public Dropdown dp2;
	public Dropdown dp3;
	public Dropdown dp4;
	public Dropdown dp5;
	public static AudioClip audioClip;

	public static string rhythm = "";
	public static string phrase = "";

	private static string[] phrases = new string[] {
		"Help me please.",
		"Let’s go out.",
		"Get my drink.",
		"Put it there.",
		"How are you?",
		"Will you join?",
		"What’s for lunch?",
		"Can I see?",
		"I am good.",
		"My head hurts.",
		"Thanks so much.",
		"Good bye Joe.",
		"Hi there Joe.",
		"Love you lots."
	};

	// Use this for initialization
	void Start () {
		bpm = 30;
		phrase = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdatePhrase(){
		if(phraseDropdown.value > 0 && phraseDropdown.value - 1 < phrases.Length){
			phrase = phrases[phraseDropdown.value - 1];
		} else {
			phrase = "";
		}
	}

	public void StartGame0(){
		gameNum = 0;
		rhythm = "4n 4r";
		switch (dp0.value) {
		case 0:
			bpm = 55;
			break;
		case 1:
			bpm = 65;
			break;
		default:
			bpm = 75;
			break;
		}
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	public void StartGame1(){
		rhythm = "4n 4n 4r";
		gameNum = 1;
		switch (dp3.value) {
		case 0:
			bpm = 55;
			break;
		case 1:
			bpm = 65;
			break;
		default:
			bpm = 75;
			break;
		}
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
    public void StartGame2(){
		rhythm = "4n 4n 4n 4r";
		gameNum = 2;
		switch (dp4.value) {
		case 0:
			bpm = 55;
			break;
		case 1:
			bpm = 65;
			break;
		default:
			bpm = 75;
			break;
		}
        impromptu = false;
        SceneManager.LoadScene("MainScene");

    }

	public void StartGame3(){
		rhythm = "4n 4n 4n 4n";
		gameNum = 3;
		switch (dp5.value) {
		case 0:
			bpm = 55;
			break;
		case 1:
			bpm = 65;
			break;
		default:
			bpm = 75;
			break;
		}
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

	public void EnableDebug(){
		debug = true;
	}

	public void onValueChanged(){

	}
}
