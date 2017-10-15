using UnityEngine;
using Vuforia;

public class Marker : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private void Start()
    {

        if (mTrackableBehaviour == null)
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();

            if (mTrackableBehaviour != null)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }
        MarkerPosition = transform;
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.TRACKED)
        {            
            MarkersManager.inst.MarkerTracked(this);
            istracked = true;
            DPanel.Add(this, string.Format("<color=green>Find</color> marker type <color=yellow>[{0}]</color> with ID<color=blue>[{1}]</color>", Type,ID));
        }
        else
        {
            istracked = false;
            MarkersManager.inst.MarkerLost(this);
            DPanel.Add(this, string.Format("<color=red>Lost</color> marker type <color=yellow>[{0}]</color> with ID<color=blue>[{1}]</color>", Type, ID));
        }
    }

    public Transform MarkerPosition;
    public MarkerType Type;
    public int ID;
    public bool istracked;

    public enum MarkerType
    {
        MainBase = 1,
        Soldier = 2,
    }
}
