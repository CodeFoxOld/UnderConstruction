using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core;
using UnityEngine;

public class ValidationBitch : MonoBehaviour
{
    [SerializeField] private int uniqueID;

    void Start()
    {
        PlayerPrefHandler.ValidateVersionNumber(uniqueID);
    }
}
