﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// number of explosives used in the current level
    /// </summary>
    public int[] explosivesUsed = new int[] { 0,0,0,0 };

    /// <summary>
    /// maximum number of explosives available for full score on the current level
    /// </summary>
    public int[] maxExplosives = new int[] { 0, 0, 0, 0 };

    //public struct Explosive
    //{
    //    public int number;
    //    public string name;

    //    public Explosive(int p1, string p2)
    //    {
    //        number = p1;
    //        name = p2;
    //    }
    //}

    //Explosive smallExplosive = new Explosive(0, "smallExplosive");
    //Explosive meidumExplosive = new Explosive(1, "mediumExplosive");
    //Explosive laregExplosive = new Explosive(2, "largeExplosive");
    //Explosive implosive = new Explosive(3, "implosive");

    public Touch _Touch;
    public PointsSystem _PointsSystem;

    // Use this for initialization
    void Start ()
    {        
        //set the timescale to 1 to ensure no wierd stuff happens with time
        Time.timeScale = 1;
        _Touch = GetComponent<Touch>();
        _PointsSystem = GetComponent<PointsSystem>();

        maxExplosives[0] = 3; //3 for test level


    }
	
	// Update is called once per frame
	//void Update ()
 //   {
		
	//}

    /// <summary>
    /// resets the scene
    /// </summary>
    public void SceneReset()
    {       
        SceneManager.LoadScene("Level 1");
    }

    /// <summary>
    /// starts the slow motion coroutine
    /// </summary>
    public void SlowMo()
    {
        StartCoroutine(SlowMo(0.3f, 0.15f, 2f));
    }

    /// <summary>
    /// change the view from first to third person and vice versa
    /// </summary>
    public void ChangeView()
    {
        _Touch.isFirstPerson = !_Touch.isFirstPerson;      
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlaceSmallExplosive()
    {
        if (_Touch.isPlacingExplosive)
        {
            _Touch.isPlacingExplosive = false;
        }
        else
        {
            _Touch.isPlacingExplosive = true;
            _Touch.explosiveToPlace = "smallExplosive";
        }
    }

    public void CheckExplosivesUsed()
    {
        for (int i = 0; i < 4; i++)
        {
            //update text on place explosive button

            if (explosivesUsed[i] >= maxExplosives[i])
            {
                _PointsSystem.IsOverExplosivesCap = true;

                //grey out the place explosive button that is over the cap
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
}

