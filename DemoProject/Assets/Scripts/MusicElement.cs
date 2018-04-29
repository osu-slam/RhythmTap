using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class MusicElement
    {
		private enum ElementType {Quarter_Note, Eighth_Note, Eighth_Note_T};
        private ElementType EleType { get; set; }
        public int MsDuration { get; set; }

        public MusicElement(string type, int msDuration)
        {
            if (type.Equals(ElementType.Quarter_Note.ToString())) {
                EleType = ElementType.Quarter_Note;
			} else if (type.Equals(ElementType.Eighth_Note_T.ToString())){
				EleType = ElementType.Eighth_Note_T;
			} else {
                EleType = ElementType.Eighth_Note;
            }
            MsDuration = msDuration;
        }

		public float GetNoteDuration(int bpm){
			//quarter note duration
			float noteDuration = 60.0f / bpm;
			switch (EleType) {
				case ElementType.Eighth_Note:
					noteDuration /= 2;
					break;
				case ElementType.Eighth_Note_T:
					noteDuration /= 3;
					break;
			}
			return noteDuration;
		}
    }
}
