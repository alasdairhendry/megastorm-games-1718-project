using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple Level Creator.
/// -Stores a list of props that can be used and allows you to place them in a grid
/// </summary>
public class LevelGenerator : MonoBehaviour {

    [Header ("Definitions")]
    [Range (10,100)]
    public int levelDimensions;

    [Range(0.1f, 10.0f)]
    public float tileDimensions;

    public List<GameObject> levelObjects = new List<GameObject>();
    public GameObject targetParent;

    public bool drawHandles = true;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(levelDimensions, 1, levelDimensions));
    }
}
