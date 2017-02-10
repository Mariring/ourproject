using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour 
{
    [Header("Wall Direction")]
    public bool isLeftWall;

    [Header("Hero")]
    public GameObject hero;

	void Start () {
	
	}
	
	void Update () 
    {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
       


    }

    void OnCollisionStay2D(Collision2D coll)
    {

    }


}

