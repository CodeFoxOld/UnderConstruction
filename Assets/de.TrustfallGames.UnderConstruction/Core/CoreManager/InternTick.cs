using System;
using System.Collections.Generic;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Core.Util;
using UnityEngine;

namespace de.TrustfallGames.UnderConstruction.Core.CoreManager {
    public class InternTick : MonoBehaviour {
        List<ObjectUpdate> updates = new List<ObjectUpdate>();

        private void FixedUpdate() {
            foreach (var obj in updates) {
                obj.InternUpdateObject.InternUpdate();
            }
        }

        public void RegisterTickObject(IInternUpdate obj, int id) {
            updates.Add(new ObjectUpdate(id, obj));
            updates = updates.OrderBy(o => o.UpdatePriority).ToList();
        }

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

        public void RemoveTickObject(IInternUpdate obj) {
            for (int i = 0; i < updates.Count; i++) {
                if (updates[i].GetHashCode() == obj.GetHashCode()) {
                    updates.RemoveAt(i);
                    i--;
                }
            }
        }
    }

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