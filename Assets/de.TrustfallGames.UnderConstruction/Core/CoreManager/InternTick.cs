using System;
using System.Collections.Generic;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Core.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    /// <summary>
    /// Class to manage the intern tick objects
    /// </summary>
    public class InternTick : MonoBehaviour {
        List<ObjectUpdate> updates = new List<ObjectUpdate>();

        private void FixedUpdate() {
            foreach (var obj in updates) {
                obj.InternUpdateObject.InternUpdate();
            }
        }

        /// <summary>
        /// Registers a intern Update object with a update priority. Low Priority number means early update
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="updatePriority"></param>
        public void RegisterTickObject(IInternUpdate obj, int updatePriority) {
            updates.Add(new ObjectUpdate(updatePriority, obj));
            updates = updates.OrderBy(o => o.UpdatePriority).ToList();
        }

        /// <summary>
        /// Prints the current update prio
        /// </summary>
        [ContextMenu("Print Update Prio")]
        public void PrintList() {
            string a = "";
            foreach (var VARIABLE in updates) {
                a = string.Concat(
                                  a,
                                  VARIABLE.UpdatePriority + " " + VARIABLE.InternUpdateObject.GetType().ToString()
                                  + "\n");
            }

            Debug.Log("Current Update Prio Report! \n" + a);
        }

        /// <summary>
        /// Unregister object from intern tick
        /// </summary>
        /// <param name="obj"></param>
        public void UnregisterTickObject(IInternUpdate obj) {
            for (int i = 0; i < updates.Count; i++) {
                if (updates[i].GetHashCode() == obj.GetHashCode()) {
                    updates.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Class to safe a update object
    /// </summary>
    internal class ObjectUpdate {
        private int updatePriority;
        private IInternUpdate internUpdateObject;

        public ObjectUpdate(int updatePriority, IInternUpdate internUpdateObject) {
            this.updatePriority = updatePriority;
            this.internUpdateObject = internUpdateObject;
        }

        public int UpdatePriority => updatePriority;
        public IInternUpdate InternUpdateObject => internUpdateObject;
    }
}