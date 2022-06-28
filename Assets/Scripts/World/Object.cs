using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Object : MonoBehaviour
{
    public EntityMetadata thisData; //Assigned in Inspector to show the Object name and portrait
    public TaskScriptable myTask;
    public double progress = 100;
    public bool isRepair;
    private void Update()
    {
        if (!isRepair && progress >= 0)
        {
            progress -= Time.deltaTime;
        }
        progress = Mathf.Clamp((float)progress, 0, 100);
    }
    public void TakeSabotage()
    {
        progress -= 25;
    }
}