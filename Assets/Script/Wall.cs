using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour 
{

    public bool isLeftWall;

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("Enemy"))
        {
            if(coll.gameObject.GetComponent<Enemy>() !=null)
            {
                Enemy _enemy = coll.gameObject.GetComponent<Enemy>();
                
                if(isLeftWall)
                {
                }
                else
                {

                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {

    }


}

