using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameDatabase", menuName = "CreateScriptable/databases/nameDatabase", order = 0)]
public class NameDatabase : ScriptableObject
{
    public NameScriptable[] nameObjects;
}