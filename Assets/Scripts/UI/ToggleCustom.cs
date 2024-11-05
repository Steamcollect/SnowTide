using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleCustom : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private UnityEvent<float> onValueChanged; 
    [SerializeField] private Image _image;
    
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _selectedSprite;

    private void OnClick(bool isSelected)
    {
        _image.sprite = isSelected ? _selectedSprite : _defaultSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool selected = !(_image.sprite == _selectedSprite);
        OnClick(selected);
        onValueChanged.Invoke(selected ? 1f : 0f);
    }
}
