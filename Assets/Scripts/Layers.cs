﻿using UnityEngine;

public static class Layers {
    public static int Default = LayerMask.NameToLayer("Default");
    public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");
    public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    public static int Water = LayerMask.NameToLayer("Water");
    public static int UI = LayerMask.NameToLayer("UI");
    public static int PostProcess = LayerMask.NameToLayer("PostProcess");
    public static int World = LayerMask.NameToLayer("World");

    public static int GetMask(params int[] layers) {
        int result = 0;
        for (int i = 0; i < layers.Length; i++) {
            result |= 1 << layers[i];
        }

        return result;
    }
}
