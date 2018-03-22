using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assets.Scripts.TextureStorage
{
    public static class RhythmDataStorage
    {
        private static XmlDocument rhythm;
        public static XmlDocument GetRhythm()
        {
            rhythm = new XmlDocument();
			rhythm.Load("./Assets/Scripts/RhythmData/rhythm.xml");
            return rhythm;
        }
    }
}
