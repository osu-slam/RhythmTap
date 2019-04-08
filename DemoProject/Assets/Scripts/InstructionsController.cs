using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour
{
	public GameObject[] phaseImgs;
	public Text practiceType;
	public Text countdownInstr;
	int numCycles = 0;
	bool nextButtonPressed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		numCycles = DrumController.numCycles;
		if (numCycles == 0) {
			practiceType.text = "tapping";
			countdownInstr.text = "There will be a red countdown. When it says \"Go!\", it's your turn to \ntap.";
			phaseImgs [0].SetActive (true);
			phaseImgs [1].SetActive (false);
			phaseImgs [2].SetActive (false);
		} else if (numCycles == 1) {
			practiceType.text = "tapping and speaking";
			countdownInstr.text = "There will be a red countdown. When it says \"Go!\", it's your turn to \ntap and speak.";
			phaseImgs [0].SetActive (false);
			phaseImgs [1].SetActive (true);
			phaseImgs [2].SetActive (false);
		} else {
			practiceType.text = "speaking";
			countdownInstr.text = "There will be a red countdown. When it says \"Go!\", it's your turn to \nspeak.";
			phaseImgs [0].SetActive (false);
			phaseImgs [1].SetActive (false);
			phaseImgs [2].SetActive (true);
		}

		if (!nextButtonPressed) {
			return;
		}
		nextButtonPressed = false;
    }

	public void StartSession(){
		SceneManager.LoadScene("MainScene");
	}
}
