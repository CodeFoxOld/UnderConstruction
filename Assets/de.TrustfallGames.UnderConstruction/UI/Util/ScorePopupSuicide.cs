﻿using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI.Util {
    

public class ScorePopupSuicide : MonoBehaviour {
    public float delay;
 
    // Use this for initialization
    void Start () {
        Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 
    }
}
}
