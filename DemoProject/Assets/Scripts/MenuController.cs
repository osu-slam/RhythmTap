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
	public static bool debug = true;

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
	public static string[] phrase;
	public static int[] displayOrder;
	public static int displayOrderLen;
	private int noteCount = 0;


	// Use this for initialization
	void Start () {
		bpm = 30;
		phrase = new string[3];
		displayOrder = new int[6];
		displayOrderLen = 0;
		rhythm = "";
		noteCount = 0;
	}

	public void StartGame(){
		switch (dp.value) {
		case 0:
			bpm = 60;
			break;
		case 1:
			bpm = 65;
			break;
		default:
			bpm = 75;
			break;
		}

		rhythm += "8n 8n 4n";
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}

	/* Called by DBScript NOT IN USE*/
	/*
	public static void StartGame(string r, string p, int b){
		string[] rSplit = r.Replace("8n 8n", "8n8n").Split(' ');
		string[] pSplit = p.Split(' ');

		for (int i = 0; i < rSplit.Length; i++){
			phrase [i] = pSplit [i];
			displayOrder [displayOrderLen++] = i;

			if (rSplit[i].Equals("8n8n")) {
				displayOrder [displayOrderLen++] = i;
			}
		}
		//gameNum = rSplit.Length == 2 ? 1 : 0;
		gameNum = rSplit.Length-1;
		bpm = b;
		rhythm = r + " 4r";
		impromptu = false;
		SceneManager.LoadScene("MainScene");
	}*/

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
		
		SceneManager.LoadScene("RhythmSelection");
	}
	public void AddRhythmsScene(){
		SceneManager.LoadScene("Menu");
	}
	public void EnableDebug(){
		debug = true;
	}

	public void onValueChanged(){

	}
}
