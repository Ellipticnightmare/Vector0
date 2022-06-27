using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "createNewTask", menuName = "CreateScriptable/tasks/createNewTask", order = 0)]
public class TaskScriptable : ScriptableObject
{
    public string taskName;
}