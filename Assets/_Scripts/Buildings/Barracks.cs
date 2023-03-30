using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building
{
    public List<Soldier> producedUnits = new List<Soldier>();

    public GameObject spawnPosition;
    
    public GameObject flag;

    public override void ShowInfo()
    {
        // Send basic building infos
        base.ShowInfo();
        // Send produced unit infos to to panel
        InformationPanel.Instance.ListProducts(producedUnits);

        //Default target for created units
        flag.SetActive(true);
    }

    public void CreateUnit(string soldierName)
    {
        GameObject soldier = GetComponent<SoldierFactory>().CreateSoldier(soldierName);
        soldier.transform.position = spawnPosition.transform.position;

        soldier.GetComponent<Soldier>().MoveTo(flag.transform.position);
    }
}
