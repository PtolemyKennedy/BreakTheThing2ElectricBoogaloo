﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// number of explosives used in the current level
    /// </summary>
    public int[] explosivesUsed = new int[] { 0, 0, 0, 0 };

    /// <summary>
    /// maximum number of explosives available for full score on the current level
    /// </summary>
    public int[] maxExplosives = new int[] { 0, 0, 0, 0 };

    public GameObject _SmallExplosive;
    public GameObject _MediumExplosive;
    public GameObject _LargeExplosive;
    public GameObject _Implosive;
    public GameObject _EndGameWin;
    public GameObject _EndGameLose;

    private string LevelNumber;

    private Touch _Touch;
    private PointsSystem _PointsSystem;
    private PhPHandler _PhPHandler;


    // Use this for initialization
    void Start ()
    {        
        //set the timescale to 1 to ensure no wierd stuff happens with time
        Time.timeScale = 1;
        _Touch = GetComponent<Touch>();
        _PointsSystem = GetComponent<PointsSystem>();
        _PhPHandler = GetComponent<PhPHandler>();

        LevelNumber = SceneManager.GetActiveScene().name.Substring(6);

        maxExplosives[0] = 3; 
        maxExplosives[1] = 1;
        maxExplosives[2] = 1;
        maxExplosives[3] = 1;

        CheckExplosivesUsed();

        
        //print("This is Level " + LevelNumber);
    }

    /// <summary>
    /// resets the scene
    /// </summary>
    //public void SceneReset()
    //{       
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// starts the slow motion coroutine
    /// </summary>
    public void SlowMo()
    {
       // StartCoroutine(SlowMo(0.3f, 0.15f, 2f));
    }

    public void CheckIfLose()
    {
        StartCoroutine(CheckIfLoseGame());
    }

    /// <summary>
    /// change the view from first to third person and vice versa
    /// </summary>
    public void ChangeView()
    {
        _Touch.isFirstPerson = !_Touch.isFirstPerson;      
    }

    public void PlaceSmallExplosive()
    {
        if (explosivesUsed[0] < maxExplosives[0])
        {
            if (_Touch.isPlacingExplosive)
            {
                _Touch.isPlacingExplosive = false;
                _SmallExplosive.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _Touch.isPlacingExplosive = true;
                _Touch.explosiveToPlace = "smallExplosive";

                _SmallExplosive.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public void PlaceMediumExplosive()
    {
        if (explosivesUsed[1] < maxExplosives[1])
        {
            if (_Touch.isPlacingExplosive)
            {
                _Touch.isPlacingExplosive = false;
                _MediumExplosive.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _Touch.isPlacingExplosive = true;
                _Touch.explosiveToPlace = "mediumExplosive";

                _MediumExplosive.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public void PlaceLargeExplosive()
    {
        if (explosivesUsed[2] < maxExplosives[2])
        {
            if (_Touch.isPlacingExplosive)
            {
                _Touch.isPlacingExplosive = false;
                _LargeExplosive.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _Touch.isPlacingExplosive = true;
                _Touch.explosiveToPlace = "largeExplosive";
                _LargeExplosive.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public void PlaceImplosive()
    {
        if (explosivesUsed[3] < maxExplosives[3])
        {
            if (_Touch.isPlacingExplosive)
            {
                _Touch.isPlacingExplosive = false;
                _Implosive.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _Touch.isPlacingExplosive = true;
                _Touch.explosiveToPlace = "implosive";
                _Implosive.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public void CheckExplosivesUsed()
    {
        //set normal colours
        _SmallExplosive.GetComponent<Image>().color = Color.white;

        //update text on place explosive button
        _SmallExplosive.GetComponentInChildren<Text>().text = "Small :" + (maxExplosives[0] - explosivesUsed[0]);
        _MediumExplosive.GetComponentInChildren<Text>().text = "Medium :" + (maxExplosives[1] - explosivesUsed[1]);
        _LargeExplosive.GetComponentInChildren<Text>().text = "Large :" + (maxExplosives[2] - explosivesUsed[2]);
        _Implosive.GetComponentInChildren<Text>().text = "Implosive :" + (maxExplosives[3] - explosivesUsed[3]);

        for (int i = 0; i < 4; i++)
        {
            if (explosivesUsed[i] >= maxExplosives[i])
            {
                _PointsSystem.IsOverExplosivesCap = true;

                //set used colours
                switch (i)
                {
                    case 0:
                        _SmallExplosive.GetComponent<Image>().color = Color.red;
                        break;
                    case 1:
                        _MediumExplosive.GetComponent<Image>().color = Color.red;
                        break;
                    case 2:
                        _LargeExplosive.GetComponent<Image>().color = Color.red;
                        break;
                    case 3:
                        _Implosive.GetComponent<Image>().color = Color.red;
                        break;
                    default:
                        break;
                }
            }
        }    
    }

    /// <summary>
    /// slows down and speeds up time
    /// </summary>
    /// <param name="scale">speed at which time will flow, 0: stop time, 1: normal time</param> 
    /// <param name="time1">time before slowing down time, uses realtime seconds</param>
    /// <param name="time2">time between slowing down and resuming time, uses realtime seconds</param>
    IEnumerator SlowMo(float scale, float time1, float time2)
    {
        yield return new WaitForSecondsRealtime(time1);
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        yield return new WaitForSecondsRealtime(time2);
        Time.timeScale = 1;
    }

    IEnumerator CheckIfLoseGame()
    {
        yield return new WaitForSeconds(5);

        if (!_PointsSystem.IsWin)
        {
            //player has lost
            _EndGameLose.SetActive(true);
            //return to main menu
            GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<LoadingScreenControl>().LoadScreen("MainMenu");
        }
    }

    public void ShowWinScreen()
    {
        _EndGameWin.SetActive(true);
    }

    public void SubmitScore()
    {
        //get name from input field
        string name = _EndGameWin.GetComponentInChildren<InputField>().text;
        //get points
        int points = _PointsSystem.GetPoints();

        //submit to database
        _PhPHandler.User = name;
        _PhPHandler.Score = points.ToString();
        _PhPHandler.Level = LevelNumber;
        _PhPHandler.OnShowUsersButtonClick();
        string[] names = _PhPHandler.MultiOutput;

        foreach (var ExistingName in names)
        {
            if (name == ExistingName)
            {
                _PhPHandler.ConvertNamesToIDs();
                _PhPHandler.OnAddScoreButtonClick();
                GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<LoadingScreenControl>().LoadScreen("MainMenu");
                return;
            }
        }

        _PhPHandler.OnAddUserButtonClick();
        _PhPHandler.ConvertNamesToIDs();
        _PhPHandler.OnAddScoreButtonClick();

        //return to main menu
        GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<LoadingScreenControl>().LoadScreen("MainMenu");
    }
}

