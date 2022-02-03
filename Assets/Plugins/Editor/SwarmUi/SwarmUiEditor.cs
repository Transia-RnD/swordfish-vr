using Plugins.SwarmUi;
using UnityEditor;
using UnityEngine;

namespace Plugins.Editor.SwarmUi {
    [CustomEditor(typeof(SwarmUiController))]
    public class SwarmUiEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            SwarmUiController swarmUiController = (SwarmUiController) target;
            if (GUILayout.Button("Test animation")) {
                swarmUiController.ToggleVisibility();
            }
        }
    }
}