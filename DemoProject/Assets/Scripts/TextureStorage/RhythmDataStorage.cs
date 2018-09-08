using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.TextureStorage
{
    public static class RhythmDataStorage
    {
        private static XmlDocument rhythm;
		private static string[] names = new string[] { 
			"how-are-you", 
			"im-hun-gry", 
			"what-is-your-name", 
			"i-am-thir-sty",
			"i-want-to-sing",
			"isochronous"
		};

        public static XmlDocument GetRhythm()
        {
			int gameNum = MenuController.gameNum;
			int bpm = Convert.ToInt32(MenuController.bpm);
			string file = Application.streamingAssetsPath + "/RhythmData/" + names [gameNum] + "-" + bpm + ".xml";
			rhythm = new XmlDocument();
			//rhythm.Load("./Assets/Scripts/RhythmData/rhythm.xml");
			rhythm.Load(file);
            return rhythm;
        }
    }
}
