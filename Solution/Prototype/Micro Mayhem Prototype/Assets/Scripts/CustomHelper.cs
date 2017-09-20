using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System;

public class CustomHelper {

    public static string GetDescription(Enum en)
    {
        Type type = en.GetType();

        MemberInfo[] memInfo = type.GetMember(en.ToString());

        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return en.ToString();
    }

    public static Color NewColor(int R, int G, int B)
    {
        Color _color;

        _color = new Color(R / 255.0f, G / 255.0f, B / 255.0f);

        return _color;
    }

    public static Color NewColor(int R, int G, int B, int A)
    {
        Color _color;

        _color = new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);

        return _color;
    }
}
