using System.Linq;
using UnityEditor;

//[CustomEditor(typeof(HideLocationMarker))]
public class HideLocationEditor : OrderEditor
{
    protected SerializedProperty locationProp;

    protected int locationVarIndex = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        locationProp = serializedObject.FindProperty("location");
    }

    public override void OnInspectorGUI()
    {
        DrawOrderGUI();
    }

    public override void DrawOrderGUI()
    {
        serializedObject.Update();

        HideLocationMarker t = target as HideLocationMarker;
        var engine = (BasicFlowEngine)t.GetEngine();

        var locationVars = engine.GetComponents<LocationVariable>();
        for (int i = 0; i < locationVars.Length; i++)
        {
            if (locationVars[i] == locationProp.objectReferenceValue as LocationVariable)
            {
                locationVarIndex = i;
            }
        }

        locationVarIndex = EditorGUILayout.Popup("Location", locationVarIndex, locationVars.Select(x => x.Key).ToArray());
        if (locationVars.Length > 0)
            locationProp.objectReferenceValue = locationVars[locationVarIndex];

        serializedObject.ApplyModifiedProperties();
    }
}
