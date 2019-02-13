using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalancingUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textField;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void FixedUpdate() {
        string a = "";
        try {
            a = string.Concat(a, "Grow Interval: " + Math.Round(GameManager.GetManager().Settings.GetGrowInterval(),2) + "\n");
        } catch (NullReferenceException e) {
            a = string.Concat(a, "Grow Interval: 0\n");
        }

        try {
            a = string.Concat(a, "Spawn Interval: " + Math.Round(GameManager.GetManager().Settings.GetSpawnInterval(),2) + "\n");
        } catch (NullReferenceException e) {
            a = string.Concat(a, "Spawn Interval: 0\n");
        }

        try {
            a = string.Concat(a, "Spawn Duration: " + Math.Round(GameManager.GetManager().Settings.GetSpawnDuration(),2) + 
            "\n");
        } catch (NullReferenceException e) {
            a = string.Concat(a, "Spawn Duration: 0\n");
        }

        textField.text = a;
    }
}