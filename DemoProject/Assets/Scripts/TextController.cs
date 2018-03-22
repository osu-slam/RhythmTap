using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TextController : MonoBehaviour {

	public Text TNBText;
	public Text ATOText;
	public Text TOVText;
	public Text TendText;
	public Text NOHText;
	public Text NODHText;
	public Text NOMText;
	// Use this for initialization
	void Start () {
		TNBText.text = DrumController.TNBText.ToString ();
		ATOText.text = DrumController.ATOText.ToString ();
		TOVText.text = DrumController.TOVText.ToString ();
		TendText.text = DrumController.TendText.ToString ();
		NOHText.text = DrumController.NOHText.ToString ();
		NODHText.text = DrumController.NODHText.ToString ();
		NOMText.text = DrumController.NOMText.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
