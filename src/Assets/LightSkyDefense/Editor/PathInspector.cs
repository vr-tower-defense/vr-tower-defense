using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor
{
    private Path path;

    /// <summary>
    /// Visualize for SceneGUI
    /// </summary>
    private void OnSceneGUI()
    {
        // Bind Transform handlers to Path
        path = target as Path;
        Transform handleTransform = path.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
          handleTransform.rotation : Quaternion.identity;

        // Dont go further if path has no curves
        if (path.curves == null)
            return;

        Vector3[] pathVectors = path.GetVector3sCordinatesFromPath(100);

        // Transform path points to Path position in WorldSpace
        for(int i = 0; i < pathVectors.Length; i++)
            pathVectors[i] = handleTransform.TransformPoint(pathVectors[i]);

        // Draw GUI line
        Handles.DrawPolyLine(pathVectors);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(path, "Add Curve");
            path.AddCurve();
            EditorUtility.SetDirty(path);
        }   
    }
}
