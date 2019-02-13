using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    public class SoundHandler : MonoBehaviour {
        [SerializeField] private SoundFile[] sounds;
        [SerializeField] private SoundCollection collection;

        private SoundHive hive;

        private Dictionary<SourceKey, AudioSource> sources;

        // Start is called before the first frame update
        void Start() {
            DontDestroyOnLoad(gameObject);
            hive = new SoundHive(this);
        }

        private void FixedUpdate() { CheckForSilentSources(); }

        private void CheckForSilentSources() {
            foreach (KeyValuePair<SourceKey, AudioSource> source in sources) {
                if (!source.Value.isPlaying) {
                    hive.Deposit(source.Value);
                    sources.Remove(source.Key);
                }
            }
        }

        public void PlaySound(SoundName name, bool loop, int hash) {
            var source = hive.Draw();
            var file = collection.GetAudioClip(name);
            source.clip = file.Clip;
            source.volume = GetAudioVolume(file.AudioType);
            source.loop = loop;
            sources.Add(new SourceKey(hash,name), source);
        }

        /// <summary>
        /// Stops a specific sound from a specific object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        public void StopSound(SoundName name, int hash) {
            AudioSource a;
            if (sources.TryGetValue(new SourceKey(hash, name), out a)) {
                a.Stop();
            }
        }

        /// <summary>
        /// Stops all sounds from a specific object
        /// </summary>
        /// <param name="hash"></param>
        public void StopSound(int hash) {
            foreach (KeyValuePair<SourceKey,AudioSource> pair in sources) {
                if (pair.Key.Hash == hash) {
                    pair.Value.Stop();
                }
            }
        }

        /// <summary>
        /// Stops all sounds
        /// </summary>
        public void StopSound() {
            foreach (AudioSource value in sources.Values) {
                value.Stop();
            }
        }

        internal AudioSource CreateNewSoundSource() {
            var go = Instantiate(new GameObject());
            return go.AddComponent<AudioSource>();
        }

        private float GetAudioVolume(SoundType type) {
            return type == SoundType.Music ? PlayerPrefs.GetFloat("MusicVolume") : PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    public enum SoundType { Music, SFX }

    public class SourceKey {
        private int hash;
        private SoundName name;

        public SourceKey(int hash, SoundName name) {
            this.hash = hash;
            this.name = name;
        }

        public int Hash => hash;
        public SoundName Name => name;

        private sealed class HashNameEqualityComparer : IEqualityComparer<SourceKey> {
            public bool Equals(SourceKey x, SourceKey y) {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.hash == y.hash && x.name == y.name;
            }

            public int GetHashCode(SourceKey obj) {
                    return 1;
            }
        }

        public static IEqualityComparer<SourceKey> HashNameComparer { get; } = new HashNameEqualityComparer();
    }
}