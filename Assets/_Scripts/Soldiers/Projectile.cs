using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 2f;

    private int projectileDamage = 2;
    private GameObject target;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (target != null){
            //If target is building find its center as a target
            if (target.GetComponent<Building>() != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.GetComponent<Building>().damageCenter.transform.position, .1f);

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, .1f);
            }

        }
        //When target dies destroy this projectile
        if (target != null&&target.GetComponent<HealthModel>().CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void SetProjectile(GameObject gameObject,int damage)
    {
        target = gameObject;
        projectileDamage = damage;
    }

    //When this projectile triggers target object. Reduce its health.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            other.GetComponent<HealthController>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
