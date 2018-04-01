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
        XmlDocument doc;
        List<MusicElement> musicElements;
		List<float> times;
        public RhythmLoader()
        {
            musicElements = new List<MusicElement>();
			times = new List<float> ();
        }

        public void LoadRhythm(XmlDocument xml)
        {
            doc = xml;
            Load();
        }

        private void Load()
        {
            XmlNodeList elements = doc.GetElementsByTagName("element");
            foreach (XmlNode element in elements)
            {
                string type = element["type"].InnerText;
                int duration = Convert.ToInt32(element["time"].InnerText);
                musicElements.Add(new MusicElement(type, duration));
				times.Add ((float)duration/1000);
            }
        }

		public List<float> GetRhythmTimes(){
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
			foreach (MusicElement me in musicElements) {
				durations.Add(me.GetNoteDuration(bpm));
			}
			return durations;
		}
    }
}
