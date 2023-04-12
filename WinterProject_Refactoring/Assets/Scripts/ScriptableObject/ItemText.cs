using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemText", menuName = "Scriptable Object/Item Text")]
public class ItemText : ScriptableObject
{
    public string[] ItemName;
    public string[] ItemExplan;
}
