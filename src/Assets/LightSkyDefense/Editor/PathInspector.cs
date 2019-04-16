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

        for(int i = 0; i < path.curves.Length; i++)
        {
            Vector3[] points;

            points = Handles.MakeBezierPoints(
                path.curves[i].start,
                path.curves[i].end,
                path.curves[i].startTangent,
                path.curves[i].endTangent,
                30);

            Handles.DrawPolyLine(points);

            HandleUtility.Repaint();
        }
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
