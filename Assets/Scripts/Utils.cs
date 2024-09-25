using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
    
public static class Utils
{
    public static T GetRandom<T>(this IEnumerable<T> elems)
    {
        if (elems.Count() == 0) throw new ArgumentException($"{elems} : Container empty");
        return elems.ElementAt(new System.Random().Next(0, elems.Count()));
    }
}