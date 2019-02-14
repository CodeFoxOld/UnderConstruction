using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    public class SoundCollection : MonoBehaviour {
        [SerializeField] private List<GameObject> sounds;

        private Dictionary<SoundName, SoundFile> soundDictionary = new Dictionary<SoundName, SoundFile>();

        public SoundFile GetAudioClip(SoundName name) {
            if (soundDictionary.ContainsKey(name)) {
                return soundDictionary[name];
            }

            return null;
        }

        public SoundCollection Init() {
            foreach (GameObject file in sounds) {
                var sound = file.GetComponent<SoundFile>();
                if (soundDictionary.ContainsKey(sound.SoundName)) {
                    throw new Exception("Soundfile " + sound.SoundName + " is duplicated!");
                }

                soundDictionary.Add(sound.SoundName, sound);
            }

            Debug.Log("Sound Collection Loaded with " + soundDictionary.Count + " entrys.");
            return this;
        }
    }

    public enum SoundName {
        Horn, GameOver, Click, Plopp,
        CharacterPickup, CharacterMove, BulldozerSpawn, BulldozerMove,
        HouseStack, BackgroundMusic, NewBest, NewRecord,
        NewHighscore, PersonalBest, YouWin, Title
    }
}