using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerListComparer
{
    public static bool AreEqual(List<KeyValuePair<string, int>> list1, List<KeyValuePair<string, int>> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        var set1 = new HashSet<KeyValuePair<string, int>>(list1, new KeyValuePairComparer());
        var set2 = new HashSet<KeyValuePair<string, int>>(list2, new KeyValuePairComparer());

        return set1.SetEquals(set2);
    }
}


public class KeyValuePairComparer : IEqualityComparer<KeyValuePair<string, int>>
{
    public bool Equals(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
    {
        return x.Key == y.Key && x.Value == y.Value;
    }

    public int GetHashCode(KeyValuePair<string, int> obj)
    {
        int hashKey = obj.Key != null ? obj.Key.GetHashCode() : 0;
        int hashValue = obj.Value.GetHashCode();
        return hashKey ^ hashValue;
    }
}