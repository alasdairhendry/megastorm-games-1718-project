using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unused.
/// </summary>
public class TargetLikeness : MonoBehaviour {

    public static TargetLikeness singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    
}
