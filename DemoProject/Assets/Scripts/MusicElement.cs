using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class MusicElement
    {
		private enum ElementType {QuarterNote, EighthNote, Eighth_Note_T};
        private ElementType EleType { get; set; }
        public int MsDuration { get; set; }

        public MusicElement(string type, int msDuration)
        {
            if (type.Equals(ElementType.QuarterNote.ToString())) {
                EleType = ElementType.QuarterNote;
			} else if (type.Equals(ElementType.Eighth_Note_T.ToString())){
				EleType = ElementType.Eighth_Note_T;
			} else {
                EleType = ElementType.EighthNote;
            }
            MsDuration = msDuration;
        }

		public float GetNoteDuration(int bpm){
			float noteDuration = 60.0f / bpm;
			switch (EleType) {
				case ElementType.EighthNote:
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
