using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFactory :MonoBehaviour
{
    [SerializeField]
    private GameObject ranger;
    [SerializeField]
    private GameObject at4;
    [SerializeField]
    private GameObject sniper;

    // This method creates a soldier GameObject based on the input soldier type string.
    public GameObject CreateSoldier(string soldierType)
    {
        if (soldierType.Equals("Ranger"))
        {
            return Instantiate(ranger);
        }
        else if (soldierType.Equals("AT-4 Soldier"))
        {
            return Instantiate(at4);
        }
        else if (soldierType.Equals("Sniper"))
        {
            return Instantiate(sniper);
        }
        else
        {
            return null;
        }
    }
}
