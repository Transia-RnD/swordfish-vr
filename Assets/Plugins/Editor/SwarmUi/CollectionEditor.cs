using Plugins.SwarmUi.Collection;
using UnityEditor;
using UnityEngine;

namespace Plugins.Editor.SwarmUi {
    [CustomEditor(typeof(GridObjectCollection))]
    public class CollectionEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GridObjectCollection gridCollection = (GridObjectCollection) target;
            if (GUILayout.Button("Update collection")) {
                gridCollection.UpdateCollection();
            }
        }
    }
}