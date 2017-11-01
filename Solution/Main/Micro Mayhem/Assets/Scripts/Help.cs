﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Help
{
    public static Vector3 ReturnToZero (this Vector3 value, float damping)
    {
        List<float> values = new List<float>() { value.x, value.y, value.z };

        float factor = damping * Time.deltaTime;

        for (int i = 0; i < values.Count; i++)
        {
            Debug.Log(i);
            if (values[i] < 0)
            {
                if (values[i] + factor >= 0)
                    values[i] = 0;
                else
                    values[i] += factor;
            }
            else if (values[i] > 0)
            {
                if (values[i] - factor <= 0)
                    values[i] = 0;
                else
                    values[i] -= factor;
            }
        }

        return new Vector3(values[0], values[1], values[2]);
    }
}
