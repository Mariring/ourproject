using UnityEngine;
using System.Collections;


#region EnemyInitStateStruct
[System.Serializable]
public struct EnemyInitState
{
    public bool _isLeft;
    public float _speed;
    public float _originHp;
}
#endregion

[System.Serializable]
public class EnemyPatternNode
{

    #region MemberVariable
    [Range(0f,20f)]
    public float nodeStartDelay = 0f;       //노드시작 딜레이
    public int spawnEnemyNum;               //마디에서 스폰하는 적 수
    public EnemyInitState[] spawnEnemey;    //enemy설정 세팅

    public bool[] spawnDirLeft;    //스폰방향
    public float[] spawnDelay;     //스폰딜레이
    public int[] asd;
    #endregion

    EnemyPatternNode()
    {
        nodeStartDelay = 0;
        spawnEnemyNum = 0;

    }

    public void AddEnemyNum()
    {

        ++spawnEnemyNum;

        EnemyInitState[] newSpawnEnemys = new EnemyInitState[spawnEnemyNum];
        bool[] newSpawnDirLeft = new bool[spawnEnemyNum];
        float[] newSpawnDelay = new float[spawnEnemyNum];


        for(int i = 0;i<spawnEnemyNum-1;++i)
        {
            newSpawnEnemys[i] = spawnEnemey[i];
            newSpawnDirLeft[i] = spawnDirLeft[i];
            newSpawnDelay[i] = spawnDelay[i];
        }

        spawnEnemey = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;
    }

    public void ReduceEnemyNum()
    {
        if (spawnEnemyNum == 0)
            return;

        --spawnEnemyNum;

        EnemyInitState[] newSpawnEnemys = new EnemyInitState[spawnEnemyNum]; 
        bool[] newSpawnDirLeft = new bool[spawnEnemyNum];
        float[] newSpawnDelay = new float[spawnEnemyNum];


        for (int i = 0; i < spawnEnemyNum; ++i)
        {
            newSpawnEnemys[i] = spawnEnemey[i];
            newSpawnDirLeft[i] = spawnDirLeft[i];
            newSpawnDelay[i] = spawnDelay[i];
        }

        spawnEnemey = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;

    }

    public void ChangeEnemyNum(int _num)
    {
        if (_num < 0)
            _num = 0;

        //spawnEnemyNum = _num;
        EnemyInitState[] newSpawnEnemys = new EnemyInitState[_num];
        bool[] newSpawnDirLeft = new bool[_num];
        float[] newSpawnDelay = new float[_num];

        if(_num > spawnEnemyNum)
        {
            for (int i = 0; i < spawnEnemyNum; ++i)
            {
                newSpawnEnemys[i] = spawnEnemey[i];
                newSpawnDirLeft[i] = spawnDirLeft[i];
                newSpawnDelay[i] = spawnDelay[i];
            }
        }
        else
        {
            for (int i = 0; i < _num; ++i)
            {
                newSpawnEnemys[i] = spawnEnemey[i];
                newSpawnDirLeft[i] = spawnDirLeft[i];
                newSpawnDelay[i] = spawnDelay[i];
            }
        }

        spawnEnemyNum = _num;
        spawnEnemey = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;
    }
    
	
}
