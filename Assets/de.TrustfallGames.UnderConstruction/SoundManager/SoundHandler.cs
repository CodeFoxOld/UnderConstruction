﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Util;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    [RequireComponent(typeof(SoundCollection))]
    public class SoundHandler : MonoBehaviour {
        [SerializeField] private float sfxVolume = 1;
        [SerializeField] private float musicVolume = 1;
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField] private int hiveSize;
        [SerializeField] private GameObject soundSourcePrefab;
        private SoundCollection collection;

        private SoundHive hive;

        private readonly Dictionary<SourceKey, SoundSource> LoopSources =
            new Dictionary<SourceKey, SoundSource>(SourceKey.HashNameComparer);

        private readonly List<AudioSource> sources = new List<AudioSource>();

        private static SoundHandler _instance;

        // Start is called before the first frame update
        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        private void FixedUpdate() {
            CheckForSilentSources();
            hiveSize = hive.Size();
        }

        private void CheckForSilentSources() {
            List<SourceKey> keys = new List<SourceKey>();
            foreach (KeyValuePair<SourceKey, SoundSource> loopSource in LoopSources) {
                if (!loopSource.Value.Source.isPlaying) {
                    hive.Deposit(loopSource.Value.Source);
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

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loop"></param>
        /// <param name="hash"></param>
        public void PlaySound(SoundName name, bool loop, int hash) { PlaySound(name, loop, hash, out AudioClip clip); }

        /// <summary>
        /// plays a sound and returns the played audio clip
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loop"></param>
        /// <param name="hash"></param>
        /// <param name="clip"></param>
        public void PlaySound(SoundName name, bool loop, int hash, out AudioClip clip) {
            if (loop) {
                clip = null;
                foreach (SourceKey key in LoopSources.Keys) {
                    if (key.Name == name) return;
                }
            }

            AudioSource source = hive.Draw();
            SoundFile file = collection.GetAudioClip(name);
            source.clip = clip = file.Clip;
            source.volume = GetAudioVolume(file);
            source.loop = loop;
            if (loop) {
                LoopSources.Add(new SourceKey(hash, name), new SoundSource(file, source));
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
            if (LoopSources.TryGetValue(new SourceKey(hash, name), out SoundSource a)) {
                a.Source.Stop();
            }
        }

        /// <summary>
        /// Stops all sounds from a specific object
        /// </summary>
        /// <param name="hash"></param>
        public void StopSound(int hash) {
            foreach (KeyValuePair<SourceKey, SoundSource> pair in LoopSources) {
                if (pair.Key.Hash == hash) {
                    pair.Value.Source.Stop();
                }
            }
        }

        /// <summary>
        /// Stops all sounds
        /// </summary>
        public void StopSound() {
            foreach (SoundSource value in LoopSources.Values) {
                value.Source.Stop();
            }
        }

        /// <summary>
        /// Creates a new Sound Source at the camera.
        /// </summary>
        /// <returns></returns>
        internal AudioSource CreateNewSoundSource() {
            GameObject go = Instantiate(
                soundSourcePrefab, Camera.main.transform.position, Camera.main.transform.rotation, transform);
            go.name = "SoundSource";
            return go.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Returns the Volume from the PlayerPrefs
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private float GetAudioVolume(SoundFile file) {
            return (file.SoundType == SoundType.Music ? musicVolume : sfxVolume) * file.Volume;
        }

        private void Awake() {
            if (_instance == null)
                _instance = this;
            else if (_instance != this) {
                Destroy(gameObject);
            }

            hive = new SoundHive(this);
            collection = GetComponent<SoundCollection>().Init();
            musicVolume = PlayerPrefHandler.GetMusicVolume();
            sfxVolume = PlayerPrefHandler.GetSfxVolume();
        }

        public static SoundHandler GetInstance() { return _instance; }

        /// <summary>
        /// Reloads the volume settings
        /// </summary>
        public void Refresh() {
            musicVolume = PlayerPrefHandler.GetMusicVolume();
            sfxVolume = PlayerPrefHandler.GetSfxVolume();
            foreach (SoundSource source in LoopSources.Values) {
                source.Source.volume = GetAudioVolume(source.File);
            }
        }

        /// <summary>
        /// Returns the length of a audio clip
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetSoundLength(SoundName name) {
            return collection.GetAudioClip(name).Clip.length;
        }
    }

    public enum SoundType { Music, SFX }

    /// <summary>
    /// Class to save a key for a sound source in a dictionary
    /// </summary>
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