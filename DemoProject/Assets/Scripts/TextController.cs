using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TextController : MonoBehaviour {

	public Text TNBText;
	public Text OnsetScoreText;
	public Text AvgScoreText;
	public Text FAText;
	public Text NOHText;
	public Text DurScoreText;
	public Text NOMText;
	public Text LessText;
	public Text MoreText;
	// Use this for initialization
	void Start () {
		TNBText.text = DrumController.TNBText.ToString ();
		OnsetScoreText.text = DrumController.OnsetScoreText.ToString ("0.00");
		AvgScoreText.text = DrumController.OnsetScoreText.ToString ("0.00");
		FAText.text = DrumController.FAText.ToString ();
		NOHText.text = DrumController.NOHText.ToString ();
		NOMText.text = DrumController.NOMText.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
