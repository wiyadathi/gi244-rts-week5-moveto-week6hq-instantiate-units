using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Structure
{
    [SerializeField] private Transform spawnPoint;
    public Transform SpawnPoint { get { return spawnPoint; } }
    [SerializeField] private Transform rallyPoint;
    public Transform RallyPoint { get { return rallyPoint; } }

    [SerializeField] private GameObject[] unitPrefabs;

    [SerializeField] private List<Unit> recruitList = new List<Unit>();

    [SerializeField] private float unitTimer = 0f;
    [SerializeField] private int curUnitProgress = 0;

    [SerializeField] private float curUnitWaitTime = 0f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ToCreateUnit(0);

        if ((recruitList.Count > 0) && (recruitList[0] != null))
        {
            unitTimer += Time.deltaTime;
            curUnitWaitTime = recruitList[0].UnitWaitTime;

            if (unitTimer >= curUnitWaitTime)
            {
                curUnitProgress++;
                unitTimer = 0f;

                if (curUnitProgress >= 100)
                {
                    curUnitProgress = 0;
                    curUnitWaitTime = 0f;
                    CreateUnitCompleted();
                }
            }
        }

    }
    
    public void ToCreateUnit(int i)
    {
        Debug.Log(structureName + " creates " + i + ":" + unitPrefabs.Length);
        if (unitPrefabs.Length == 0)
            return;

        if (unitPrefabs[i] == null)
            return;

        Unit unit = unitPrefabs[i].GetComponent<Unit>();

        if (unit == null) 
            return;

        if (!faction.CheckUnitCost(unit)) //not enough resources
            return;

        //Deduct Resource
        faction.DeductUnitCost(unit);

        //If it's me, update UI
        if (faction == GameManager.instance.MyFaction)
            MainUI.instance.UpdateAllResource(faction);

        //Add unit into faction's recruit list
        recruitList.Add(unit);

        Debug.Log("Adding" + i + "to Recruit List");
    }

    public void CreateUnitCompleted()
    {
        int id = recruitList[0].ID;

        if (unitPrefabs[id] == null)
            return;

        GameObject unitObj = Instantiate(unitPrefabs[id], spawnPoint.position, Quaternion.Euler(0f, 180f, 0f));

        recruitList.RemoveAt(0);

        Unit unit = unitObj.GetComponent<Unit>();
        unit.MoveToPosition(rallyPoint.position); //Go to Rally Point

        //Add unit into faction's Army
        faction.AliveUnits.Add(unit);

        Debug.Log("Unit Recruited");
        //If it's me, update UI
        if (faction == GameManager.instance.MyFaction)
            MainUI.instance.UpdateAllResource(faction);
    }
    
    public void ToggleSelectionVisual(bool flag)
    {
        if (SelectionVisual != null)
            SelectionVisual.SetActive(flag);
    }


}
