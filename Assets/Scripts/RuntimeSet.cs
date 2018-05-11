// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
//
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject {
    public List<T> Items = new List<T>();
    public List<T> UnPicked = new List<T>();

    public void Add(T thing) {
        if (!Items.Contains(thing)) {
            Items.Add(thing);
            UnPicked.Add(thing);
        }
    }

    public void Remove(T thing) {
        if (Items.Contains(thing)) {
            Items.Remove(thing);
            UnPicked.Remove(thing);
        }
    }

    public T PickRandom() {
        if (UnPicked.Count == 0) {
            // reset UnPicked if we run out of items
            UnPicked = new List<T>(Items);
        }
        var index = Random.Range(0, UnPicked.Count);
        var thing = UnPicked[index];
        UnPicked.Remove(thing);
        return thing;
    }
}
