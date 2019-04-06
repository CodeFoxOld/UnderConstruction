using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    /// <summary>
    /// Class to save a sound
    /// </summary>
    public class SoundFile : MonoBehaviour {
        [SerializeField] private SoundType soundType;
        [SerializeField] private SoundName soundName;
        [SerializeField] private AudioClip clip;
        [SerializeField] [Range(0,1)] private float volume = 1;
        [SerializeField] private bool validate = false;
        public SoundType SoundType => soundType;
        public SoundName SoundName => soundName;
        public AudioClip Clip => clip;
        public float Volume => volume;

        private void OnValidate() {
            if(!validate) return;
            var clip = this.clip == null ? "null" : this.clip.name;
            gameObject.name = "Type: " + soundType + " | Name: " + soundName + " | Clip: " + clip + " at Volume " + volume;
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}