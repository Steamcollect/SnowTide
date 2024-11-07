using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public static class Utils
{
    public static T GetRandom<T>(this IEnumerable<T> elems)
    {
        if (elems.Count() == 0) return default;
        return elems.ElementAt(new System.Random().Next(0, elems.Count()));
    }

    public static void BumpVisual(this Transform t, float  endValue = 1.1f, float duration = 0.06f)
    {
        t.DOKill();
        t.DOScale(endValue, duration).OnComplete(() =>
        {
            t.DOScale(1, duration + 0.02f);
        });
    }
    public static void BumpFieldOfView(this Camera cam)
    {
        cam.DOKill();
        cam.DOFieldOfView(55, .06f).OnComplete(() =>
        {
            cam.DOFieldOfView(60, .08f);
        });
    }

    public static IEnumerator Delay(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    } 
    
}