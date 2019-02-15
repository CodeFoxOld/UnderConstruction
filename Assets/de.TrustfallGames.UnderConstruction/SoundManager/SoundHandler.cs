using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    [RequireComponent(typeof(SoundCollection))]
    public class SoundHandler : MonoBehaviour {
        [SerializeField] private float           sfxVolume   = 1;
        [SerializeField] private float           musicVolume = 1;
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField] private int             hiveSize;
        private                  SoundCollection collection;

        private SoundHive hive;

        private readonly Dictionary<SourceKey, AudioSource> LoopSources =
            new Dictionary<SourceKey, AudioSource>(SourceKey.HashNameComparer);

        private readonly List<AudioSource> sources = new List<AudioSource>();

        private static SoundHandler _instance;

        // Start is called before the first frame update
        private void Start() {
            DontDestroyOnLoad(gameObject);
            hive        = new SoundHive(this);
            collection  = GetComponent<SoundCollection>().Init();
            musicVolume = PlayerPrefHandler.GetMusicVolume();
            sfxVolume   = PlayerPrefHandler.GetSfxVolume();
        }

        private void FixedUpdate() {
            CheckForSilentSources();
            hiveSize = hive.Size();
        }

        private void CheckForSilentSources() {
            List<SourceKey> keys = new List<SourceKey>();
            foreach (KeyValuePair<SourceKey, AudioSource> loopSource in LoopSources) {
                if (!loopSource.Value.isPlaying) {
                    hive.Deposit(loopSource.Value);
                    keys.Add(loopSource.Key);
                }
            }

            foreach (SourceKey key in keys) {
                LoopSources.Remove(key);
            }

            for (int i = 0; i < sources.Count; i++) {
                if (!sources[i].isPlaying) {
                    hive.Deposit(sources[i]);
                    sources.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Plays a Sound without looping
        /// </summary>
        /// <param name="name"></param>
        public void PlaySound(SoundName name) { PlaySound(name, false, 0, out AudioClip clip); }

        public void PlaySound(SoundName name, bool loop, int hash) { PlaySound(name, loop, hash, out AudioClip clip); }

        public void PlaySound(SoundName name, bool loop, int hash, out AudioClip clip) {
            if (loop) {
                clip = null;
                foreach (SourceKey key in LoopSources.Keys) {
                    if (key.Name == name) return;
                }
            }

            AudioSource source   = hive.Draw();
            SoundFile   file     = collection.GetAudioClip(name);
            source.clip   = clip = file.Clip;
            source.volume = GetAudioVolume(file.SoundType, file.Volume);
            source.loop   = loop;
            if (loop) {
                LoopSources.Add(new SourceKey(hash, name), source);
            } else {
                sources.Add(source);
            }

            source.Play();
        }

        /// <summary>
        /// Stops a specific sound from a specific object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        public void StopSound(SoundName name, int hash) {
            if (LoopSources.TryGetValue(new SourceKey(hash, name), out AudioSource a)) {
                a.Stop();
            }
        }

        /// <summary>
        /// Stops all sounds from a specific object
        /// </summary>
        /// <param name="hash"></param>
        public void StopSound(int hash) {
            foreach (KeyValuePair<SourceKey, AudioSource> pair in LoopSources) {
                if (pair.Key.Hash == hash) {
                    pair.Value.Stop();
                }
            }
        }

        /// <summary>
        /// Stops all sounds
        /// </summary>
        public void StopSound() {
            foreach (AudioSource value in LoopSources.Values) {
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
            go.name = "SoundSource";
            var source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer;
            source.playOnAwake           = false;
            return source;
        }

        /// <summary>
        /// Returns the Volume from the PlayerPrefs
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private float GetAudioVolume(SoundType type, float baseVolume) {
            Debug.Log("Playing with Volume: " + (type == SoundType.Music ? musicVolume : sfxVolume) * baseVolume);
            return (type == SoundType.Music ? musicVolume : sfxVolume) * baseVolume;
        }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        public static SoundHandler GetInstance() { return _instance; }

        public void Refresh() {
            musicVolume = PlayerPrefHandler.GetMusicVolume();
            sfxVolume   = PlayerPrefHandler.GetSfxVolume();
        }
        
    }

    public enum SoundType { Music, SFX }

    public class SourceKey {
        private readonly int       hash;
        private readonly SoundName name;

        public SourceKey(int hash, SoundName name) {
            this.hash = hash;
            this.name = name;
        }

        public int       Hash => hash;
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