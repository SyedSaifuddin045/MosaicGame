using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StencilUI : MonoBehaviour, IPointerDownHandler
{
    public static Action<int> StencilUIClicked;
    public void OnPointerDown(PointerEventData eventData)
    {
        int index = transform.GetSiblingIndex();
        // Debug.Log("Index of Stencil Clicked " + index);
        StencilUIClicked?.Invoke(index);
    }
}