using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor
{
    private Path _path;

    /// <summary>
    /// Visualize for SceneGUI
    /// </summary>
    private void OnSceneGUI()
    {
        // Bind Transform handlers to Path
        _path = target as Path;
        var handleTransform = _path.transform;
        var handleRotation = Tools.pivotRotation == PivotRotation.Local ?
          handleTransform.rotation : Quaternion.identity;

        // Dont go further if path has no curves
        if (_path.Curves == null)
            return;

        var pathVectors = _path.GetVector3sCoordinatesFromPath(100);

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
            Undo.RecordObject(_path, "Add Curve");
            _path.AddCurve();
            EditorUtility.SetDirty(_path);
        }   
    }
}
