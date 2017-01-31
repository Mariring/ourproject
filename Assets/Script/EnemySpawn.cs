using UnityEngine;
using System.Collections;
using Mariring;

public class EnemySpawn : MonoBehaviour
{

    public GameObject leftSpawn;
    public GameObject rightSpawn;

    public GameObject enemy;
    public Hero hero;

    public float spawnDelay;

	// Use this for initialization
	void Awake () 
    {
        StartCoroutine(SpawnEnemyCoroutine());
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    
	}


    void SpawnEnemy()
    {
        GameObject _enemy;

        int spawnDir = Random.Range(0, 2);

        if (hero.ropeState.inRope || hero.hState == HeroState.RopeRiding)//hero.ropeState.ropeRiding)
        {
            if (hero.ropeState.ropeLeft)
                spawnDir = 1;
            else
                spawnDir = 0;
        }

        if (spawnDir==0)    //left
        {
            _enemy = (GameObject)Instantiate(enemy, leftSpawn.transform.position, Quaternion.identity);
            _enemy.GetComponent<Enemy>().isLeft = false;
        }
        else    //right
        {
            _enemy = (GameObject)Instantiate(enemy, rightSpawn.transform.position, Quaternion.identity);
            _enemy.GetComponent<Enemy>().isLeft = true;
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while(true)
        {
            float _spawnDelay= spawnDelay;

            if (hero.score > 80)
            {
                _spawnDelay = Random.Range(spawnDelay / 8, spawnDelay/6);
            }
            if(hero.score>40)
            {
                _spawnDelay = Random.Range(spawnDelay /6, spawnDelay / 4);
            }
            else if( hero.score>30)
            {
                _spawnDelay = Random.Range(spawnDelay / 4, spawnDelay / 2);
    
            }
            else if( hero.score >20)
            {
                _spawnDelay = Random.Range(spawnDelay / 2, spawnDelay);
            }
            else if (hero.score > 10)
            {
                _spawnDelay = spawnDelay;
            }

            
            yield return new WaitForSeconds(_spawnDelay);

            SpawnEnemy();
        }
    }
}
