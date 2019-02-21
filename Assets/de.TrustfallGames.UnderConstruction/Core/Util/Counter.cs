using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.UI.Util {
    /// <summary>
    /// Automatic Counter.
    /// </summary>
    public class Counter {
        private float start;
        private bool[] marker = new bool[0];
        private float[] stops = new float[0];
        private float current;
        private bool autoReset = true;
        private bool executed;

        private Counter() { GameManager.GetManager().CounterHive.RegisterCounter(this); }

        /// <summary>
        /// Initialise a new Counter object with auto reset.
        /// </summary>
        /// <param name="start"></param>
        public Counter(float start) {
            current = this.start = start;
            GameManager.GetManager().CounterHive.RegisterCounter(this);
        }

        public Counter(ref float time) {
            
        }


        /// <summary>
        /// Initialise a new Counter object with customizable auto reset.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="autoReset"></param>
        public Counter(float start, bool autoReset) {
            current = this.start = start;
            this.autoReset = autoReset;
            GameManager.GetManager().CounterHive.RegisterCounter(this);
        }

        /// <summary>
        /// Initialise a new Counter object with customizable auto reset and markers for more event point in one cycle;
        /// </summary>
        /// <param name="start"></param>
        /// <param name="autoReset"></param>
        /// <param name="stops"></param>
        public Counter(float start, bool autoReset, params float[] stops) {
            this.autoReset = autoReset;
            current = this.start = start;
            marker = new bool[stops.Length];
            this.stops = stops;
            GameManager.GetManager().CounterHive.RegisterCounter(this);
        }

        /// <summary>
        /// Calculates the next Frame.
        /// </summary>
        /// <returns>Returns true in the Frame, when the counter goes on 0 or below</returns>
        public void Next() {
            CheckUnusedMarker();
            if (current <= 0) {
                if (autoReset) {
                    Reset();
                }

                return;
            }

            if (current > 0)
                current -= Time.fixedDeltaTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Check() {
            if (executed) return false;
            if (current <= 0) {
                executed = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the marker. Returns true in the Frame, when the marker is passed.
        /// </summary>
        /// <param name="stopIndex"></param>
        /// <returns></returns>
        public bool CheckMarker(int stopIndex) {
            if (marker[stopIndex]) return false;

            return current < stops[stopIndex];
        }

        /// <summary>
        /// Marks marker as used
        /// </summary>
        private void CheckUnusedMarker() {
            int i = 0;
            foreach (float stop in stops) {
                if (stop > current) {
                    marker[i] = true;
                }

                i++;
            }
        }

        /// <summary>
        /// Resets the counter and starts a new counter cycle
        /// </summary>
        public void Reset() {
            current = start;
            executed = false;
        }

        public void Reset(float time) {
            start = current = time;
            executed = false;
        }
        

        public float Current => current;
    }
}