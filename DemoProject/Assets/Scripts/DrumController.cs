using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using Assets.Scripts.TextureStorage;

public class DrumController : MonoBehaviour {
	private const float NaN = 0.0f / 0;
	public float bpm = 0f; //default

	public AudioSource Clave;
	public AudioSource WoodBlock;
	public AudioSource[] voices_60bpm;
	AudioSource voice;

    //public Text scoreText;
    public Text countdownText;
	public Text promptText;
	public Text pressToContinueText;
	public Text[] phraseSections;
	public Image[] phraseBackgrounds;
	public GameObject microphone;
	public GameObject drum;
	public GameObject nextButton;
	//bool analyzed = false;

	public List<float> keyDownList;
	public List<float> stdList;
	public List<float> tickList;
	public List<float> halfNotes;

	bool hasEnded = false;

	float launchTime = 0.0f;
	float nextMetronomeBeat;
	float dspTime;
	static float lengthOfAudio;
	float error = 0.1f;
	float beat = 0;

	public static int TNBText = 8;
	public static float OnsetScoreText = 0.0f;
	public static int FAText = 0;
	public static int NOHText = 0;
	public static int NOMText = 0;

	private static EndScreenController endScreenController;
	private float endScreenTime;

	int stdListCounter = 0;
	int tickListCounter = 0;

	int numCycles = 0;
	static int MAX_CYCLES = 3;
	float offset = 0f;
	int audioPlayed = 0;
	bool micActive = false;
	bool nextButtonPressed = false;
	AudioClip[] audioClip = new AudioClip[MAX_CYCLES];

	float waitTimeStart = -1f;
	float waitTimeEnd = 0.0f;

	void Start () {
		/* Initialize variables */
		waitTimeStart = -1f;
		waitTimeEnd = 0.0f;
		// Scoring variables
		OnsetScoreText = 0.0f;
		FAText = 0;											// Number of false alarms
		NOHText = 0;										// Number of hits
		NOMText = 0;										// Number of misses

		// Time-related variables
		bpm = (float)MenuController.bpm;
		lengthOfAudio = 60.0f;								// In seconds
		beat = 60.0f/bpm;									// Duration of each beat
		nextMetronomeBeat = (float)(launchTime + beat);
		launchTime = Time.timeSinceLevelLoad; 				// Time the game starts

		// Other variables
		countdownText.text = "";
		keyDownList = new List<float> ();
	

		/* Load but hide end screen */
		endScreenController = new EndScreenController ();
		endScreenController.Disable ();
		endScreenTime = 0.0f;


		/* Avoid unnecessary logs when debugging*/
		if(MenuController.debug == false)
			LogManager.Instance.LogSessionStart (bpm, MenuController.gameNum);


        /* Load rhythms if in rhythmic */
		if (DBScript.rhythmicMode) {
			RhythmLoader rhythmLoader = new RhythmLoader ();
			rhythmLoader.LoadRhythm (MenuController.rhythm, bpm, lengthOfAudio);
			stdList = rhythmLoader.GetRhythmTimes ();
			tickList = rhythmLoader.GetTickTimes ();
		} else {
			stdList = new List<float> ();
			tickList = new List<float> ();
		}

		/* Populate phrase prompt */
		/* NOT IN USE */
		/*string[] phrase = MenuController.phrase;
		for (int i = 0; i < phrase.Length; i++) {
			if (phrase [i] == null || phrase [i].Equals (""))
				phraseBackgrounds [i].enabled = false;
			else
				phraseSections [i].text = phrase [i];
		}*/


		/* Shuffle voice prompts for randomization */
		System.Random rnd = new System.Random();
		voices_60bpm = voices_60bpm.OrderBy(x => rnd.Next()).ToArray();  
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			SceneManager.LoadScene ("Menu");
		}

		if (MenuController.impromptu == false) {
			UpdateRegularPlayMode ();
		}
	}

	void UpdateRegularPlayMode(){
		if (numCycles < MAX_CYCLES) {
			if (Time.timeSinceLevelLoad - launchTime - offset > beat * 12) {
				nextButton.SetActive (true);
				if (!nextButtonPressed) {
					return;
				}
				nextButtonPressed = false;

				//waitTimeEnd = Time.timeSinceLevelLoad;

				launchTime = Time.timeSinceLevelLoad;
				stdListCounter = 0;
				tickListCounter = 0;
				microphone.SetActive (false);
				drum.SetActive (false);

				numCycles++;
				offset = 0;
				//offset = (numCycles * beat * 12) + (waitTimeEnd - waitTimeStart);
				 
				audioPlayed = 0;

				return;
			} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 6) {
				countdownText.text = "";

				// Display icons
				if (numCycles == 0) { // Tapping
					Vector3 drumPos = drum.transform.position;
					drum.transform.position = new Vector3(drumPos.x, drumPos.y, 0);
					drum.SetActive (true);
				} else if (numCycles == 1) { // Tapping and speaking
					Vector3 drumPos = drum.transform.position;
                    microphone.transform.localScale = new Vector3(7f, 7f, 1f);
					microphone.transform.localPosition = new Vector3(140f, 80f, 0);
					drum.transform.position = new Vector3(drumPos.x, drumPos.y, 0);
					drum.SetActive (true);
					microphone.SetActive (true);
				} else { // Speaking
                    microphone.transform.localScale = new Vector3(20f, 20f, 1f);
					microphone.transform.position = drum.transform.position;
					microphone.SetActive (true);
				}
				//microphone.SetActive (true);
				//if(DBScript.rhythmicMode) drum.SetActive (true);
			} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 2) {
				UpdateCountDownText ();
				if (Time.timeSinceLevelLoad - launchTime - offset > beat * 2 && audioPlayed == 1) {
					voice = voices_60bpm [numCycles];
					voice.Play ();
					audioPlayed = 2;
				} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 4 && audioPlayed == 2) {
					voice = voices_60bpm [numCycles];
					voice.Play ();
					audioPlayed = 3;
				}
			} else {
				if (micActive) {
					//Microphone.End (Microphone.devices[0]);
					micActive = false;
				}

				if (audioPlayed == 0) {
					voice = voices_60bpm [numCycles];
					voice.Play ();
					audioPlayed = 1;
				}
				nextButton.SetActive (false);
			}
		} else {
			EndPlayingSession ();
		}

		if(DBScript.rhythmicMode) UpdateDrumPrompt ();

		if (hasEnded){
			if (endScreenTime > 0.0f) {
				float temp = Time.timeSinceLevelLoad;
				float diff = temp - endScreenTime;
				if (diff > 1.0f) {
					LoadAnalysis ();
				}
			}
		}
	}

	//analizing timestamps after finishing the song
	void PerformanceAnalysis(){
		TNBText = 0; //TODO: Temporarily set to zero
		int stdListIndex = 0; //index for stdList
		int listIndex = 0; //index for list

		while (stdListIndex < TNBText && listIndex < keyDownList.Count) {
			float upper = stdList [stdListIndex] + error;
			float lower = stdList [stdListIndex] - error;
			if (keyDownList [listIndex] < upper && keyDownList [listIndex] > lower) {
				if (MenuController.debug == false) {
					LogManager.Instance.Log (stdList [stdListIndex], keyDownList [listIndex], stdListIndex);
				}
					
				NOHText++;
				stdListIndex++;
				listIndex++;
			} else if (keyDownList [listIndex] > upper) {
				if(MenuController.debug == false)
					LogManager.Instance.Log (stdList [stdListIndex], NaN, stdListIndex);
				stdListIndex++;
			} else {
				if(MenuController.debug == false)
					LogManager.Instance.Log (NaN, keyDownList [listIndex], -1);
				listIndex++;
				FAText++;
			}
		}

		//log remaining data
		while (stdListIndex < TNBText) {
			if(MenuController.debug == false)
				LogManager.Instance.Log (stdList [stdListIndex], NaN, stdListIndex);
			stdListIndex++;
		}

		while (listIndex < keyDownList.Count) {
			if(MenuController.debug == false)
				LogManager.Instance.Log (NaN, keyDownList [listIndex], -1);
			listIndex++;
			FAText++;
		}

		NOMText = TNBText - NOHText;
		OnsetScoreText = (float)(NOHText * 100) / TNBText;

		if (MenuController.debug == false) {
			LogManager.Instance.Log ("OnsetScore", OnsetScoreText.ToString ());
			LogManager.Instance.Log ("NumberOfHits", NOHText.ToString ());
			LogManager.Instance.Log ("NumberOfMisses", NOMText.ToString ());
			LogManager.Instance.Log ("NumberOfFalseAlarms", FAText.ToString ());
			LogManager.Instance.Log ("TotalNumberOfBeats", TNBText.ToString ());
		}
	}

	void UpdateDrumPrompt(){
		if (stdListCounter < stdList.Count && Time.timeSinceLevelLoad >= launchTime + stdList [stdListCounter]) {
			WoodBlock.Play ();
			stdListCounter++;
		}
		if (tickListCounter < tickList.Count && Time.timeSinceLevelLoad >= launchTime + tickList [tickListCounter]) {
			Clave.Play ();
			tickListCounter++;
		}
	}

	void UpdateCountDownText(){
		if (Time.timeSinceLevelLoad - launchTime - offset > beat * 5) {
			countdownText.text = "Go!";
			if (!micActive) {
				//audioClip[numCycles] = Microphone.Start (Microphone.devices[0], false, 6, 44100);
				micActive = true;
			}
		} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 4) {
			countdownText.text = "1";
		} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 3) {
			countdownText.text = "2";
		} else if (Time.timeSinceLevelLoad - launchTime - offset > beat * 2) {
			countdownText.text = "3";
		}
	}

	void UpdateKeyDown(){
		//add timestamp to the list
		keyDownList.Add (Time.timeSinceLevelLoad - launchTime);
	}

	void EndPlayingSession(){
		hasEnded = true;

		endScreenController.Enable ();
		endScreenTime = Time.timeSinceLevelLoad;

		for (int i = 0; i < MAX_CYCLES; i++) {
			string filename = WelcomeController.name + i + "_" +
				DateTime.Now.Month.ToString() + "_" + 
				DateTime.Now.Day.ToString() + "_" + 
				DateTime.Now.Hour.ToString() + "_" + 
				DateTime.Now.Minute.ToString();
			//SavWav.Save (filename, audioClip[i]);
		}
		/**if (analyzed == false) {
			PerformanceAnalysis ();
			analyzed = true;
		}*/
		if (!DBScript.rhythmicMode) {
			// Hide voice scores from analysis
			SceneManager.LoadScene ("Analysis2");
		} else {
			SceneManager.LoadScene ("Analysis");
		}

	}

	void LoadAnalysis(){
		SceneManager.LoadScene ("Analysis");
	}

	public void NextButtonPressed(){
		nextButtonPressed = true;
	}
}