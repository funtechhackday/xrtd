using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkersManager : MonoBehaviour {

    #region Singleton
    public static MarkersManager inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
    }
    #endregion

    public Marker mainBase;
    public List<Marker> markers = new List<Marker>();
    public List<Unit> units = new List<Unit>();
    public List<Unit> enemys = new List<Unit>();

    public List<MatchMarkersUnits> matchs = new List<MatchMarkersUnits>();

    public void MarkerLost(Marker marker)
    {
        if(marker.Type == Marker.MarkerType.MainBase)
        {
            mainBase = null;
            EnemySpawner.inst.StopSpawn();            
            return;
        }
        if (matchs.Count > 0)
        {
            foreach (var m in matchs)
            {
                m.Remove(marker);
            }
        }
    }
    public void MarkerTracked(Marker marker)
    {
        if (marker.Type == Marker.MarkerType.MainBase)
        {
            mainBase = marker;
        }
        else
        {
            if (matchs.Count > 0)
            {
                foreach (var m in matchs)
                {

                    if (m.isLackMarker())
                    {
                        m.Set(marker);
                        DPanel.Add(this, string.Format("New marker type <color=yellow>[{0}]</color> was <color=red>replace</color>", marker.Type));
                        return;
                    }
                }
            }
            matchs.Add(new MatchMarkersUnits(marker));
            DPanel.Add(this, string.Format("New marker type <color=yellow>[{0}]</color> was <color=green>added</color>", marker.Type));
        }
        //DPanel.Add(this, string.Format("New marker type <color=yellow>[{0}]</color> was <color=red>ADDED</color> to array with ID <color=blue>[{1}]</color>", marker.Type, marker.ID));
    }

    public bool CanCreateNewSoldier()
    {
        if (matchs.Count > 0)
            foreach (var m in matchs)
                if (m.isLackUnit() && m.marker != null && m.marker.istracked)
                    return true;
        return false;
    }

    public Transform GetMainBase()
    {
        return mainBase == null ? null : mainBase.MarkerPosition;
    }

    public Transform GetUnitTargetTransform(Unit unit)
    {
        if (matchs.Count > 0)
            foreach (var m in matchs)
                if (m.Get(unit) != null)
                    return m.Get(unit).MarkerPosition;
        return null;
    }
    public Marker GetFreeMarker(Unit unit)
    {
        if(matchs.Count>0)
        foreach (var m in matchs)
            if (m.isLackUnit())
                return m.Set(unit);
        return null;
    }
    public void AddNewUnit(Unit unit)
    {
        if(CanCreateNewSoldier())
        {
            unit.Target = GetFreeMarker(unit).MarkerPosition;
            units.Add(unit);
            DPanel.Add(this, string.Format("New UNIT type <color=yellow>[{0}]</color> was <color=red>ADDED</color> to array with ID <color=blue>[{1}]</color>", unit.Type, unit.ID));
        }
    }
    public void AddNewEnemy(Unit unit)
    {
            unit.Target = mainBase.MarkerPosition;
            enemys.Add(unit);
            DPanel.Add(this, string.Format("New UNIT type <color=yellow>[{0}]</color> was <color=red>ADDED</color> to array with ID <color=blue>[{1}]</color>", unit.Type, unit.ID));
    }

    public void RemoweUnit(Unit unit)
    {
        units.Remove(unit);
        if (matchs.Count > 0)
            foreach (var m in matchs)
                m.Remove(unit);
        DPanel.Add(this, string.Format("UNIT type <color=yellow>[{0}]</color> was <color=red>REMOWED</color> from array with ID <color=blue>[{1}]</color>", unit.Type, unit.ID));
    }
    public void RemoweEnemy(Unit unit)
    {
        enemys.Remove(unit);        
    }
}

[System.Serializable]
public class MatchMarkersUnits
{
    public Marker marker;
    public int markerID;
    public Unit unit;
    public int unitID;

    public MatchMarkersUnits(Marker marker)
    {
        this.marker = marker;
        markerID = marker.ID;
        unit = null;
        unitID = -1;
    }
    public MatchMarkersUnits(Unit unit)
    {
        this.unit = unit;
        unitID = unit.ID;
        marker = null;
        markerID = -1;
    }
    public MatchMarkersUnits(Marker marker, Unit unit)
    {
        this.marker = marker;
        markerID = marker.ID;
        this.unit = unit;
        unitID = unit.ID;
    }
    public void Set(Marker marker)
    {
        this.marker = marker;
        markerID = marker.ID;
        if (unit != null)
            unit.Target = marker.MarkerPosition;
    }
    public Marker Set(Unit unit)
    {
        this.unit = unit;
        unitID = unit.ID;
        return marker;
    }
    public Unit Get(Marker marker)
    {
        if (this.marker.Equals(marker))
            return unit;
        return null;
    }
    public Marker Get(Unit unit)
    {
        if (this.unit == unit)
            return marker;
        return null;
    }
    public void Set(Marker marker, Unit unit)
    {
        if (this.marker != marker) return;
        this.unit = unit;
        unitID = unit.ID;
    }
    public void Remove(Marker marker)
    {
        if (this.marker.Equals(marker))
        {
            this.marker = null;
            unit.Target = null;
        }
    }
    public void Remove(Unit unit)
    {
        if (this.unit.Equals(unit))
            this.unit=null;
    }
    public bool isLackUnit()
    {
        if (unit == null) return true;
        return false;
    }
    public bool isLackMarker()
    {
        if (marker == null) return true;
        return false; ;
    }
    public bool isMine(Marker marker)
    {
        return (this.marker.Equals(marker));
    }
    public bool isMine(Unit unit)
    {
        return (this.unit.Equals(unit));
    }
}


