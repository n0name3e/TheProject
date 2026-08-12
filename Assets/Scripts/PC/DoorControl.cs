using UnityEngine;
using TMPro;

public class DoorControl : MonoBehaviour
{
    [SerializeField] private Table passwordTable;
    [Tooltip("Permission that opens this door")]
    [SerializeField] private int doorId = 1; // so if this door is opened with id 1
                                             // then it will select password with id 1 from passwordTable

    [SerializeField] private Door door;
    [SerializeField] private GameObject incorrectPasswordText;
    [SerializeField] private GameObject correctPasswordText;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;

    private AudioSource audioSource;
    private string correctPassword;

    private void Start()
    {
        audioSource = UI.Instance.monitorAudio;
        for (int i = 0; i < passwordTable.data.Count; i++)
        {
            if (passwordTable.data[i].column.ToLower() == "permission")
            {
                for (int j = 0; j < passwordTable.data[i].rows.Count; j++)
                {
                    if (passwordTable.data[i].rows[j] == doorId.ToString())
                    {
                        correctPassword = passwordTable.data[i+1].rows[j];
                        return;
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckPassword();
        }
    }
    private string SetCorrectPassword()
    {
        foreach (DatabaseEntry entry in passwordTable.data)
        {
            if (entry.column.ToLower() == "permission_id")
            {
                for (int i = 0; i < entry.rows.Count; i++)
                {
                    if (entry.rows[i] == doorId.ToString())
                    {
                        return passwordTable.data.Find(e => e.column == "password").rows[i];
                    }
                }
            }
        }
        return "";
        /*DatabaseEntry passwordColumn = null;

        foreach (DatabaseEntry entry in passwordTable.data)
        {
            if (entry.column.ToLower() == "permission_id")
            {
                for (int i = 0; i < entry.rows.Count; i++)
                {
                    if (entry.rows[i] == doorId.ToString())
                    {
                        targetRowIndex = i;
                    }
                }
            }

            if (entry.column.ToLower() == "password")
            {
                passwordColumn = entry;
            }
        }
        if (targetRowIndex != -1 && passwordColumn != null)
        {
            return passwordColumn.rows[targetRowIndex];
        }
        return null;*/
    }
    public void CheckPassword()
    {
        string inputPassword = passwordField.text;

        print(SetCorrectPassword());
        if (inputPassword == SetCorrectPassword())
        {
            incorrectPasswordText.SetActive(false);
            correctPasswordText.SetActive(true);
            passwordField.interactable = false;
            //door.SetActive(false);
            door.Activate();
            audioSource.PlayOneShot(correctSound);
            return;
        }
        audioSource.PlayOneShot(incorrectSound);
        passwordField.text = "";
        incorrectPasswordText.SetActive(true);
        /*for (int i = 0; i < passwordTable.data.Count; i++)
        {
            if (passwordTable.data[i].column.ToLower() == "password")
            {
                for (int j = 0; j < passwordTable.data[i].rows.Count; j++)
                {
                    if (passwordTable.data[i].rows[j] == correctPassword)
                    {
                        print("correct");
                    }
                }
            }
        }*/
    }
}
