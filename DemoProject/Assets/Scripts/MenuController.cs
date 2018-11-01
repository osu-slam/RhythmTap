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
	public static AudioClip audioClip;
	public InputField inputField;
	public Dropdown dp;
	public Sprite quarterNote;
	public Sprite eighthNotes;
	public Sprite emptySlot;
	public Image[] notes;
	public InputField[] words;

	public static string rhythm = "";
	public static string phrase = "";
	private int noteCount = 0;

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
		rhythm = "";
		noteCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		inputField.text = rhythm;
	}

	public void AddQuarterNote(){
		if (noteCount < 3) {
			if (noteCount > 0)
				rhythm += " ";
			rhythm += "4n";
			notes [noteCount].sprite = quarterNote;
			noteCount++;
		}
	}

	public void AddEighthNotes(){
		if (noteCount < 3) {
			if (noteCount > 0)
				rhythm += " ";
			rhythm += "8n 8n";
			notes [noteCount].sprite = eighthNotes;
			noteCount++;
		}
	}

	public void ClearEntries(){
		phrase = "";
		rhythm = "";
		noteCount = 0;
		foreach (Image img in notes){
			img.sprite = emptySlot;
		}
		foreach (InputField input in words){
			input.text = "";
		}

	}

	public void StartGame(){
		if (noteCount == 0)
			return;
		
		for (int i = 0; i < notes.Length; i++){
			phrase += words[i].text + " ";
			if(notes[i].sprite.Equals(eighthNotes))
				phrase += words[i].text + " ";
		}
		phrase = phrase.Trim ();

		switch (dp.value) {
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
		gameNum = noteCount == 2 ? 1 : 0;
		rhythm += " 4r";
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}
	/*
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
*/
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
