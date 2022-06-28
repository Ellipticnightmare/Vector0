using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newJob", menuName = "CreateScriptable/jobs/createNewJob", order = 0)]
public class JobScriptable : ScriptableObject
{
    public string jobName;
    public List<TaskScriptable> jobTasks = new List<TaskScriptable>();
}