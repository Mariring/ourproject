using UnityEngine;
using System.Collections;
using Mariring;

public class EnemyAngry : EnemyNormal 
{
    [Header("Angry Time")]
    public float unBeatableTime;

	// Use this for initialization
	void Awake () 
    {
        base.Awake();

        eValue = EnemyValue.Angry;
        isUnbeatable = true;

	}
    void Start()
    {
        base.Start();
    } 
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        base.FixedUpdate();


	}


    void Update()
    {
        base.Update();
        StateUpdate();
    }

    void StateUpdate()
    {

        if(unBeatableTime>0f)
        {
            unBeatableTime -= Time.deltaTime;
        }
        else
        {
            isUnbeatable = false;
        }




    }


}
