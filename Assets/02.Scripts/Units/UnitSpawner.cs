using UnityEngine;

public class UnitSpawner : MonoBehaviour {

    public GameObject unitPrefab;

    public void SpawnNewUnit()
    {
        if (unitPrefab == null) return;
        var MB = MarkersManager.inst.GetMainBase();
        if (MB == null) return;
        if(MarkersManager.inst.CanCreateNewSoldier())
        {
            var u = Instantiate(unitPrefab, MB.position, MB.rotation, transform).GetComponent<Unit>();
            if(u == null)
            {
                Destroy(u);
                return;
            }
            MarkersManager.inst.AddNewUnit(u);            
        }
        else
            DPanel.Add(this, "New UNIT <color=red>can't be created</color>");
    }
}
