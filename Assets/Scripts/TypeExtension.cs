using System;
using UnityEngine;

public static class TypeExtension {
    public static bool ToBool(this string value)
    {
        bool ret = false;
        if (!string.IsNullOrEmpty(value))
        {
            if (value.ToLower() == "true" || value.ToLower() == "t" || value == "1")
            {
                ret = true;
            }
            else if (value.ToLower() == "false" || value.ToLower() == "f" || value == "0")
            {
                ret = false;
            }
        }
        return ret;
    }

    public static Byte ToByte(this string value)
    {
        Byte ret = 0;
        if (!string.IsNullOrEmpty(value))
        {
            ret = Byte.Parse(value);
        }
        else
        {
            ret = 0;
        }
        return ret;
    }

    public static int ToInt(this string value)
    {
        int ret = 0;
        if (!string.IsNullOrEmpty(value))
        {
            ret = int.Parse(value);
        }
        else
        {
            ret = 0;
        }
        return ret;
    }

    public static float ToFloat(this string value)
    {
        float ret = 0f;
        if (!string.IsNullOrEmpty(value))
        {
            ret = float.Parse(value);
        }
        else
        {
            ret = 0f;
        }
        return ret;
    }

    public static T ToEnum<T>(this string value, T defaultValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static void Init(this Component value)
    {
        value.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
    }

}
