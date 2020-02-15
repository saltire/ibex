using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GroundScript))]
public class LevelSelector : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		GroundScript ground = (GroundScript)target;

		if (GUILayout.Button("Generate Meshes")) {
      ground.CreateMesh();
		}
	}
}
