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
			countdownInstr.text = "You will also see a red countdown at the center of the screen. Once countdown reaches \"Go!\", it is your turn to tap on the screen.";
			phaseImgs [0].SetActive (true);
			phaseImgs [1].SetActive (false);
			phaseImgs [2].SetActive (false);
		} else if (numCycles == 1) {
			practiceType.text = "tapping and speaking";
			countdownInstr.text = "You will also see a red countdown at the center of the screen. Once countdown reaches \"Go!\", it is your turn to tap on the screen while speaking the phrase.";
			phaseImgs [0].SetActive (false);
			phaseImgs [1].SetActive (true);
			phaseImgs [2].SetActive (false);
		} else {
			practiceType.text = "speaking";
			countdownInstr.text = "You will also see a red countdown at the center of the screen. Once countdown reaches \"Go!\", it is your turn to speak the phrase.";
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
