using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class HeroAttackBox : MonoBehaviour {

    public List<GameObject> enemyInBox;
    

	void Start () 
    {
        enemyInBox = new List<GameObject>();
	}
	
	
	void Update () 
    {
	
	}
    
    void OnTriggerStay2D(Collider2D coll)
    {
        if (!coll.CompareTag("Enemy"))
            return;

        if(!enemyInBox.Contains(coll.gameObject))
        {
            enemyInBox.Add(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Enemy"))
            return;

        if(enemyInBox.Contains(coll.gameObject))
        {
            enemyInBox.Remove(coll.gameObject);
        }
    }
    
}
