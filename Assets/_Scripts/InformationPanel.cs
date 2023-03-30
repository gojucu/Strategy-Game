using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InformationPanel : MonoBehaviour
{
    public static InformationPanel Instance;

    // Private variables for UI elements
    public GameObject infoArea;

    public GameObject productionListArea;

    [Header("ScrollView")]
    public ObjectPool objectPool;
    public GameObject productionListContent;

    public TextMeshProUGUI nameText;
    public Image buildingIcon;

    public bool isShowing;
    public Building selectedBuilding;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        //If there is a selected building and we click right mouse button stop focusing on that building
        if (isShowing && selectedBuilding != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ClearInfo();
            }
        }
    }
    //Show the selected building infos
    public void ShowBuildingInfo(string objectName, Sprite objectImage,Building building)
    {
        //Reset old info
        ClearInfo();

        nameText.text = objectName;
        buildingIcon.sprite = objectImage;
        infoArea.SetActive(true);
        isShowing = true;
        selectedBuilding = building;
    }

    // List all the units we can produce in the panel
    public void ListProducts(List<Soldier> soldiers)
    {
        productionListArea.SetActive(true);
        //Show unit list on panel
        foreach (Soldier soldier in soldiers)
        {
            GameObject go = objectPool.GetObject();
            go.GetComponentInChildren<TextMeshProUGUI>().text = soldier.soldierConfig.GetName();
            go.transform.Find("Icon").GetComponent<Image>().sprite = soldier.soldierConfig.GetSprite();
            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(()=>ProduceUnit(soldier));

        }
    }

    public void ClearInfo()
    {
        nameText.text = "";
        buildingIcon.sprite = null;
        objectPool.ReturnAll();
        infoArea.SetActive(false);
        productionListArea.SetActive(false);
        isShowing = false;
        if (selectedBuilding != null)
        {
            //Reset selected tiles colors in the grid system
            selectedBuilding.ResetSelectedTiles();
            //If building is barracks close flag
            if (selectedBuilding.GetComponent<Barracks>() != null && selectedBuilding.GetComponent<Barracks>().flag != null)
            {
                selectedBuilding.GetComponent<Barracks>().flag.SetActive(false);

            }
        }
        selectedBuilding = null;

    }

    public void ProduceUnit(Soldier soldier)
    {
        selectedBuilding.GetComponent<Barracks>().CreateUnit(soldier.soldierConfig.GetName());
    }
}
