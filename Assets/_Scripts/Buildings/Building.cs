using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] BuildingModel buildingModel;
    public bool Placed { get; private set;}
    public BoundsInt area;

    [Header("Health System")]
    public float maxHealth = 50;
    public HealthView healthView;

    private HealthModel healthModel;
    private HealthController healthController;

    public GameObject damageCenter;

    private void Start()
    {
        healthModel = gameObject.AddComponent<HealthModel>();
        healthModel.SetHealthModel(maxHealth, maxHealth);

        healthController = gameObject.AddComponent<HealthController>();
        healthController.SetHealthController(healthModel, healthView);

        healthView.UpdateHealthBar(healthModel.CurrentHealth, healthModel.MaxHealth);
    }
    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GetComponent<Collider2D>().enabled = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
    }

    private void OnMouseDown()
    {
        if (Placed && !GridBuildingSystem.current.IsHoldingBuilding())
        {
            ShowInfo();
        }
    }

    // Display building icon and namein the UI
    public virtual void ShowInfo()
    {
        //Send basic building infos
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        GridBuildingSystem.current.SelectArea(areaTemp);

        InformationPanel.Instance.ShowBuildingInfo(buildingModel.objectName,buildingModel.objectImage,this);
    }

    public void ResetSelectedTiles()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        GridBuildingSystem.current.DeselectArea(areaTemp);
    }
}
