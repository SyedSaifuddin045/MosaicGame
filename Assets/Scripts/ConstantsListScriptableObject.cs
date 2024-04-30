using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstantsListScriptableObject", menuName = "ConstantsListScriptableObject", order = 0)]
public class ConstantsListScriptableObject : ScriptableObject
{
    public List<Color> Colors = new List<Color>();
    public List<StencilScriptableObject> stencilScriptableObjects = new List<StencilScriptableObject>();
}