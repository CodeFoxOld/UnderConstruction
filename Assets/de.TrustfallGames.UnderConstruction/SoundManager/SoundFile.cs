using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    public class SoundFile : MonoBehaviour {
        [SerializeField] private SoundType audioType;
        [SerializeField] private SoundName soundName;
        [SerializeField] private AudioClip clip;
        public SoundType AudioType => audioType;
        public SoundName SoundName => soundName;
        public AudioClip Clip => clip;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}