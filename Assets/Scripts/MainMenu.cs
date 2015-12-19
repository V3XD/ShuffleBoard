using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Auto;
    public GameObject Button;
    public GameObject CursorPrefab;
    public Dropdown dropdownID;
    public GameObject Flick;
    protected TouchCursor touchCursor;
    private GameObject Cursor;
    private bool buttonsEnabled;
    private Parameters parameters;

    // OnValueChanged dropdown
    public void setID()
    {
        parameters.ID = dropdownID.value;
        if (parameters.ID != 0)
        {
            Cursor = Instantiate(CursorPrefab);
            touchCursor = Cursor.GetComponent<TouchCursor>();
            EnableButtons();
        }
    }

    private void Awake()
    {
        parameters = Parameters.Instance;
        buttonsEnabled = false;
    }

    private void Start()
    {
        if (parameters.ID != 0)
        {
            dropdownID.options = parameters.listIDs;
            dropdownID.value = parameters.ID;
            parameters.trialNumber = 0;
        }
        else
        {
            ReadIdDirectories();
        }
    }

    private void FixedUpdate()
    {
        if (buttonsEnabled)
        {
            if (touchCursor.isUp())
            {
                Ray ray = new Ray(Cursor.transform.position, Vector3.forward);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.collider != null)
                {
                    if (hit.collider.name == ("Auto"))
                    {
                        parameters.condition = 1;
                    }
                    else if (hit.collider.name == ("Button"))
                    {
                        parameters.condition = 2;
                    }
                    else if (hit.collider.name == ("Flick"))
                    {
                        parameters.condition = 3;
                    }

                    ReadParameters(parameters.condition.ToString());
                    parameters.SetPath();
                    File.AppendAllText(parameters.outpath, parameters.outputHeader + Environment.NewLine);
                    SceneManager.LoadScene("Game");
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.F4))
        {
            Application.Quit();
        }
    }

    private void EnableButtons()
    {
        Button.GetComponent<Renderer>().enabled = true;
        Auto.GetComponent<Renderer>().enabled = true;
        Flick.GetComponent<Renderer>().enabled = true;
        buttonsEnabled = true;
    }

    private void ReadIdDirectories()
    {
        string logDirectory = @"Log\";
        string[] subDirectories = Directory.GetDirectories(logDirectory);
        dropdownID.options.Clear();
        dropdownID.options.Add(new Dropdown.OptionData("-"));
        foreach (string subdirectory in subDirectories)
        {
            string ID = subdirectory.Remove(0, logDirectory.Length);
            dropdownID.options.Add(new Dropdown.OptionData(ID));
        }

        parameters.listIDs = dropdownID.options;
    }

    private void ReadParameters(string condition)
    {
        string filename = parameters.ID.ToString() + "." + condition + ".csv";
        string logDirectory = @"Log\" + parameters.ID.ToString() + @"\";
        StreamReader reader = new StreamReader(File.OpenRead(logDirectory + filename));
        reader.ReadLine(); // skip header
        parameters.totalTrials = 0;
        parameters.delay.Clear();
        parameters.barrierPosition.Clear();
        parameters.force.Clear();
        parameters.autoShootDelay.Clear();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            parameters.delay.Add(float.Parse(values[1]));
            parameters.barrierPosition.Add(int.Parse(values[2]));
            parameters.force.Add(int.Parse(values[3]));
            if (values.Length > 4) // condition 1: auto shoot
            {
                parameters.autoShootDelay.Add(float.Parse(values[4]));
            }
            else
            {
                parameters.autoShootDelay.Add(0);
            }

            parameters.totalTrials++;
        }
    }
}