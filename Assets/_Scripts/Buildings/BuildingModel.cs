using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "New/Building")]
public class BuildingModel : ScriptableObject
{
    // Public variables to hold the object's information
    public string objectName;
    public Sprite objectImage;

}
