using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using Assets.Scripts.TextureStorage;

public class DrumController : MonoBehaviour {
	private const float NaN = 0.0f / 0;
	public float bpm = 0f; //default
	private static int BeatsPerMeasure = 4;

	public AudioSource Clave;
	public AudioSource WoodBlock;
	public AudioSource SingleStickSfx;
    public AudioSource game0Audio;

    //public Text scoreText;
    public Text countdownText;
	public Text promptText;
	public Text phraseText;
	bool analyzed = false;


	//float variance;
	public List<float> list;
	public List<float> stdList;
	public List<float> halfNotes;
	private List<float> stdDuration;
	bool hasStarted = false;
	bool hasEnded = false;
	bool hasLaunched = false;
	bool hasPlayed = false;

	float startTime = 0.0f;
	float launchTime = 0.0f;

	float nextMetronomeBeat;

	int numberOfHits = 0;
	static float lengthOfAudio;

	float countDownTime;

	float error = 0.1f;
	float beat = 0;
	float drumHighlightBreak = 0.05f;

	public static int TNBText = 8;
	public static float OnsetScoreText = 0.0f;
	public static float AvgScoreText = 0.0f;
	public static int FAText = 0;
	public static int NOHText = 0;
	public static float DurScoreText = 0;
	public static int NOMText = 0;
	public static int LessText = 0;
	public static int MoreText = 0;

	SpriteRenderer sr;
	private static EndScreenController endScreenController;
	private float endScreenTime;
	int i = 0;
	int j = 0;
	int totalBeats;

	private string[] phraseTokens;
	private int numTokens = 0;

	// Use this for initialization
	void Start () {
		endScreenController = new EndScreenController ();
		endScreenController.Disable ();
		endScreenTime = 0.0f;
		sr = GetComponent<SpriteRenderer> ();
		bpm = (float)MenuController.bpm;
		phraseTokens = MenuController.phrase.Split (' ');
		if (phraseTokens.Length > 0 && phraseTokens[0].Length > 0) {
			switch (MenuController.gameNum) {
			case 0:
				numTokens = 1;
				break;
			case 1:
				numTokens = 2;
				break;
			case 2:
				numTokens = 3;
				break;
			default:
				numTokens = 0;
				break;
			}
		}

		if(MenuController.debug == false)
			LogManager.Instance.LogSessionStart (bpm, MenuController.gameNum);

		if (MenuController.impromptu == false) {
			analyzed = false;

			//LogManager.Instance.LogSessionStart (bpm);

			list = new List<float> ();
			//Set total num of beats based on current bpm selected;
			lengthOfAudio = 60.0f;//in seconds
			beat = 60.0f/bpm;
			nextMetronomeBeat = (float)(AudioSettings.dspTime + beat);
			totalBeats = (int)(bpm / 4) * 4 + 8;

			if (MenuController.gameNum == 1)
				BeatsPerMeasure = 3;
			else
				BeatsPerMeasure = 4;

			stdList = new List<float> ();

            /* Testing */
            RhythmLoader rhythmLoader = new RhythmLoader();
			rhythmLoader.LoadRhythm(MenuController.rhythm, bpm, lengthOfAudio);
			stdList = rhythmLoader.GetRhythmTimes ();
			stdDuration = rhythmLoader.GetNoteDurations ((int)bpm);

			/* init */
			countdownText.text = "";
			hasLaunched = false;
			hasStarted = false;
			hasPlayed = false;
			numberOfHits = 0;

			OnsetScoreText = 0.0f;
			AvgScoreText = 0.0f;
			FAText = 0;
			NOHText = 0;
			DurScoreText = 0;
			NOMText = 0;
			MoreText = 0;
			LessText = 0;

			countDownTime = 0.0f;
			launchTime = Time.timeSinceLevelLoad;
			startTime = 0.0f;
		} else {
			countdownText.text = "";
			lengthOfAudio = 60.0f;
			//LogManager.Instance.LogSessionStart (bpm);
			launchTime = Time.timeSinceLevelLoad;
			hasLaunched = false;
		}
		//StartAudio ();
		Clave.Play();
		totalBeats--;
	}

	void StartAudio(){
		RhythmSoundStorage.GetAudio (game0Audio);
		game0Audio.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			SceneManager.LoadScene ("Menu");
		}

		if (MenuController.impromptu == false) {

			if (AudioSettings.dspTime >= nextMetronomeBeat && totalBeats-- > 0)
			{
				Clave.Play();
				nextMetronomeBeat += beat;
			}

			UpdateRegularPlayMode ();
		}
	}

	void UpdateRegularPlayMode(){
		int numIntroBeats = BeatsPerMeasure * 2;
		int numCountdownBeats = 6;
		float timeBeforeCountdown = (numIntroBeats - numCountdownBeats) * beat;
		float introLen = numIntroBeats * beat;

		if (Time.timeSinceLevelLoad - launchTime > timeBeforeCountdown && !hasLaunched) {
			StartCountDown (); //hasLaunched = true
		}

		if (!hasEnded) {
			if (!hasPlayed && hasLaunched) {
				UpdateCountDownText (); //hasStarted = true

				if (Time.timeSinceLevelLoad - countDownTime > introLen) {
					hasPlayed = true;
					StartPlayingSession (); //set startTime
				}
				if (Time.timeSinceLevelLoad - startTime > lengthOfAudio + introLen) {
					EndPlayingSession (introLen); //hasEnded = true
				}
			} else {
				if (Time.timeSinceLevelLoad - launchTime > lengthOfAudio + introLen) {
					EndPlayingSession (introLen); //hasEnded = true
				}
			}
		}
		UpdateDrumHighlight (introLen);
		UpdateDrumPrompt ();

		if (hasStarted && !hasEnded) {
			if (Input.GetKeyDown (KeyCode.Space))
				UpdateKeyDown ();
			else if (Input.GetKeyUp (KeyCode.P))
				EndPlayingSession (introLen);

			if (Input.touchCount > 0) {
				switch(Input.GetTouch(0).phase){
				case TouchPhase.Began:
					UpdateKeyDown ();
					break;
				}
			}
		}

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
	void PerformanceAnalysis(float introLen){
		TNBText = i;
		int stdListIndex = 0; //index for stdList
		int listIndex = 0; //index for list

		while (stdListIndex < TNBText && listIndex < list.Count) {
			float upper = stdList [stdListIndex] + error + introLen;
			float lower = stdList [stdListIndex] - error + introLen;
			if (list [listIndex] < upper && list [listIndex] > lower) {
				if (MenuController.debug == false) {
					LogManager.Instance.Log (stdList [stdListIndex], 
						list [listIndex] - introLen, 
						//stdDuration [stdListIndex], 
						//duration [listIndex], 
						stdListIndex);
				}

				//update onset accuracy score
				numberOfHits++;
				//increment
				stdListIndex++;
				listIndex++;
			} else if (list [listIndex] > upper) {
				//miss
				if(MenuController.debug == false)
					LogManager.Instance.Log (stdList [stdListIndex], NaN, /*stdDuration [stdListIndex], 0,*/ stdListIndex);
				stdListIndex++;
			} else {
				if(MenuController.debug == false)
					LogManager.Instance.Log (NaN, list [listIndex] - introLen, /*NaN, duration [listIndex],*/ -1);
				listIndex++;
				FAText++;
			}
		}

		//log remaining data
		while (stdListIndex < TNBText) {
			if(MenuController.debug == false)
				LogManager.Instance.Log (stdList [stdListIndex], NaN, /*stdDuration [stdListIndex], 0,*/ stdListIndex);
			stdListIndex++;
		}

		while (listIndex < list.Count) {
			if(MenuController.debug == false)
				LogManager.Instance.Log (NaN, list [listIndex] - introLen,/* NaN, duration [listIndex],*/ -1);
			listIndex++;
			FAText++;
		}

		NOHText = numberOfHits;
		NOMText = TNBText - NOHText;
		OnsetScoreText = (float)(NOHText * 100) / TNBText;
		//AvgScoreText = ((OnsetScoreText + DurScoreText) / 2.0f) - FAText;

		if (MenuController.debug == false) {
			//LogManager.Instance.Log ("AverageScore", AvgScoreText.ToString ());

			LogManager.Instance.Log ("OnsetScore", OnsetScoreText.ToString ());
			LogManager.Instance.Log ("NumberOfHits", NOHText.ToString ());
			LogManager.Instance.Log ("NumberOfMisses", NOMText.ToString ());
			LogManager.Instance.Log ("NumberOfFalseAlarms", FAText.ToString ());
			LogManager.Instance.Log ("TotalNumberOfBeats", TNBText.ToString ());

			//LogManager.Instance.Log ("DurationScore", DurScoreText.ToString ());
			//LogManager.Instance.Log ("> ExpDuration", MoreText.ToString ());
			//LogManager.Instance.Log ("<= ExpDuration", LessText.ToString ());
		}
	}

	void StartCountDown(){
		countDownTime = Time.timeSinceLevelLoad;
		countdownText.text = "5";
		hasLaunched = true;
	}

	void UpdateDrumHighlight(float introLen){
		if (i < stdList.Count) {
			float time;
			time = stdList [i] + introLen;
			if (!HighlightDrum (time, stdDuration[i], introLen))
				i++;
		}
	}

	void UpdateDrumPrompt(){
		if (j < stdList.Count && Time.timeSinceLevelLoad - launchTime + .01f > stdList[j]) {
			WoodBlock.Play ();
			if (numTokens > 0)
				phraseText.text = phraseTokens [(j) % numTokens];
			j++;
		}
	}

	bool HighlightDrum(float time, float duration, float offset){
		float upperBound = time + duration - drumHighlightBreak;
		float lowerBound = time + 0.05f;
		if (Time.timeSinceLevelLoad > lowerBound && Time.timeSinceLevelLoad < upperBound) {
			sr.color = Color.gray;
		} else {
			sr.color = Color.white;
		}
		
		if (Time.timeSinceLevelLoad >= upperBound)
			return false;
		else
			return true;
	}

	void UpdateCountDownText(){
		if (Time.timeSinceLevelLoad - countDownTime > 6 * beat) {
			countdownText.text = "";
		} else if (Time.timeSinceLevelLoad - countDownTime > 5 * beat) {
			countdownText.text = "Go!";
			hasStarted = true;
		} else if (Time.timeSinceLevelLoad - countDownTime > 4 * beat) {
			countdownText.text = "1";
		} else if (Time.timeSinceLevelLoad - countDownTime > 3 * beat) {
			countdownText.text = "2";
		} else if (Time.timeSinceLevelLoad - countDownTime > 2 * beat) {
			countdownText.text = "3";
		} else if (Time.timeSinceLevelLoad - countDownTime > 1 * beat) {
			countdownText.text = "4";
		}
	}

	void StartPlayingSession(){
		startTime = Time.timeSinceLevelLoad;
	}

	void UpdateKeyDown(){
		//add timestamp to the list
		list.Add (Time.timeSinceLevelLoad - launchTime);

		//play sound clip
		SingleStickSfx.Play ();
	}

	void EndPlayingSession(float introLen){
		hasEnded = true;

		if (MenuController.gameNum != 3) {
			endScreenController.Enable ();
			endScreenTime = Time.timeSinceLevelLoad;

			if (analyzed == false) {
				PerformanceAnalysis (introLen);
				analyzed = true;
			}
		} else {
			SceneManager.LoadScene ("Menu");
		}
	}

	void LoadAnalysis(){
		SceneManager.LoadScene ("Analysis");
	}

	void PromptMenu(){
		hasEnded = true;
		promptText.text = "Press space to continue";
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("Menu");
		} 
	}
}