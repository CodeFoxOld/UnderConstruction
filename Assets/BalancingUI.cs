using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.GameTimeManager;
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
            a = string.Concat(a, "Reg. DayTime Objects: " + GameTimeHandler.GetInstance().RegisteredObjectsCount()+"\n");
        } catch (NullReferenceException e) {
            a = string.Concat(a, "Grow Interval: 0\n");
        }
        textField.text = a;
    }
}