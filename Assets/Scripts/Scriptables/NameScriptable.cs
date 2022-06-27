using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNameObject", menuName = "CreateScriptable/name/createNewNameObject", order = 0)]
public class NameScriptable : ScriptableObject
{
    public string firstName, middleInitial, lastName;
    public string[] firstNameVariations, middleInitialVariations, lastNameVariations;
}