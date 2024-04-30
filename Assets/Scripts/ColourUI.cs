using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourUI : MonoBehaviour, IPointerDownHandler
{
    public static Action<Color> ColorUIClicked;
    Image ColourImage;
    private void Awake()
    {
        ColourImage = gameObject.GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        InvokeColourChangeEvent();
    }

    public void InvokeColourChangeEvent()
    {
        if (ColourImage != null)
            ColorUIClicked?.Invoke(ColourImage.color);
        else
            Debug.LogWarning("Colour Image is Null");
    }
}