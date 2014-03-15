using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PipeManager))]
public class PipeManagerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		PipeManager pipeManager = (PipeManager)target;
		if (GUILayout.Button("Set Mountains")) {
			pipeManager.SetMountains();
		}
	}
}
