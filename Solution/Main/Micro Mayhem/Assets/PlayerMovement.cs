using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator animator;

    [SerializeField] private LayerMask rotationRayMask;
    [SerializeField] private GameObject upperRotationalBone;
    [SerializeField] private float movementSpeed = 15.0f;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        LowerBodyMovement();
        UpperBodyMovement();
	}

    private void LowerBodyMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horizontalInput, 0.0f, verticalInput).normalized * movementSpeed * Time.deltaTime;

        animator.SetFloat("ForwardMotion", verticalInput);
        animator.SetFloat("SidewardMotion", horizontalInput);
    }

    private void UpperBodyMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 500.0f, rotationRayMask))
        {
            float angle = Mathf.Atan2(hit.point.z - transform.position.z, hit.point.x - transform.position.x) * Mathf.Rad2Deg - 90;
            if (angle < 0)
                angle += 360;
            print(angle + " - " + hit.point);

            upperRotationalBone.transform.eulerAngles = new Vector3(upperRotationalBone.transform.eulerAngles.x, -angle, upperRotationalBone.transform.eulerAngles.z);
        }
    }
}
