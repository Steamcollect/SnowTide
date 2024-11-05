using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSwitchSelection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Image> images;
    
    [Header("Settings")]
    [SerializeField] private float transparency;

    public void SwitchIconVisible(int index)
    {
        Color color;
        for (int i = 0; i < images.Count; i++)
        {
            color = images[i].color;
            color.a = index == i ? 1f: transparency;
            images[i].color = color;
        }
    }
    
}
