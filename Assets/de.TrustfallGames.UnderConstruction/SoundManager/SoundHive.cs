using System.Collections.Generic;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    public class SoundHive {
        private SoundHandler soundHandler;

        private SoundHive() { }

        public SoundHive(SoundHandler parent) { soundHandler = parent; }

        private Stack<AudioSource> sources = new Stack<AudioSource>();

        public AudioSource Draw() {
            if (sources.Count == 0) {
                return soundHandler.CreateNewSoundSource();
            }

            return sources.Pop();
        }

        public void Deposit(AudioSource source) { sources.Push(source); }
    }
}