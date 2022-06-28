using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCharacter", menuName = "CreateScriptable/characters/createNewCharacter", order = 0)]
public class CharacterScriptable : ScriptableObject
{
    public Sprite portrait;
    public NameScriptable nameObject;
    public Material myBody;
}