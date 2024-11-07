using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHeasterEgg : MonoBehaviour
{
    [SerializeField] GameObject initialSkin;
    [SerializeField] GameObject goldSkin;
    [SerializeField] Material mat;

    float color = 0;

    bool isSkinActive = false;

    [SerializeField] RSE_BasicEvent rseOnGameStart;
    [SerializeField] RSE_BasicEvent rseOnSkinChange;

    private void OnEnable()
    {
        rseOnGameStart.action += Start;
        rseOnSkinChange.action += ActiveSkin;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= Start;
        rseOnGameStart.action -= ActiveSkin;
    }

    private void Start()
    {
        isSkinActive = false;
        initialSkin.SetActive(true);
        goldSkin.SetActive(false);
    }

    private void Update()
    {
        if (isSkinActive)
        {
            color += Time.deltaTime * .8f;
            if (color > 1)
            {
                color = 0f;
            }

            mat.color = Color.HSVToRGB(color, 1, 1);
        }
    }

    void ActiveSkin()
    {
        isSkinActive = true;
        initialSkin.SetActive(false);
        goldSkin.SetActive(true);
    }
}