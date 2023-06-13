using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TypeHelpers {
    public static StringBuilder SerializeFieldLabels<T>() where T : new() {
        string[] labels = (new T()).GetType().GetFields().Select(label => {
            return string.Concat(label.Name.Select((x, i) => {
                return i > 0 && char.IsUpper(x) ? $"_{x.ToString().ToLower()}" : x.ToString().ToLower();
            }));
        }).ToArray();
        return StringHelpers.ConvertToRow(labels);
    }
    public static StringBuilder SerializeFields<T>(T instance) {
        string[] fields = instance.GetType().GetFields().Select(x => x.GetValue(instance).ToString()).ToArray();
        return StringHelpers.ConvertToRow(fields);
    }
}
public static class StringHelpers {
    public static StringBuilder ConvertToRow(params object[] row) {
        string[] row_string = row.Select(x => $"{x:G}").ToArray();
        return ConvertToRow(row_string);
    }
    public static StringBuilder ConvertToRow(params float[] row) {
        string[] row_string = row.Select(x => $"{x:G}").ToArray();
        return ConvertToRow(row_string);
    }
    public static StringBuilder ConvertToRow(params string[] row) {
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Join(",", row));
        return sb;
    }
}
