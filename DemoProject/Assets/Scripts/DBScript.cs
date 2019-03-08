using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;

public class DBScript : MonoBehaviour {
	private string dbPath;
	private List<string> options;
	private List<string> ids;
	public Dropdown rhythmDropdown;
	public Dropdown bpmDropdown;
	public InputField[] words;
	public Toggle rhythmToggle;
	public static bool rhythmicMode;

	void Start () {
		/*dbPath = "URI=file:" + Application.persistentDataPath + "/rhythmTap.db";
		CreateSchema();

		if (rhythmDropdown != null) {
			rhythmDropdown.ClearOptions ();
			GetRhythms ();
			rhythmDropdown.AddOptions (options);
		}*/
		//InsertRhythm("4n 8n 8n", "Hi Kris-ten");
		//InsertRhythm("4n 4n 4n", "Help me please");
		//InsertRhythm("4n", "Hi");
		//GetRhythms();
	}
		
	private string GenerateUniqeID (){
		return System.Guid.NewGuid().ToString();
	}

	public void StartGame() {
	//	string rhythmOpTxt = rhythmDropdown.options [rhythmDropdown.value].text;
	//	string[] opSplit = rhythmOpTxt.Replace ("\nText: ", "\n").Split('\n');

		int bpm;
		switch (bpmDropdown.value) {
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

		//MenuController.StartGame (opSplit [0], opSplit [1], bpm);
	}

	public void CreateSchema() {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'rhythms' ( " +
					"  'id' TEXT PRIMARY KEY, " +
					"  'rhythm' TEXT NOT NULL, " +
					"  'prompt' TEXT" +
					");";

				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema: " + result);
			}
		}
	}

	private void InsertRhythm(string rhythm, string prompt) {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "INSERT INTO rhythms (id, rhythm, prompt) " +
					"VALUES (@ID, @Rhythm, @Prompt);";

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "ID",
					Value = GenerateUniqeID()
				});

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Rhythm",
					Value = rhythm
				});

				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "Prompt",
					Value = prompt
				});

				var result = cmd.ExecuteNonQuery();
				Debug.Log("insert score: " + result);
			}
		}
	}

	public void GetRhythms() {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				options = new List<string> ();
				ids = new List<string> ();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM rhythms ORDER BY rhythm;";

				Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read()) {
					string id = reader.GetString(0);
					string rhythm = reader.GetString(1);
					string prompt = reader.GetString(2);
					string text = string.Format("{0}: {1} [#{2}]", id, rhythm, prompt);

					options.Add (rhythm + "\nText: " + prompt);
					ids.Add (id);
					Debug.Log(text);
				}
				Debug.Log("scores (end)");
			}
		}
	}

	public void DeleteRhythm() {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "DELETE FROM rhythms WHERE " +
					"id = @ID";

				int dpval = rhythmDropdown.value;
				if (ids.Count <= dpval)
					return;
				if (ids.Count - 1 == dpval)
					rhythmDropdown.value--;
				
				cmd.Parameters.Add(new SqliteParameter {
					ParameterName = "ID",
					Value = ids[dpval]
				});

				var result = cmd.ExecuteNonQuery();
				Debug.Log("delete score: " + result);
				Debug.Log("deleted option: " + dpval);

				rhythmDropdown.ClearOptions ();
				GetRhythms ();
				rhythmDropdown.AddOptions (options);
			}
		}
	}

	public void AddRhythm(){
		string[] phrase = new string[3];
		int i = 0;
		foreach(InputField s in words){
			phrase [i++] = s.text;
		}

		string p = string.Join (" ", phrase);

		InsertRhythm (MenuController.rhythm, p);

		SceneManager.LoadScene("RhythmSelection");
	}

	void Update(){
		if(rhythmToggle != null)
			rhythmicMode = rhythmToggle.isOn;
	}
}
