using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StencilScriptableObject", menuName = "StencilScriptableObject", order = 0)]
public class StencilScriptableObject : ScriptableObject
{
    public Stencil_Type type;
    public bool symmetrical;
    public Sprite sprite;
    public Stencil_Rotation rotation;
}

[CustomEditor(typeof(StencilScriptableObject))]
public class StencilScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StencilScriptableObject stencilObject = (StencilScriptableObject)target;

        EditorGUI.BeginChangeCheck();

        // Display other fields
        stencilObject.type = (Stencil_Type)EditorGUILayout.EnumPopup("Type", stencilObject.type);
        stencilObject.symmetrical = EditorGUILayout.Toggle("Symmetrical", stencilObject.symmetrical);
        stencilObject.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", stencilObject.sprite, typeof(Sprite), false);

        // Check if symmetrical is false, then display rotation
        if (!stencilObject.symmetrical)
        {
            stencilObject.rotation = (Stencil_Rotation)EditorGUILayout.EnumPopup("Rotation", stencilObject.rotation);
        }

        if (EditorGUI.EndChangeCheck())
        {
            // Apply changes
            EditorUtility.SetDirty(stencilObject);
        }
    }
}
