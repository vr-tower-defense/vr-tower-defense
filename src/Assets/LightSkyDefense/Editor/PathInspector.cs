using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor
{
    private Path path;

    private void OnSceneGUI()
    {
        path = target as Path;
        Transform handleTransform = path.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
          handleTransform.rotation : Quaternion.identity;

        if (path.curves == null)
            return;

        Vector3[] pathVectors = path.GetVector3sCordinatesFromPath(100);

        for(int i = 0; i < pathVectors.Length - 1; i++)
        {
            pathVectors[i] = handleTransform.TransformPoint(pathVectors[i]);
        }

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
