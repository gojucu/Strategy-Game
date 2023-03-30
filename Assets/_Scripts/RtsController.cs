using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsController : MonoBehaviour
{
    public static RtsController Instance;
    [SerializeField] private Transform selectionAreaTransform;

    private Vector3 startPosition;
    private List<Soldier> selectedUnitList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        selectedUnitList = new List<Soldier>();
        selectionAreaTransform.gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPosition.z = 0f;
        }


        if (Input.GetMouseButton(0))
        {
            // Left Mouse Button Held Down
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);

            Vector3 worldPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, worldPos);

            // Deselect all Units
            foreach (Soldier unitRTS in selectedUnitList)
            {
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(false);
                }
            }
            selectedUnitList.Clear();

            // Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray)
            {
                Soldier unitRTS = collider2D.GetComponent<Soldier>();
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitList.Add(unitRTS);
                }
            }

        }


        if (Input.GetMouseButtonDown(1))
        {
            // Right Mouse Button Pressed
            Vector3 moveToPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            RaycastHit2D hit = Physics2D.Raycast(moveToPosition, Vector2.zero);
            HealthController healthController = null;

            if (hit.collider != null)
            {
                healthController = hit.collider.GetComponent<HealthController>();
            }


            foreach (Soldier soldier in selectedUnitList)
            {

                if (healthController != null)
                {
                    soldier.SetAttackTarget(healthController.gameObject);
                }
                else
                {
                    soldier.ClearTarget();
                    soldier.MoveTo(moveToPosition);
                }

            }
        }
    }

    public void RemoveFromSelected(Soldier soldier)
    {
        selectedUnitList.Remove(soldier);
    }
    //Returns the value for If the target soldier is selected with mouse 
    public bool IsUnitSelected(Soldier soldier)
    {
        if (selectedUnitList.Contains(soldier))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
