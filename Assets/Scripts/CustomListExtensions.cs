using UnityEngine;
using System.Collections.Generic;

public static class CustomListExtensions {
    public static List<T> Shuffle<T>(this List<T> list) {
        List<T> newList = new List<T>(list);
        int n = newList.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            T value = newList[k];
            newList[k] = newList[n];
            newList[n] = value;
        }
        return newList;
    }
}
