using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assets.Scripts.TextureStorage
{
    public static class RhythmSoundStorage
    {
		public static void GetAudio(AudioSource source)
        {
			source.clip = MenuController.audioClip;
			source.playOnAwake = false;
        }
    }
}
