using System;
using System.Collections.Generic;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Util;
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
            PrintList();
        }

        private void PrintList() {
            string a = "";
            foreach (var VARIABLE in updates) {
                a = string.Concat(
                                  a,
                                  VARIABLE.UpdatePriority + " " + VARIABLE.InternUpdateObject.GetType().ToString()
                                  + "\n");
            }

            Debug.Log("Current Update Prio Report! \n" + a);
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