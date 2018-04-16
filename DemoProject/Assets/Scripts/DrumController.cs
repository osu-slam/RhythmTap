using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using Assets.Scripts.TextureStorage;

public class DrumController : MonoBehaviour {
	public float bpm = 0f; //default

	public AudioSource SingleStickSfx;
    public AudioSource game0Audio;

    //public Text scoreText;
    public Text countdownText;
	public Text promptText;
	bool analyzed = false;


	float variance;
	public List<float> list;
	public List<float> stdList;
	public List<float> halfNotes;
	private List<float> stdDuration;
	private List<float> duration;
	bool hasStarted = false;
	bool hasEnded = false;
	bool hasLaunched = false;
	bool hasPlayed = false;

	float startTime = 0.0f;
	float launchTime = 0.0f;


	int numberOfHits = 0;
	int numberOfMisses = 0;
	int numberOfMisHits = 0;
	static float lengthOfAudio;

	float totalPriorOff = 0;
	float totalLateOff = 0;
	float totalOff = 0;
	float countDownTime;

	float error = 0.1f;
	float beat = 0;
	int stdIndex = 0;
	float drumHighlightBreak = 0.05f;
	float currentNoteLen = 0.0f;

	public static int TNBText = 8;
	public static float ATOText = 0.0f;
	public static float TOVText = 0.0f;
	public static string TendText = "Early";
	public static int NOHText = 0;
	public static float NODHText = 0;
	public static int NOMText = 0;

	SpriteRenderer sr;
	int i = 0;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();

		if (MenuController.impromptu == false) {
			analyzed = false;
			bpm = (float)MenuController.bpm;
			//LogManager.Instance.LogSessionStart (bpm);

			list = new List<float> ();
			//Set total num of beats based on current bpm selected;
			lengthOfAudio = 60.0f;//in seconds
			beat = 60.0f/bpm;

			stdList = new List<float> ();
			duration = new List<float> ();
            /* Testing */
            RhythmLoader rhythmLoader = new RhythmLoader();
			rhythmLoader.LoadRhythm(RhythmDataStorage.GetRhythm());
			stdList = rhythmLoader.GetRhythmTimes ();
			halfNotes = rhythmLoader.GetHalfNoteDurations ((int)bpm);
			stdDuration = rhythmLoader.GetNoteDurations ((int)bpm);
			TNBText = stdList.Count;

			/* init */
			countdownText.text = "";
			hasLaunched = false;
			hasStarted = false;
			hasPlayed = false;
			numberOfMisHits = 0;
			numberOfMisses = 0;
			numberOfHits = 0;
			stdIndex = 0;
			variance = 0.0f;

			countDownTime = 0.0f;
			launchTime = Time.timeSinceLevelLoad;
			startTime = 0.0f;
		} else {
			countdownText.text = "";
			lengthOfAudio = 60.0f;
			bpm = (float)MenuController.bpm;
			//LogManager.Instance.LogSessionStart (bpm);
			launchTime = Time.timeSinceLevelLoad;
			hasLaunched = false;
		}
		StartAudio ();
	}

	void StartAudio(){
		/*switch (MenuController.gameNum) {
		case 0:
			game0Audio.Play ();
			break;
		case 1:
			game1Audio.Play ();
			break;
		case 2:
			game2Audio.Play ();
			break;
		case 3:
			game3Audio.Play ();
			break;
		default:
			game4Audio.Play ();
			break;
		}*/
		RhythmSoundStorage.GetAudio (game0Audio);
		game0Audio.Play ();
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
		if (Time.timeSinceLevelLoad - launchTime > 2*beat && !hasLaunched) {
			StartCountDown (); //hasLaunched = true
		}
		if (!hasPlayed && hasLaunched) {
			UpdateCountDownText (); //hasStarted = true

			if (Time.timeSinceLevelLoad - countDownTime > 8 * beat) {
				hasPlayed = true;
				StartPlayingSession (); //set startTime
			}
			if (Time.timeSinceLevelLoad - startTime > lengthOfAudio + beat * 8) {
				EndPlayingSession (); //hasEnded = true
			}
		} else {
			if (Time.timeSinceLevelLoad - launchTime > lengthOfAudio + beat * 9) {
				EndPlayingSession (); //hasEnded = true
			}
		}

		UpdateDrumHighlight ();

		if (hasStarted && !hasEnded) {
			if (Input.GetKeyDown (KeyCode.Space))
				UpdateKeyDown ();
			else if (Input.GetKeyUp (KeyCode.Space))
				UpdateKeyUp ();
			else if (Input.GetKeyUp (KeyCode.P))
				EndPlayingSession ();
		}


		/*if (!hasPlayed && hasLaunched) {
			UpdateCountDownText ();

			if (Time.timeSinceLevelLoad - countDownTime > 6 * beat) {
				hasPlayed = true;
				StartPlayingSession ();
			}

			if (hasStarted && (Input.GetKeyDown (KeyCode.Space) && (Time.timeSinceLevelLoad - startTime) < (lengthOfAudio + beat*8))) {
				UpdateKeyDown ();
			}
				
			if (hasPlayed && ((numberOfHits + numberOfMisses) < TNBText)) {
				if ((Time.timeSinceLevelLoad - startTime) > (stdList [stdIndex] + halfNotes[stdIndex])) {
					stdIndex++; //passed;
					numberOfMisses++;
					countdownText.text = "Miss";
				}
			}
				
			if (Time.timeSinceLevelLoad - startTime > lengthOfAudio + beat*8) {
				EndPlayingSession ();
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Space) && !hasLaunched) {
				hasLaunched = true;
				launchTime = Time.timeSinceLevelLoad;
				UpdateKeyDown ();
				//SingleStickSfx.Play ();
				//LogManager.Instance.Log ((Time.timeSinceLevelLoad - launchTime), stdIndex);
			} else if ((Time.timeSinceLevelLoad - launchTime < lengthOfAudio) && (Input.GetKeyDown (KeyCode.Space))) { 
				//play and log

				SingleStickSfx.Play ();
				LogManager.Instance.Log ((Time.timeSinceLevelLoad - launchTime), stdIndex);
			} else if (Time.timeSinceLevelLoad - launchTime > lengthOfAudio+beat*8) {
				PromptMenu (); 
			}
		}*/
	}

	//analizing timestamps after finishing the song
	void PerformanceAnalysis(){
		int stdListIndex = 0; //index for stdDuration and stdList
		int listIndex = 0; //index for duration and list
		float score = 0.0f;

		while (stdListIndex < stdList.Count && listIndex < list.Count) {
			float upper = stdList [stdListIndex] + error + 8 * beat;
			float lower = stdList [stdListIndex] - error + 8 * beat;
			if (list [listIndex] < upper && list [listIndex] > lower) {
				float subScore = duration [listIndex] / (stdDuration [stdListIndex] - 2*drumHighlightBreak);
				if (subScore >= 1)
					subScore = 1;
				score += subScore;

				numberOfHits++;
				stdListIndex++;
				listIndex++;
			} else if (list [listIndex] > upper) {
				stdListIndex++;
			} else {
				listIndex++;
			}
		}

		NOHText = numberOfHits;
		TOVText = (float)((int)(score * 100)/TNBText);
		//LogManager.Instance.Log ((Time.timeSinceLevelLoad - startTime), stdIndex);
		/*if ((Time.timeSinceLevelLoad - startTime) < stdList [stdIndex]) {
			totalPriorOff += Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]);
		} else {
			totalLateOff += Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]);
		}
		//set strings
		ATOText = (totalPriorOff + totalLateOff) / (float)numberOfHits;
		for (int i = 0; i < list.Count; i++) {
			variance += Mathf.Abs (list [i] - ATOText);
		}
		variance /= numberOfHits;
		NOMText = numberOfMisses;

		TOVText = variance;
		NODHText = numberOfMisHits;


		NOHText = numberOfHits;


		if (totalLateOff > totalPriorOff) {
			TendText = "Late";
		} else {
			TendText = "Early";
		}*/
		/*
		LogManager.Instance.Log("TotalNumberOfBeats", TNBText.ToString());
		LogManager.Instance.Log("NumberOfHits", NOHText.ToString());
		LogManager.Instance.Log("NumberOfMisses", NOMText.ToString());
		LogManager.Instance.Log("NumberOfMis-Hits", NODHText.ToString());
		LogManager.Instance.Log("AverageTimeOffsets", ATOText.ToString());
		LogManager.Instance.Log("TimeOffsetsVariance", TOVText.ToString());
		LogManager.Instance.Log("Tendency", TendText.ToString());*/

	}

	void StartCountDown(){
		countDownTime = Time.timeSinceLevelLoad;
		countdownText.text = "5";
		hasLaunched = true;
	}

	void UpdateDrumHighlight(){
		if (i < stdList.Count) {
			float time;
			time = stdList [i] + 8 * beat;
			if (!HighlightDrum (time, stdDuration[i]))
				i++;
		}
	}

	bool HighlightDrum(float time, float duration){
		//float upperBound = time + 0.25f;
		//float lowerBound = time + 0.05f;
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
			hasStarted = true;
		} else if (Time.timeSinceLevelLoad - countDownTime > 5 * beat) {
			countdownText.text = "Go!";
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
		//rhythm.Stop();
		//halfBeat = 0.50f; // TODO
		startTime = Time.timeSinceLevelLoad;
		//rhythm.Play();
	}

	void UpdateKeyDown(){
		//add timestamp to the list
		list.Add (Time.timeSinceLevelLoad);
		duration.Add (Time.timeSinceLevelLoad);

		//play sound clip
		SingleStickSfx.Play ();
		/*if (hasPlayed) {

			if ((Time.timeSinceLevelLoad - startTime) < (stdList [stdIndex] - halfBeat) || (stdIndex == (TNBText))) {
				numberOfMisHits++;
				countdownText.text = "Mis-Hit";
			} else if ((numberOfHits + numberOfMisses) < TNBText) {
				numberOfHits++;
				totalOff += Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]);
				LogManager.Instance.Log ((Time.timeSinceLevelLoad - startTime), stdIndex);
				if ((Time.timeSinceLevelLoad - startTime) < stdList [stdIndex]) {
					//earlier
					if (stdIndex < (TNBText - 1)) {
						totalPriorOff += Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]);
						list.Add (Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]));
						stdIndex++;
					}
					countdownText.text = "Hit";
				} else {
					//late
					if (stdIndex < (TNBText - 1)) {
						totalLateOff += Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]);
						list.Add (Mathf.Abs (Time.timeSinceLevelLoad - startTime - stdList [stdIndex]));
						stdIndex++;
						countdownText.text = "Hit";
					}
				}


			}
		} else {
			//deal with the first beat
			if (numberOfHits == 0) {
				numberOfHits++;
				LogManager.Instance.Log ((Time.timeSinceLevelLoad - startTime), stdIndex);
				totalPriorOff += Mathf.Abs (Time.timeSinceLevelLoad - countDownTime - stdList [stdIndex]);
				stdIndex++;
				countdownText.text = "Hit";
			}
		}*/
	}

	void UpdateKeyUp(){
		if(duration.Count > 0)
			duration[duration.Count - 1] = Time.timeSinceLevelLoad - duration [duration.Count - 1];
	}

	void EndPlayingSession(){
		if (Input.GetKey (KeyCode.Space)) {
			UpdateKeyUp ();
		}
		if (analyzed == false) {
			PerformanceAnalysis ();
			analyzed = true;
		}
		//PromptMenu ();
		//if (Input.GetKeyDown (KeyCode.P)) {
			SceneManager.LoadScene ("Analysis");
		//}
	}

	void PromptMenu(){
		hasEnded = true;
		promptText.text = "Press space to continue";
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("Menu");
		} 
	}
}