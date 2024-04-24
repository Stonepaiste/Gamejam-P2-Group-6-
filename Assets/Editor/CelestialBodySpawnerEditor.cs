using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CelestialBodySpawner))]
public class CelestialBodySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CelestialBodySpawner myScript = (CelestialBodySpawner)target;
        if (GUILayout.Button("Spawn"))
        {
            myScript.ClearAndSpawn();
        }
    }
}