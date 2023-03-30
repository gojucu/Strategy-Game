using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private HealthModel healthModel;
    private HealthView healthView;



    public void TakeDamage(int damage)
    {
        healthModel.CurrentHealth -= damage;
        healthView.UpdateHealthBar(healthModel.CurrentHealth,healthModel.MaxHealth);
        if (healthModel.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GetComponent<Building>() != null)
        {
            //Clear the grid area
            Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = GetComponent<Building>().area;
            areaTemp.position = positionInt;
            GetComponent<Collider2D>().enabled = false;
            FindObjectOfType<AstarPath>().Scan();
            GridBuildingSystem.current.DestroyArea(areaTemp);
        }
        //Kill unit
        if (GetComponent<Soldier>() != null)
        {
            if (RtsController.Instance.IsUnitSelected(GetComponent<Soldier>()))
            {
                RtsController.Instance.RemoveFromSelected(GetComponent<Soldier>());
            }
        }
        Destroy(gameObject,.15f);
    }

    public void SetHealthController(HealthModel model, HealthView view)
    {
        this.healthModel = model;
        this.healthView = view;
    }

}
