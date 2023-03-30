using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Soldier : MonoBehaviour
{
    public SoldierConfig soldierConfig;

    [SerializeField] GameObject selectedGameObject; 

    [Header("Health System")]
    public float maxHealth = 10;
    public HealthView healthView;

    private HealthModel healthModel;
    private HealthController healthController;
    [Header("Attack System")]

    public float attackDistance = 10f;
    public float moveSpeed = 5f;
    public GameObject projectilePrefab;

    private GameObject target;
    private bool canAttack = true;

    private void Start()
    {
        healthModel = gameObject.AddComponent<HealthModel>();
        healthModel.SetHealthModel(maxHealth,maxHealth);

        healthController = gameObject.AddComponent<HealthController>();
        healthController.SetHealthController(healthModel, healthView);

        healthView.UpdateHealthBar(healthModel.CurrentHealth, healthModel.MaxHealth);

        
    }
    private void Update()
    {
        if (target == null)
        {
            return;
        }


        float distance = Vector3.Distance(transform.position, target.transform.position);

        //If target in attack distance attack target else move close to target
        if (distance <= attackDistance)
        {
            GetComponent<AIDestinationSetter>().target = transform.position;
            StartCoroutine(Attack());
        }
        else
        {
            GetComponent<AIDestinationSetter>().target = target.transform.position;
        }
    }

    //Set target to attack
    public void SetAttackTarget(GameObject gameObject)
    {
        if (gameObject != this.gameObject)
        {
            target = gameObject;

            if (gameObject.GetComponent<Building>() != null)
            {
                MoveTo(gameObject.GetComponent<Building>().damageCenter.transform.position);
            }
            else
            {
                MoveTo(gameObject.transform.position);
            }
        }

    }
    public void ClearTarget()
    {
        target=null;
    }
    // This is a coroutine function that allows for the Attack() method to be executed over time.
    IEnumerator Attack()
    {
        
        while (target!=null && canAttack)
        {
            canAttack = false;
            //perform attack
            FireProjectile();
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }
    }
    // This method is responsible for firing a projectile at a target.
    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetProjectile(target, soldierConfig.GetDamage());
    }

    //When this unit selected activate selectedGameobject to show this unit is selected.
    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        GetComponent<AIDestinationSetter>().target = targetPosition;
    }

    //Show attack distance
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
