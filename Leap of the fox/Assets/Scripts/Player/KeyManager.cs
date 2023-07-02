using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private List<Key> keys = new List<Key>();


    public void AddKey(Key key)
    {
        keys.Add(key);
    }

    public void RemoveKey(Key key)
    {
        keys.Remove(key);
    }

    public void RemoveAllKeys()
    {
        keys = new List<Key>();
    }

    public Key GetKey(ColorEnum color)
    {
        foreach (Key key in keys)
        {
            if (key.color == color)
                return key;
        }
        return null;
    }


}
