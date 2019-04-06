using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    /// <summary>
    /// Class which holds a Sound source and a file
    /// </summary>
    public class SoundSource {
        private SoundFile file;
        private AudioSource source;

        public SoundFile File => file;
        public AudioSource Source => source;

        public SoundSource(SoundFile file, AudioSource source) {
            this.file = file;
            this.source = source;
        }
    }
}