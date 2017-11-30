using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the "Lines" that are displayed between The Cleanser and it's targets
/// </summary>
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
        SetParticles();

        if (target != null)
            centreOfMass = target.GetComponent<Rigidbody>().worldCenterOfMass;
    }

    // Calculate how many vertices the line renderer should have
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

    // Set the position of the verts in a linear path, from the origin to the target
    private void SetAverageVerts()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {            
            lineRenderer.SetPosition(i, Vector3.Lerp(this.transform.position, targetFinder.position, (float)i / (float)lineRenderer.positionCount));
        }
    }

    [SerializeField] bool p = false;    // I believe this was used to debug, not sure.. so gonna leave it here in case it completely breaks the game

    // Calculate the mid point, from the origin to the target using the local forward vector to give a bezier curve to the line
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

    // Follow the target
    private void FollowTarget()
    {
        if (target == null || targetFinder == null)
            Destroy(gameObject);

        targetFinder.transform.position = Vector3.Lerp(targetFinder.transform.position, centreOfMass, Time.deltaTime * 15.0f);
    }

    // Maintain the position of the particles at the end of the line
    private void SetParticles()
    {
        transform.Find("EndParticle").transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
    }

    // Play the line renderer
    public void Play(Transform _target)
    {
        targetFinder = transform.Find("targetFinder");
        target = _target;
        active = true;
        //targetFinder.position = this.transform.position;
    }

    // Resume the line renderer
    public void Resume()
    {
        active = true;
        //targetFinder.position = this.transform.position;
    }

    // Stop the line renderer
    public void Stop()
    {
        active = false;
        lineRenderer.positionCount = 0;        
    }   
}
