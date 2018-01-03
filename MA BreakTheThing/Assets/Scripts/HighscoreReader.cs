using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighscoreReader : MonoBehaviour
{
    private PhPHandler phpHandler;
    private string DatabaseOutput;
    private Text Names;
    private Text Scores;
    private Dropdown LevelDropdown;
    void Start()
    {
        phpHandler      = GameObject.FindGameObjectWithTag("DatabaseManager").GetComponent<PhPHandler>();
        LevelDropdown   = GameObject.FindGameObjectWithTag("Dropdown").GetComponent<Dropdown>();

        //don't judge me
        GameObject[] OutputBoxes = GameObject.FindGameObjectsWithTag("DataBaseOutput");
        foreach (var box in OutputBoxes)
        {
            print(box.name);
            if (box.name == "Names")
            {
                Names = box.GetComponent<Text>();
            }
            else
            {
                Scores = box.GetComponent<Text>();
            }
        }
        OnLevelDropdownChanged();
    }
    // Update is called once per frame
    void Update ()
    {

	}

    public void OnLevelDropdownChanged()
    {
        Names.text = "";
        Scores.text = "";
        phpHandler.Level = (LevelDropdown.value + 1).ToString();
        phpHandler.OnShowScoresButtonClick();
        DatabaseOutput = phpHandler.Output;
        string[] namesAndScores = DatabaseOutput.Split(' ');
        for (int i = 0; i < namesAndScores.Length; i++)
        {
            print("Entry No. " + i + " " + namesAndScores[i]);

            if (i % 2 == 0)
            {
                Names.text = Names.text + namesAndScores[i] + '\n';
            }
            else
            {
                Scores.text = Scores.text + namesAndScores[i] + '\n';
            }
        }

    }
}
