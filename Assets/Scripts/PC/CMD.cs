using ConsoleTables;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // ok
using System.Text;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger; // Ok
using static UnityEngine.Rendering.DebugUI.Table; // OK

public class CMD : MonoBehaviour
{
    public Database Database;
    private List<Table> tables;

    [SerializeField] private TMP_Dropdown columnChoice;
    [SerializeField] private TMP_Dropdown tableChoice;
    [SerializeField] private TMP_Text outputText;

    private int currentTableIndex = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tables = Database.tables;

        AppendTablesDropdown();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.Alpha1))
        {
            OutputEverything(tableChoice.options[tableChoice.value].ToString());
            print("output begin");
        }*/
        if (Input.GetKeyDown(KeyCode.Return))
        {
            print(tableChoice.options[tableChoice.value].ToString().ToLower());
            print("output begin return");
            StartCoroutine(OutputEverything(tableChoice.options[tableChoice.value].text));
        }
    }
    private void AppendTablesDropdown()
    {
        tableChoice.ClearOptions();
        List<string> tableNames = new List<string>();
        for (int i = 0; i < tables.Count; i++)
        {
            tableNames.Add(tables[i].name);
        }
        tableChoice.AddOptions(tableNames);
    }
    public void SetActiveTable(int index)
    {
        currentTableIndex = index;
        AppendColumnDropdown();
        print("set active");
    }
    private void AppendColumnDropdown()
    {
        columnChoice.ClearOptions();
        List<string> columnNames = new List<string>();
        Table activeTable = tables[currentTableIndex];
        for (int i = 0; i < activeTable.data.Count; i++)
        {
            columnNames.Add(activeTable.data[i].column);
        }
        columnChoice.AddOptions(columnNames);
    }
    public IEnumerator OutputEverything(string tableName)
    {
        foreach (Table table in tables)
        {
            if (table.tableName.ToLower() == tableName.ToLower())
            {
                float t = Time.unscaledTime;
                StringBuilder output = new StringBuilder();
                List<string> cols = new List<string>();
                foreach (DatabaseEntry entry in table.data)
                {
                    cols.Add(entry.column);
                    //output.Append(entry.column + "\t");
                }
                ConsoleTable tableOutput = new ConsoleTable(cols.ToArray());
                //output.Append("\n");
                tableOutput.Options.EnableCount = false;
                outputText.text = tableOutput.ToString();
                yield return new WaitForEndOfFrame();
                for (int i = 0; i < table.data[0].rows.Count; i++)
                {
                    List<string> rows = new List<string>();
                    for (int j = 0; j < table.data.Count; j++)
                    {
                        rows.Add(table.data[j].rows[i]);
                        yield return new WaitForEndOfFrame();
                    }
                    tableOutput.AddRow(rows.ToArray());
                    outputText.text = tableOutput.ToString();
                    /*foreach (DatabaseEntry entry in table.data)
                    {
                        output.Append(entry.rows[i] + "\t");
                    }
                    output.Append("\n");*/
                }

                //print(tableOutput.ToMinimalString());
                //outputText.text = output.ToString();
                outputText.text = tableOutput.ToString() + "\n\n" +
                    $"{table.data[0].rows.Count} rows selected in {Time.unscaledTime - t} seconds";
                outputText.rectTransform.sizeDelta = new Vector2(outputText.preferredWidth, outputText.preferredHeight);
                yield break;
                // return;
            }
        }
    }
}
