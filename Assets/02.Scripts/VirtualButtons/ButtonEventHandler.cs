using UnityEngine;
using System.Collections;
using Vuforia;

public class ButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    public UnitSpawner spawner;

    void Start()
    {
        // Register with the virtual buttons TrackableBehaviour
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
        {
            vbs[i].RegisterEventHandler(this);
        }
    }
    // Called when the virtual button has just been pressed:
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        DPanel.Add(this, string.Format("Virtual button was <color=red>click</color> with name <color=blue>[]</color>", vb.name));
        spawner.SpawnNewUnit();
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased");
        DPanel.Add(this, string.Format("Virtual button was <color=red>released</color> with name <color=blue>[]</color>", vb.name));
    }
}