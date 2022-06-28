using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "CreateScriptable/databases/characterDatabase", order = 0)]
public class CharacterDatabase : ScriptableObject
{
    public CharacterScriptable[] characters;
}