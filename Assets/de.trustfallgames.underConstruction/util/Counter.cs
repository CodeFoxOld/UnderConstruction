using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Util {
    /// <summary>
    /// Automatic Counter.
    /// </summary>
    public class Counter {
        private readonly float start;
        private bool[] marker = new bool[0];
        private float[] stops = new float[0];
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

        /// <summary>
        /// Initialise a new Counter object with customizable auto reset and markers for more event point in one cycle;
        /// </summary>
        /// <param name="start"></param>
        /// <param name="autoReset"></param>
        /// <param name="stops"></param>
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
            CheckUnusedMarker();
            if (current < 0) return;
            current -= Time.deltaTime;
            if (current > 0)
                return;
            if (autoReset) Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Marks marker as used
        /// </summary>
        private void CheckUnusedMarker() {
            const int i = 0;
            foreach (float stop in stops) {
                if (stop < current && marker[i] == false) {
                    marker[i] = true;
                }
            }
        }

        /// <summary>
        /// Resets the counter and starts a new counter cycle
        /// </summary>
        public void Reset() { current = start; }

        public float Current => current;
    }
}