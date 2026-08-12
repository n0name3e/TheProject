using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Table", menuName = "ScriptableObjects/Table", order = 0)]
public class Table : ScriptableObject
{
    public string tableName;
    public List<DatabaseEntry> data;

    public void RandomizePasswords()
    {
        if (tableName != "passwords")
        {
            return;
        }    
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].column != "password")
            {
                continue;
            }
            for (int j = 0; j < data[i].rows.Count; j++)
            {
                data[i].rows[j] = Random.Range(100, 1000).ToString();
            }
            break;
            //data[i].rows[] = Random.Range(100, 1000).ToString();
        }
    }
   
    
}
