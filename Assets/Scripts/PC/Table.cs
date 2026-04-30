using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Table", menuName = "ScriptableObjects/Table", order = 0)]
public class Table : ScriptableObject
{
    public string tableName;
    public List<DatabaseEntry> data;
}
