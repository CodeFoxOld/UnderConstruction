using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI.menu {
    /// <summary>
    ///Class rotates the attached gameobject over time
    /// </summary>
    public class MainMenuCameraBehaviour : MonoBehaviour
    {
        [Header("Rotation speed of the examine camera")] [Range(0,50)] [SerializeField]
        private float rotationSpeed = 5f;
    
        void Update()
        {
            transform.Rotate(0, -(rotationSpeed * Time.deltaTime), 0);
        }
    }
}
