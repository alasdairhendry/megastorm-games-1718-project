using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderBeam : MonoBehaviour {

    [SerializeField] private Transform target;
    private Transform targetFinder;
    private ParticleSystem endParticles;
    private LineRenderer lineRenderer;

    private Vector3 centreOfMass = new Vector3();

    [SerializeField] private bool active = false;

	// Use this for initialization
	void Start () {
        targetFinder = transform.Find("targetFinder");
        lineRenderer = GetComponent<LineRenderer>();
        //print("I HAVE SPAWNED");
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.singleton.IsPaused)
            return;

        if (!active)
            return;

        if (target == null)
            Destroy(gameObject);

        CalculateVertices();
        SetAverageVerts();
        CalculateMidPointLocal();
        //FollowTarget();
        SetParticles();

        if (target != null)
            centreOfMass = target.GetComponent<Rigidbody>().worldCenterOfMass;
    }

    private void CalculateVertices()
    {
        int Vn = 0;
        float dist = Vector3.Distance(this.transform.position, targetFinder.position);
        Vn = Mathf.FloorToInt((float)dist * 4.0f);
        if (Vn % 2 == 0)
            Vn += 1;

        if (Vn < 3)
            Vn = 3;

        lineRenderer.positionCount = Vn;
    }

    private void SetAverageVerts()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            //print(lr.positionCount);
            lineRenderer.SetPosition(i, Vector3.Lerp(this.transform.position, targetFinder.position, (float)i / (float)lineRenderer.positionCount));
        }
    }

    [SerializeField] bool p = false;
    private void CalculateMidPointLocal()
    {
        if (p)
            return;

        int mp = Mathf.CeilToInt(lineRenderer.positionCount / 2.0f);

        Vector3 mpV = lineRenderer.GetPosition(mp);

        mpV = new Vector3(lineRenderer.GetPosition(0).x, mpV.y, mpV.z);

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = (float)i / (float)lineRenderer.positionCount;
            Vector3 newPosition = Vector3.Lerp(Vector3.Lerp(lineRenderer.GetPosition(0), mpV, t), Vector3.Lerp(mpV, lineRenderer.GetPosition(lineRenderer.positionCount - 1), t), t);
            lineRenderer.SetPosition(i, newPosition);
        }
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target == null || targetFinder == null)
            Destroy(gameObject);

        targetFinder.transform.position = Vector3.Lerp(targetFinder.transform.position, centreOfMass, Time.deltaTime * 15.0f);
    }

    private void SetParticles()
    {
        transform.Find("EndParticle").transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
    }

    public void Play(Transform _target)
    {
        targetFinder = transform.Find("targetFinder");
        target = _target;
        active = true;
        //targetFinder.position = this.transform.position;
    }

    public void Resume()
    {
        active = true;
        //targetFinder.position = this.transform.position;
    }

    public void Stop()
    {
        active = false;
        lineRenderer.positionCount = 0;
        //print("I HAVE STOPPED");
    }

    private void OnDestroy()
    {
        //print("I HAVE BEEN DESTROYED");
    }
}
