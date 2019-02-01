using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Util {
    /// <summary>
    /// Automatic Counter.
    /// </summary>
    public class Counter {
        private readonly float start;
        private bool[] marker;
        private float[] stops;
        private float current;
        private bool autoReset = true;

        private Counter() { GameManager.GetManager().RegisterCounter(this); }

        /// <summary>
        /// Initialise a new Counter object with auto reset.
        /// </summary>
        /// <param name="start"></param>
        public Counter(float start) {
            current = this.start = start;
            GameManager.GetManager().RegisterCounter(this);
        }

        /// <summary>
        /// Initialise a new Counter object with customizable auto reset.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="autoReset"></param>
        public Counter(float start, bool autoReset) {
            current = this.start = start;
            this.autoReset = autoReset;
            GameManager.GetManager().RegisterCounter(this);
        }

        public Counter(float start, bool autoReset, params float[] stops) {
            marker = new bool[stops.Length];
            this.stops = stops;
            GameManager.GetManager().RegisterCounter(this);
        }

        /// <summary>
        /// Calculates the next Frame.
        /// </summary>
        /// <returns>Returns true in the Frame, when the counter goes on 0 or below</returns>
        public void Next() {
            if (current < 0) return;
            current -= Time.deltaTime;
            if (current > 0)
                return;
            if (autoReset) Reset();
        }

        public bool Check() { return !(current > 0); }

        /// <summary>
        /// Checks the marker. Returns true in the Frame, when the marker is passed.
        /// </summary>
        /// <param name="stopIndex"></param>
        /// <returns></returns>
        public bool CheckMarker(int stopIndex) {
            if (marker[stopIndex]) return false;
            if (current > stops[stopIndex]) {
                return true;
            }

            return false;
        }

        private void CheckUnusedMarker() {
            int i = 0;
            foreach (var stop in stops) {
                if (stop < current && marker[i] == false) {
                    marker[i] = true;
                }
            }
        }

        public void Reset() { current = start; }

        public float Current => current;
    }
}