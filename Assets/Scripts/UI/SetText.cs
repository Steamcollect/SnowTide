using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] RSO_TxtSet textSet;

    private void Awake()
    {
        textSet.Value = text;
    }
}