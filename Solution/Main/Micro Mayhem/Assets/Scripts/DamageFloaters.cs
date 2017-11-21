using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloaters : MonoBehaviour {

    public static DamageFloaters singleton;

    [SerializeField] private GameObject floater;
    private List<FloaterTarget> targets = new List<FloaterTarget>();

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    public void AddFloater(string text, Color colour, Transform target, Vector3 offset, float lifetime)
    {
        FloaterTarget _target;

        if(CheckTargetExists(target.gameObject) == null)
        {
            _target = new FloaterTarget(target.gameObject);
            targets.Add(_target);               
            //print("Created new Target");
        }
        else
        {
            //print("Target Found");
            _target = CheckTargetExists(target.gameObject);
        }

        GameObject _floater = Instantiate(floater);

        _floater.transform.parent = target;
        _floater.transform.position = offset + _target.SetArea(_floater);
        //_floater.transform.localPosition = Vector3.zero;
        //_floater.transform.position += offset;
        float damage = float.Parse(text);
        Vector3 scale = Vector3.Lerp(new Vector3(0.4f, 0.4f, 0.4f), new Vector3(1.2f, 1.2f, 1.2f), damage / 100.0f);
        _floater.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

        _floater.GetComponentInChildren<TextMesh>().text = text;
        _floater.GetComponentInChildren<TextMesh>().transform.Find("Shadow").gameObject.GetComponent<Renderer>().material.color = Color.white;

        _floater.GetComponent<Floater>().Init(() => { /*Debug.Log("Killed Floater");*/ }, lifetime);
    }

    public void AddFloater(string text, Color colour, Transform target, float lifetime)
    {       
        GameObject _floater = Instantiate(floater);

        _floater.transform.position = target.transform.position;
        _floater.transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);

        _floater.GetComponentInChildren<TextMesh>().text = text;
        _floater.GetComponentInChildren<TextMesh>().transform.Find("Shadow").gameObject.GetComponent<Renderer>().material.color = Color.white;

        _floater.GetComponent<Floater>().Init(() => { }, lifetime);
    }

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

    public void DestroyFloater(GameObject _floater)
    {
        Destroy(_floater);
    }
}

public class FloaterTarget
{
    public GameObject obj;
    List<Vector3> areas = new List<Vector3>();
    List<GameObject> floaters = new List<GameObject>();
    public int floaterCount = 0;

    public FloaterTarget(GameObject _origin)
    {
        obj = _origin;
        areas.Add(Vector3.zero);
        areas.Add(new Vector3(1, 0, 0));
        areas.Add(new Vector3(0, 1, 0));
        areas.Add(new Vector3(-1, 0, 0));
        areas.Add(new Vector3(0, -1, 0));
    }

    public Vector3 SetArea(GameObject _floater)
    {        
        Vector3 position = areas[floaterCount];

        //Debug.Log("Set Position");
        //Debug.Log("Floater Counter = " + floaterCount.ToString());

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
