using Assets.Scripts.TextureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assets.Scripts
{
    class RhythmLoader
    {
        List<MusicElement> musicElements;
		List<float> times;
		float msRhythmDuration;
		float msTimeLim;
		float bpm;

        public RhythmLoader()
        {
            musicElements = new List<MusicElement>();
			times = new List<float> ();
			msRhythmDuration = 0.0f;
			msTimeLim = 0.0f;
			bpm = 0.0f;
        }

		public void LoadRhythm(String input, float bpm, float timeLimit)
        {
			this.bpm = bpm;
			Parse (input);
			msTimeLim = timeLimit;
        }

		private void Parse(String input){
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
		}
			

		public List<float> GetRhythmTimes(){
			float time = 0.0f;
			int numCycles = (int) (msTimeLim / msRhythmDuration) + 8;
			for (int i = 0; i < numCycles; i++) {
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
