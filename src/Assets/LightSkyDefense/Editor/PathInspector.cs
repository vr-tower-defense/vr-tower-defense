using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor
{
    private Path path;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        path = target as Path;
        handleTransform = path.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        if (path.curves == null)
            return;

        Vector3[] pathVectors = path.GetVector3sCordinatesFromPath(30);

        Handles.DrawPolyLine(pathVectors);

        HandleUtility.Repaint();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        path = target as Path;

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(path, "Add Curve");
            path.AddCurve();
            EditorUtility.SetDirty(path);
        }   
    }

}
