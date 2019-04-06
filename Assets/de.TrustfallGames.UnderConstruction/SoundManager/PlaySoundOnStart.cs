using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    /// <summary>
    /// Plays the attached sound on start with or without loop
    /// </summary>
    public class PlaySoundOnStart : MonoBehaviour {
        [SerializeField] private SoundName name;
        [SerializeField] private bool      loop;

        private void Start() {
            SoundHandler.GetInstance().PlaySound(name, loop, GetInstanceID());
        }
    }
}