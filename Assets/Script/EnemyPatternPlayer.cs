using UnityEngine;
using System.Collections;

public class EnemyPatternPlayer : MonoBehaviour
{
    public GameObject leftSpawnPosition;
    public GameObject rightSpawnPosition;
    public GameObject enemy;

    #region MemberVariable

    //패턴들
    EnemyPattern[] patterns;

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

    public bool test;

    public int testPtNum;



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

    }

    void Init()
    {
        enemySpawnDelayTimer = 0f;
        nodeDelayTimer = 0f;
        nowNodeNum = 0;
        nowSpawnEnemyNum = 0;

        nodeDelaying = true;
        spawnsDelaying = false;
        Resources.Load("Prefabs/EnemyPrefab");
        patterns = Resources.FindObjectsOfTypeAll<EnemyPattern>();
    }


    void Update()
    {


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
    }

    void SelectRandomPattern()
    {
        if (test)
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

        GameObject _SpawnPos = leftSpawnPosition;

        if (!selectedNode.spawnDirLeft[nowSpawnEnemyNum])
            _SpawnPos = rightSpawnPosition;

        GameObject _spawnEnemy = (GameObject)Instantiate(enemy, _SpawnPos.transform.position, Quaternion.identity);


        selectedNode.spawnEnemey[nowSpawnEnemyNum]._isLeft = selectedNode.spawnDirLeft[nowSpawnEnemyNum];
        _spawnEnemy.GetComponent<Enemy>().SetInitState(selectedNode.spawnEnemey[nowSpawnEnemyNum]);


        ++nowSpawnEnemyNum;


        if (selectedNode.spawnEnemyNum == nowSpawnEnemyNum)
        {
            ++nowNodeNum;
            SelectNowNode();
            return;
        }

    }






}
