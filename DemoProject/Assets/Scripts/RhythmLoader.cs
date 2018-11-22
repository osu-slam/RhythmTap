using Assets.Scripts.TextureStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts
{
    class RhythmLoader
    {
        List<MusicElement> musicElements;
		List<float> times;
		List<float> tickTimes;
		List<float> restDurations;
		float msRhythmDuration;
		float msTimeLim;
		float bpm;
		float qNoteDur;
		int numCyc;

        public RhythmLoader()
        {
            musicElements = new List<MusicElement>();
			times = new List<float> ();
			tickTimes = new List<float> ();
			restDurations = new List<float> ();
			msRhythmDuration = 0.0f;
			msTimeLim = 0.0f;
			bpm = 0.0f;
			qNoteDur = 0.0f;
			numCyc = 0;
        }

		public void LoadRhythm(String input, float bpm, float timeLimit)
        {
			this.bpm = bpm;
			msTimeLim = timeLimit;
			Parse (input);
        }

		private void Parse(String input){
			//if(DBScript.arrhythmicMode) input += " 8r";
			String[] elements = input.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for(int i = 0; i < elements.Length; i++)
			{
				switch (elements[i])
				{
				case "4n":
					musicElements.Add(new MusicElement("Quarter_Note"));
					break;
				case "8n":
					musicElements.Add(new MusicElement("Eighth_Note"));
					break;
				case "8tn":
					musicElements.Add(new MusicElement("Eighth_Note_T"));
					break;
				case "4r":
					musicElements.Add(new MusicElement("Quarter_Rest"));
					break;
				case "8r":
					musicElements.Add(new MusicElement("Eighth_Rest"));
					break;
				default:
					return;
				}

				msRhythmDuration += musicElements.Last ().GetNoteDuration ((int)bpm);
			}

			numCyc = (int) (msTimeLim / msRhythmDuration) + 4;
			qNoteDur = new MusicElement("Quarter_Note").GetNoteDuration((int)bpm);
			for (int i = 0; i < numCyc; i++) {
				if (DBScript.arrhythmicMode) {
					restDurations.Add (UnityEngine.Random.Range (0.6f * qNoteDur, 1.4f * qNoteDur));
				} else {
					restDurations.Add (qNoteDur);
				}
			}
		}
			

		public List<float> GetTickTimes(){
			float time = 0.0f;
			//int numCycles = (int) (msTimeLim / msRhythmDuration) + 4;
			for (int i = 0; i < numCyc; i++) {
				for (int j = 0; j <= MenuController.gameNum + 1; j++) {
					tickTimes.Add (time);

					/*float qNoteDur = new MusicElement("Quarter_Note").GetNoteDuration((int)bpm);
					if (DBScript.arrhythmicMode && j == MenuController.gameNum + 1) {
						//time += qNoteDur + new MusicElement("Eighth_Note").GetNoteDuration((int)bpm);
						time += UnityEngine.Random.Range (0.6f * qNoteDur, 1.4f * qNoteDur);
					} else {
						time += qNoteDur;
					}*/

					if (j == MenuController.gameNum + 1) {
						time += restDurations [i];
					} else {
						time += qNoteDur;
					}
				}
			}
			return tickTimes;
		}

		public List<float> GetRhythmTimes(){
			float time = 0.0f;
			//int numCycles = (int) (msTimeLim / msRhythmDuration) + 8;
			for (int i = 0; i < numCyc; i++) {
				foreach (MusicElement me in musicElements) {
					if (!me.IsRest ())
						times.Add (time);
					time += me.GetNoteDuration ((int)bpm);
				}
			}
			return times;
		}

		public List<float> GetHalfNoteDurations(int bpm){
			List<float> halfNoteDuration = new List<float> ();
			foreach (MusicElement me in musicElements) {
				halfNoteDuration.Add(me.GetNoteDuration (bpm));
			}
			return halfNoteDuration;
		}

		public List<float> GetNoteDurations(int bpm){
			List<float> durations = new List<float> ();

			int numCycles = (int) (msTimeLim / msRhythmDuration) + 8;
			for (int i = 0; i < numCycles; i++)
			{
				foreach (MusicElement me in musicElements) {
					if(!me.IsRest())
						durations.Add(me.GetNoteDuration(bpm));
				}
			}

			return durations;
		}
    }
}
