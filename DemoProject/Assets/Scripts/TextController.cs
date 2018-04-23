using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TextController : MonoBehaviour {

	public Text TNBText;
	public Text OnsetScoreText;
	public Text AvgScoreText;
	//public Text TendText;
	public Text NOHText;
	public Text DurScoreText;
	//public Text NOMText;
	// Use this for initialization
	void Start () {
		TNBText.text = DrumController.TNBText.ToString ();
		OnsetScoreText.text = DrumController.OnsetScoreText.ToString ("0.00");
		AvgScoreText.text = DrumController.AvgScoreText.ToString ("0.00");
		//TendText.text = DrumController.TendText.ToString ();
		NOHText.text = DrumController.NOHText.ToString ();
		DurScoreText.text = DrumController.DurScoreText.ToString ("0.00");
		//NOMText.text = DrumController.NOMText.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
