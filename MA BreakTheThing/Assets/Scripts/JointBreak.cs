using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointBreak : MonoBehaviour
{
    PointsSystem _ps;
    FixedJoint fj;

	// Use this for initialization
	void Start ()
    {
        _ps = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PointsSystem>();
        fj = GetComponent<FixedJoint>();      
	}
	
	// Update is called once per frame
	//void Update ()
 //   {
		
	//}

    //when the joint breaks score points relative to how difficult the joint was to break
    private void OnJointBreak(float breakForce)
    {
        _ps.ScorePoints(fj.breakForce);
        //SpawnPoints();
    }

    //attempt at making points pop up on screen
    
    //private void SpawnPoints()
    //{       
    //    //instantiate pop up of points scored
    //    GameObject PopUp = Instantiate(Resources.Load("PointsPopUp"), gameObject.transform) as GameObject;
    //    PopUp.transform.localPosition = transform.position;
    //    PopUp.transform.localScale = new Vector3(1, 1, 1);
    //    PopUp.transform.GetComponent<Text>().text = "+ " + fj.breakForce;

    //    StartCoroutine(MoveUpAndFade(PopUp));
    //}

    //IEnumerator MoveUpAndFade(GameObject go)
    //{
    //    //makes the gameobject move up and fade out over 2 seconds
    //    for (float i = 1f; i >= 0; i -= 0.05f)
    //    {
    //        go.transform.Translate(0, 5, 0);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    Destroy(go);
    //}
}
