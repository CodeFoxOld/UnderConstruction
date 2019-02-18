using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.SoundManager {
    public class PlaySoundOnStart : MonoBehaviour {
        [SerializeField] private SoundName name;
        [SerializeField] private bool      loop;

        private void Start() {
            SoundHandler.GetInstance().PlaySound(name, loop, GetInstanceID());
        }
    }
}