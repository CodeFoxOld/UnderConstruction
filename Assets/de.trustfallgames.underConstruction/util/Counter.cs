using UnityEngine;

namespace de.trustfallgames.underConstruction.util {
    /// <summary>
    /// Automatic Counter.
    /// </summary>
    public class Counter {
        private readonly float start;
        private float current;
        private bool autoReset = true;

        private Counter() { }

        public Counter(float start) {
            current = this.start = start;
        }

        public Counter(float start, bool autoReset) {
            current = this.start = start;
            this.autoReset = autoReset;
        }

        /// <summary>
        /// Calculates the next Frame.
        /// </summary>
        /// <returns>Returns true in the Frame, when the counter goes on 0 or below</returns>
        public bool Next() {
            if (current < 0) return false;
            current -= Time.deltaTime;
            if (current > 0) {
                return false;
            }

            if (autoReset) Reset();
            return true;
        }

        public void Reset() {
            current = start;
        }
    }
}