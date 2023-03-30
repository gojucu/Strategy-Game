using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoldierData", menuName = "New/Soldier")]
public class SoldierConfig : ScriptableObject
{
    // Public variables to hold the object's information
    [SerializeField]
    private string objectName;
    [SerializeField]
    private Sprite objectImage;
    [SerializeField]
    private int damage = 0;

    public string GetName()
    {
        return objectName;
    }
    public Sprite GetSprite()
    {
        return objectImage;
    }
    public int GetDamage()
    {
        return damage;
    }
}
