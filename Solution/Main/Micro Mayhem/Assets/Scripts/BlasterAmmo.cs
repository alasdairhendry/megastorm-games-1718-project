using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterAmmo : Bullet {

    [SerializeField] private GameObject particleEffect;

    public override void Init(Vector3 _initialVelocity, float _speed, float _damage)
    {
        initialVelocity = _initialVelocity;
        GetComponent<Rigidbody>().AddRelativeForce(_initialVelocity, ForceMode.Impulse);
        speed = _speed;
        damage = _damage;
    }

    protected override void Start()
    {        
        //transform.position += transform.InverseTransformDirection(initialVelocity) * Time.deltaTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameState.singleton.IsPaused)
            return;

        transform.position += transform.forward * Time.deltaTime * speed;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Character" || other.gameObject.name == "Blaster_PREF(Clone)")
        {
            return;
        }

        GameObject particle = Instantiate(particleEffect);
        particle.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
        Destroy(this.gameObject);        

        Rigidbody[] rigidbodies = GameObject.FindObjectsOfType<Rigidbody>();
        
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, 5.5f, Vector3.forward);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector3 direction = hit.collider.gameObject.transform.position - this.transform.position;
                float positionDeficit = Mathf.Lerp(1.0f, 0.0f, Vector3.Distance(this.transform.position, hit.collider.gameObject.transform.position) / 5.5f);

                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * positionDeficit * 5.0f, ForceMode.VelocityChange);

                if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
                {
                    hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage * positionDeficit);
                }
            }            
        }
    }
}
