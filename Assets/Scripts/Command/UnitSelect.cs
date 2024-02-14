using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Unit curUnit; //current selected single unit
    public Unit CurUnit { get { return curUnit; } }

    private Camera cam;
    private Faction faction;

    public static UnitSelect instance;
    
    void Awake()
    {
        faction = GetComponent<Faction>();
    }

    void Start()
    {
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Unit", "Building", "Resource", "Ground");

        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        //mouse down
                if (Input.GetMouseButtonDown(0))
                {
                    ClearEverything();
                }
        
                // mouse up
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("1");
                    TrySelect(Input.mousePosition);
                }

    }
    
    private void SelectUnit(RaycastHit hit)
    {
   
        curUnit = hit.collider.GetComponent<Unit>();

        curUnit.ToggleSelectionVisual(true);

        Debug.Log("Selected Unit");

        if (GameManager.instance.MyFaction.IsMyUnit(curUnit))
        {
            ShowUnit(curUnit);
        }
    }
    
    private void TrySelect(Vector2 screenPos)
    {
        Debug.Log("2");
        
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        //if we left-click something
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Debug.Log("3");
            
            switch (hit.collider.tag)
            {
                case "Unit":
                    SelectUnit(hit);
                    break;
            }
        }
    }
    
    private void ClearAllSelectionVisual()
    {
        if (curUnit != null)
            curUnit.ToggleSelectionVisual(false);
    }
    private void ClearEverything()
    {
        ClearAllSelectionVisual();
        curUnit = null;
        
        //clear UI
        InfoManager.instance.ClearAllInfo();
    }

    private void ShowUnit(Unit u)
    {
        InfoManager.instance.ShowAllInfo(u);
    }


}

