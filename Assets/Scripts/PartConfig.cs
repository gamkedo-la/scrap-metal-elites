using System;
using UnityEngine;

[System.Serializable]
public class PartConfigRow {
    public ConfigTag key;
    public object value;
    public PartConfigRow(
        ConfigTag key,
        object value
    ) {
        this.key = key;
        this.value = value;
    }
}

[System.Serializable]
public class PartConfig {
    PartConfigRow[] rows;

    public PartConfig() {
        rows = new PartConfigRow[0];
    }

    public T Get<T>(ConfigTag key) {
        object result = null;
        for (var i=0; i<rows.Length; i++) {
            if (rows[i].key == key) {
                result = rows[i].value;
            }
        }
        if (result == null) {
            return default(T);
        } else {
            return (T)result;
        }
    }

    public void Save<T>(ConfigTag key, T value) {
        // see if key already exists
        for (var i=0; i<rows.Length; i++) {
            if (rows[i].key == key) {
                rows[i].value = value;
                return;
            }
        }

        // otherwise... add to end
        Array.Resize(ref rows, rows.Length+1);
        rows[rows.Length-1] = new PartConfigRow(key, value);
    }

}
