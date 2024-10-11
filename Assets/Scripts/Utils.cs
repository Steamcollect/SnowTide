﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public static class Utils
{
    public static T GetRandom<T>(this IEnumerable<T> elems)
    {
        if (elems.Count() == 0) throw new ArgumentException($"{elems} : Container empty");
        return elems.ElementAt(new System.Random().Next(0, elems.Count()));
    }

    public static void BumpVisual(this Transform t)
    {
        t.DOKill();
        t.DOScale(1.1f, .06f).OnComplete(() =>
        {
            t.DOScale(1, .08f);
        });
    }

    public static Vector3 Clamp(Vector3 current, Vector3 min, Vector3 max)
    {
        return current;
    }
}