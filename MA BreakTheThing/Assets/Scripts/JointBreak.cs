using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
