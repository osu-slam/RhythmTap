using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class MusicElement
    {
        private enum ElementType {QuarterNote, EighthNote};
        private ElementType EleType { get; set; }
        public int MsDuration { get; set; }

        public MusicElement(string type, int msDuration)
        {
            if (type.Equals(ElementType.QuarterNote.ToString())) {
                EleType = ElementType.QuarterNote;
            } else {
                EleType = ElementType.EighthNote;
            }
            MsDuration = MsDuration;
        }

		public float GetHalfNoteDuration(int bpm){
			float halfNoteDuration = 60.0f / bpm;
			switch (EleType) {
				case ElementType.EighthNote:
					halfNoteDuration /= 2;
					break;
			}
			return halfNoteDuration;
		}
    }
}
