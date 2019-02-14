using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    [RequireComponent(typeof(SoundCollection))]
    public class SoundHandler : MonoBehaviour {
        [SerializeField] private float sfxVolume = 1;
        [SerializeField] private float musicVolume = 1;
        private SoundCollection collection;

        private SoundHive hive;

        private readonly Dictionary<SourceKey, AudioSource> sources = new Dictionary<SourceKey, AudioSource>(SourceKey.HashNameComparer);
        private SoundHandler _instance;

        // Start is called before the first frame update
        private void Start() {
            DontDestroyOnLoad(gameObject);
            hive = new SoundHive(this);
            collection = GetComponent<SoundCollection>().Init();
            musicVolume = PlayerPrefHandler.GetMusicVolume();
            sfxVolume = PlayerPrefHandler.GetSfxVolume();
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
            AudioSource source = hive.Draw();
            SoundFile file = collection.GetAudioClip(name);
            source.clip = file.Clip;
            source.volume = GetAudioVolume(file.SoundType, file.Volume);
            source.loop = loop;
            sources.Add(new SourceKey(hash, name), source);
        }

        /// <summary>
        /// Stops a specific sound from a specific object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        public void StopSound(SoundName name, int hash) {
            if (sources.TryGetValue(new SourceKey(hash, name), out AudioSource a)) {
                a.Stop();
            }
        }

        /// <summary>
        /// Stops all sounds from a specific object
        /// </summary>
        /// <param name="hash"></param>
        public void StopSound(int hash) {
            foreach (KeyValuePair<SourceKey, AudioSource> pair in sources) {
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

        /// <summary>
        /// Creates a new Sound Source at the camera.
        /// </summary>
        /// <returns></returns>
        internal AudioSource CreateNewSoundSource() {
            GameObject go = Instantiate(
                                        new GameObject(), Camera.main.transform.position, new Quaternion(0, 0, 0, 0),
                                        transform);
            return go.AddComponent<AudioSource>();
        }

        /// <summary>
        /// Returns the Volume from the PlayerPrefs
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private float GetAudioVolume(SoundType type, float baseVolume) {
            return (type == SoundType.Music ? musicVolume : sfxVolume) * baseVolume;
        }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        public SoundHandler GetInstance() { return _instance; }

    }

    public enum SoundType { Music, SFX }

    public class SourceKey {
        private readonly int hash;
        private readonly SoundName name;

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

            public int GetHashCode(SourceKey obj) { return 1; }
        }

        public static IEqualityComparer<SourceKey> HashNameComparer { get; } = new HashNameEqualityComparer();
    }
}