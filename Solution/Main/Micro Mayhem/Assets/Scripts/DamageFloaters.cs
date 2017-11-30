using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Management class that helps us to spawn "Damage Floaters" to display the damage done to an object
/// </summary>
public class DamageFloaters : MonoBehaviour {

    public static DamageFloaters singleton;

    [SerializeField] private GameObject floater;
    private List<FloaterTarget> targets = new List<FloaterTarget>();

    // Construct a singleton pattern
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    // Create a new floater
    public void AddFloater(string text, Color colour, Transform target, Vector3 offset, float lifetime)
    {
        // Create an empty target
        FloaterTarget _target;

        // Check if we already have a container for this object, and if not, create a new one 
        if(CheckTargetExists(target.gameObject) == null)
        {
            // Create a new target, assign it to our target variable, and add it to our records
            _target = new FloaterTarget(target.gameObject);
            targets.Add(_target);                           
        }
        else
        {            
            // The target already exists in our records, so assign it.
            _target = CheckTargetExists(target.gameObject);
        }

        GameObject _floater = Instantiate(floater);

        _floater.transform.parent = target;
        _floater.transform.position = offset + _target.SetArea(_floater);

        float damage = float.Parse(text);
        Vector3 scale = Vector3.Lerp(new Vector3(0.4f, 0.4f, 0.4f), new Vector3(1.2f, 1.2f, 1.2f), damage / 100.0f);
        _floater.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

        _floater.GetComponentInChildren<TextMesh>().text = text;
        _floater.GetComponentInChildren<TextMesh>().transform.Find("Shadow").gameObject.GetComponent<Renderer>().material.color = Color.white;

        _floater.GetComponent<Floater>().Init(() => { /*Debug.Log("Killed Floater");*/ }, lifetime);
    }

    // Create a new floater without creating new targets, good for one offs
    public void AddFloater(string text, Color colour, Transform target, float lifetime)
    {       
        GameObject _floater = Instantiate(floater);

        _floater.transform.position = target.transform.position;
        _floater.transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);

        _floater.GetComponentInChildren<TextMesh>().text = text;
        _floater.GetComponentInChildren<TextMesh>().transform.Find("Shadow").gameObject.GetComponent<Renderer>().material.color = Color.white;

        _floater.GetComponent<Floater>().Init(() => { }, lifetime);
    }

    // Check if we already have a given target in our records
    private FloaterTarget CheckTargetExists(GameObject _target)
    {
        foreach (FloaterTarget target in targets)
        {
            if(target.obj == _target)
            {
                return target;
            }
        }

        // We found no targets in the list, return null
        return null;
    }

    // Destroy the given floater
    public void DestroyFloater(GameObject _floater)
    {
        Destroy(_floater);
    }
}

/// <summary>
/// Allows us to keep track of our Floaters and spawn them efficiently and smartly.
/// -- This means that if the Object takes damage multiple times in a few seconds, the floaters are not covering each other up
/// </summary>
public class FloaterTarget
{
    public GameObject obj;                                      // The object the floater is for
    List<Vector3> areas = new List<Vector3>();                  // The areas that a floater can appear, in respect to the parent Object
    List<GameObject> floaters = new List<GameObject>();         // The current floaters that are owned by this Object
    public int floaterCount = 0;                                // The amount of floaters we current have

    // Generic constructor, which assigns 5 areas: the origin & 4 areas around the origin where floaters can spawn
    public FloaterTarget(GameObject _origin)
    {
        obj = _origin;
        areas.Add(Vector3.zero);
        areas.Add(new Vector3(1, 0, 0));
        areas.Add(new Vector3(0, 1, 0));
        areas.Add(new Vector3(-1, 0, 0));
        areas.Add(new Vector3(0, -1, 0));
    }

    // Returns the appropriate position for the intended Floater to spawn
    public Vector3 SetArea(GameObject _floater)
    {        
        Vector3 position = areas[floaterCount];

        if(floaters.Count <= floaterCount)
            floaters.Add(_floater);
        else
        {
            DamageFloaters.singleton.DestroyFloater(floaters[floaterCount]);
            floaters[floaterCount] = _floater;
        }

        floaterCount++;

        if (floaterCount >= areas.Count)
            floaterCount = 0;

        return position + obj.transform.position;
    }
}
