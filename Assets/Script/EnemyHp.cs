using UnityEngine;
using System.Collections;

public class EnemyHp : MonoBehaviour 
{

    public GameObject[] slots;
    public Enemy myEnmey;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    
        for(int i = 0;i<slots.Length;++i)
        {
            if (myEnmey.nowHp <= i)
            {
                //sprite 미표시
                slots[i].SetActive(false);
            }
            else
            {
                //sprite 표시

                slots[i].SetActive(true);
            }
        }

	}
}
