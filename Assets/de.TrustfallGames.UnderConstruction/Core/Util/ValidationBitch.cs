using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.Util {
    /// <summary>
    /// Validates the unique id with the one in the playerprefs
    /// </summary>
    public class ValidationBitch : MonoBehaviour {
        [SerializeField] private int uniqueID;

        void Start() { PlayerPrefHandler.ValidateVersionNumber(uniqueID); }
    }
}