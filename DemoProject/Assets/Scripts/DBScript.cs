using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DBScript : MonoBehaviour {
	private string dbPath;

	void Start () {
		dbPath = "URI=file:" + Application.persistentDataPath + "/rhythmTap.db";
		CreateSchema();
		//InsertRhythm("4n 8n 8n", "Hi Kris-ten");
		//InsertRhythm("4n 4n 4n", "Help me please");
		//InsertRhythm("4n", "Hi");
		//GetRhythms();
	}

	private string GenerateUniqeID (){
		return System.Guid.NewGuid().ToString();
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

	public void InsertRhythm(string rhythm, string prompt) {
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

	public List<string> GetRhythms() {
		using (var conn = new SqliteConnection(dbPath)) {
			conn.Open();
			using (var cmd = conn.CreateCommand()) {
				List<string> data = new List<string> ();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM rhythms ORDER BY rhythm;";

				Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read()) {
					string id = reader.GetString(0);
					string rhythm = reader.GetString(1);
					string prompt = reader.GetString(2);
					string text = string.Format("{0}: {1} [#{2}]", id, rhythm, prompt);
					data.Add (rhythm + "," + prompt);
					Debug.Log(text);
				}
				Debug.Log("scores (end)");
				return data;
			}
		}
	}
}
