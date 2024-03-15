using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomJson : MonoBehaviour
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }
    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = list;
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    public class Wrapper<T>
    {
        public List<T> items;
    }
}

