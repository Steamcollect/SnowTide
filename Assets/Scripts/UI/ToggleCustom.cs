using System;
using System.Collections;
using System.Collections.Generic;
using BT.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleCustom : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent<float> onValueChanged; 
    [SerializeField] private Image _image;
    
    [SerializeField] private SoundType _soundType;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private AudioMixer audioMixer;

    private void OnEnable()
    { 
        UpdateVolumeClick();   
    }
    
    private void UpdateVolumeClick()
    {
        var nameParameter = _soundType switch
        {
            SoundType.Sound => "SoundVolume",
            SoundType.Music => "MusicVolume",
            _ => ""
        };
        if (nameParameter == "") return;
        float value;
        audioMixer.GetFloat(nameParameter,out value);
        OnClick(!Mathf.Approximately(value, -80f));
    }

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
