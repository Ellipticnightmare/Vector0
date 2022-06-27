using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobDatabase", menuName = "CreateScriptable/databases/jobDatabase", order = 0)]
public class JobDatabase : ScriptableObject
{
    public JobScriptable[] jobs;
}