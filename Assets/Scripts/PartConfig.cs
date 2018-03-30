using System;
using UnityEngine;

[System.Serializable]
public class PartConfigRow {
    public ConfigTag key;
    public int valueType;
    public bool boolValue;
    public string stringValue;
    public float floatValue;

    public PartConfigRow(ConfigTag key) {
        this.key = key;
    }
}

[System.Serializable]
public class PartConfig {
    public PartConfigRow[] rows;

    public PartConfig() {
        rows = new PartConfigRow[0];
    }

    public T Get<T>(ConfigTag key) {
        for (var i=0; i<rows.Length; i++) {
            if (rows[i].key == key) {
                if (typeof(T) == typeof(bool)) {
                    return (T)(object)(rows[i].boolValue);
                } else if (typeof(T) == typeof(string)) {
                    return (T)(object)(rows[i].stringValue);
                } else if (typeof(T) == typeof(float)) {
                    return (T)(object)(rows[i].floatValue);
                }
            }
        }
        return default(T);
    }

    public void Save<T>(ConfigTag key, T value) {
        // see if key already exists
        int index = 0;
        bool found = false;
        for (; index<rows.Length; index++) {
            if (rows[index].key == key) {
                found = true;
            }
        }

        // resize to add room if not found
        if (!found) {
            Array.Resize(ref rows, rows.Length+1);
            index = rows.Length-1;
            rows[index] = new PartConfigRow(key);
        }

        // set value based on type
        if (typeof(T) == typeof(bool)) {
            rows[index].valueType = 0;
            rows[index].boolValue = (bool)(object)value;
        } else if (typeof(T) == typeof(string)) {
            rows[index].valueType = 1;
            rows[index].stringValue = (string)(object)value;
        } else if (typeof(T) == typeof(float)) {
            rows[index].valueType = 2;
            rows[index].floatValue = (float)(object)value;
        }
    }

}
