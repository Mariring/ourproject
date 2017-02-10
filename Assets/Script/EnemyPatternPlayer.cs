using UnityEngine;
using System.Collections;
using Mariring;

using System.Collections.Generic;

public class EnemyPatternPlayer : MonoBehaviour
{

    #region Inspector

    [Header(" ")]
    public GameObject leftSpawnPosition;
    public GameObject rightSpawnPosition;
    public Hero hero;

    [Header("Spawn Enemy")]
    public GameObject enemy;
    public GameObject rushEnemy;
    public GameObject angryEnemy;

    [Header("Test Mode")]
    public bool test;
    public int testPtNum;

    #endregion

    #region MemberVariable

    //패턴들
    Object[] ptObject;
    EnemyPattern[] patterns;
    EnemyPattern feverPattern;

    //선택된 패턴
    EnemyPattern selectedPt;
    //선택된 노드
    EnemyPatternNode selectedNode;

    //스폰타이머
    float enemySpawnDelayTimer;
    //노드타이머
    float nodeDelayTimer;
    //현재 노드(마디) 순서
    int nowNodeNum;
    //현재 적 스폰 순서
    int nowSpawnEnemyNum;

    bool nodeDelaying;
    bool spawnsDelaying;

    #endregion

    bool isPlay;
    bool isFever;

    List<Enemy> feverEnemys;


    void Awake()
    {
        Init();

        if (patterns.Length == 0)
        {
            this.enabled = false;
            return;
        }

        SelectRandomPattern();
        SelectNowNode();

        feverEnemys = new List<Enemy>();

    }

    void Init()
    {
        isPlay = false;

        enemySpawnDelayTimer = 0f;
        nodeDelayTimer = 0f;
        nowNodeNum = 0;
        nowSpawnEnemyNum = 0;

        nodeDelaying = true;
        spawnsDelaying = false;

        ptObject = Resources.LoadAll("Prefabs/EnemyPrefab");

        patterns = new EnemyPattern[ptObject.Length];

        for(int i=0;i<patterns.Length;++i)
        {
            GameObject ptObj = (GameObject)ptObject[i];
            patterns[i] = ptObj.GetComponent<EnemyPattern>();
            
            if(patterns[i].patternLv == EnemyPatternLevel.Fever)
            {
                feverPattern = patterns[i];
            }
        }




    }

    void FixedUpdate()
    {
        if (!isPlay)
            return;

        PatternTimer();

    }

    void PatternTimer()
    {
        if (nodeDelaying)    //노드 딜레이중
        {
            nodeDelayTimer += Time.deltaTime;

            if (selectedNode.nodeStartDelay <= nodeDelayTimer)
            {
                nodeDelayTimer = 0f;
                enemySpawnDelayTimer = 0f;
                SelectNowNode();
            }

        }
        else if (spawnsDelaying)
        {
            enemySpawnDelayTimer += Time.deltaTime;

            if (selectedNode.spawnDelay[nowSpawnEnemyNum] <= enemySpawnDelayTimer)
            {
                enemySpawnDelayTimer = 0f;
                SpanwEnemy();
            }

        }
        else    //아무것도 딜레이 중이 아님
        {
            //?
        }

        if(hero.hState == HeroState.Fever && !isFever)
        {
            isFever = true;
            SelectFeverPattern();

        }
        else if(isFever && hero.hState != HeroState.Fever)
        {
            isFever = false;
            SelectRandomPattern();
            AllDeleteFeverEnemy();
        }

    }

    void SelectRandomPattern()
    {
        if(isFever)
        {
            selectedPt = feverPattern;
        }
        else if (test)
        {
            selectedPt = patterns[testPtNum];
        }
        else
        {
            selectedPt = patterns[Random.Range(0, patterns.Length)];
        }

        nowNodeNum = 0;
        nowSpawnEnemyNum = 0;
        nodeDelaying = true;
        spawnsDelaying = false;
    }

    void SelectFeverPattern()
    {
        selectedPt = feverPattern;
        nowNodeNum = 0;
        nowSpawnEnemyNum = 0;
        nodeDelaying = true;
        spawnsDelaying = false;
    }

    void SelectNowNode()
    {
        if (nowNodeNum == selectedPt.nodeNum)
        {
            SelectRandomPattern();
            return;
        }

        nowSpawnEnemyNum = 0;
        selectedNode = selectedPt.nodeData[nowNodeNum];
        nodeDelaying = false;
        spawnsDelaying = true;

    }

    void SpanwEnemy()
    {

        
        if ((hero.hState == HeroState.RopeRiding|| hero.hState == HeroState.RopeFlying) && 
            ((hero.isLeft && !selectedNode.spawnDirLeft[nowSpawnEnemyNum])    ||
            (!hero.isLeft && selectedNode.spawnDirLeft[nowSpawnEnemyNum]))
            )
        {

        }
        else
        {
            GameObject _SpawnPos = leftSpawnPosition;

            if (!selectedNode.spawnDirLeft[nowSpawnEnemyNum])
                _SpawnPos = rightSpawnPosition;

            GameObject _spawnEnemy = null;

            if (selectedNode.spawnEnemyValue[nowSpawnEnemyNum] == EnemyValue.Normal)
            {
                _spawnEnemy = (GameObject)Instantiate(enemy, _SpawnPos.transform.position, Quaternion.identity);

            }
            else if (selectedNode.spawnEnemyValue[nowSpawnEnemyNum] == EnemyValue.Angry)
            {
                _spawnEnemy = (GameObject)Instantiate(angryEnemy, _SpawnPos.transform.position, Quaternion.identity);
            }
            else if(selectedNode.spawnEnemyValue[nowSpawnEnemyNum] == EnemyValue.Rush)
            {
                _spawnEnemy = (GameObject)Instantiate(rushEnemy, _SpawnPos.transform.position, Quaternion.identity);
            }
            

            if (_spawnEnemy == null)
                return;

            //GameObject _spawnEnemy = (GameObject)Instantiate(enemy, _SpawnPos.transform.position, Quaternion.identity);
            selectedNode.spawnEnemy[nowSpawnEnemyNum]._isLeft = selectedNode.spawnDirLeft[nowSpawnEnemyNum];
            _spawnEnemy.GetComponent<Enemy>().SetInitState(selectedNode.spawnEnemy[nowSpawnEnemyNum]);
            _spawnEnemy.GetComponent<Enemy>().eValue = selectedNode.spawnEnemyValue[nowSpawnEnemyNum];

            if(selectedPt == feverPattern && hero.hState == HeroState.Fever)
            {
                feverEnemys.Add(_spawnEnemy.GetComponent<Enemy>());
            }

        }
        


        ++nowSpawnEnemyNum;


        if (selectedNode.spawnEnemyNum == nowSpawnEnemyNum)
        {
            ++nowNodeNum;
            SelectNowNode();
            return;
        }

    }


    public void AllDeleteFeverEnemy()
    {

        for(int i=0;i<feverEnemys.Count;++i)
        {
            if(feverEnemys[i] !=null)
            {
                feverEnemys[i].FeverEndFly();
            }
        }

        feverEnemys.Clear();


    }


    public void GameStart()
    {
        isPlay = true;
    }



}
