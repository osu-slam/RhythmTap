using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class MusicElement : MonoBehaviour
    {
		private enum ElementType {Quarter_Note, Eighth_Note, Eighth_Note_T, Quarter_Rest, Eighth_Rest};
        private ElementType EleType { get; set; }
		private bool isRest;

        public MusicElement(string type)
        {
            if (type.Equals(ElementType.Quarter_Note.ToString())) {
                EleType = ElementType.Quarter_Note;
				isRest = false;
			} else if (type.Equals(ElementType.Eighth_Note_T.ToString())){
				EleType = ElementType.Eighth_Note_T;
				isRest = false;
			} else if (type.Equals(ElementType.Eighth_Note.ToString())){
				EleType = ElementType.Eighth_Note;
				isRest = false;
			} else if (type.Equals(ElementType.Quarter_Rest.ToString())){
				EleType = ElementType.Quarter_Rest;
				isRest = true;
			} else {
				EleType = ElementType.Eighth_Rest;
				isRest = true;
			}
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
				case ElementType.Eighth_Rest:
					noteDuration /= 2;
					break;
			}
			return noteDuration;
		}

		public bool IsRest()
		{
			return isRest;
		}
    }
}
