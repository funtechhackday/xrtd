using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{

    #region Singleton
    public static DebugPanel inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
    }
    #endregion

    public Text debugText;    
}

public static class DPanel
{
    public static void Add(MonoBehaviour script, string text)
    {
        if (DebugPanel.inst.debugText != null && DebugPanel.inst.debugText.isActiveAndEnabled)
        {
            var s = string.Format(" <color=green>{0}</color> <color=blue>[{1}]</color> {2}\n", Time.time, script.name, text);
            DebugPanel.inst.debugText.text = s + DebugPanel.inst.debugText.text;
            Debug.Log(s);
        }
    }
}
