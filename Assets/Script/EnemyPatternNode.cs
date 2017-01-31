using UnityEngine;
using System.Collections;
using Mariring;

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
    [SerializeField]
    public float nodeStartDelay = 0f;       //노드시작 딜레이

    [SerializeField]
    public int spawnEnemyNum;               //마디에서 스폰하는 적 수

    [SerializeField]
    public EnemyInitState[] spawnEnemy;     //enemy설정 세팅


    [SerializeField]
    public EnemyValue[] spawnEnemyValue;    //적 종류

    [SerializeField]
    public bool[] spawnDirLeft;             //스폰방향

    [SerializeField]
    public float[] spawnDelay;              //스폰딜레이
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
        EnemyValue[] newEnemeyValue = new EnemyValue[spawnEnemyNum];


        for(int i = 0;i<spawnEnemyNum-1;++i)
        {
            newSpawnEnemys[i] = spawnEnemy[i];
            newSpawnDirLeft[i] = spawnDirLeft[i];
            newSpawnDelay[i] = spawnDelay[i];
            newEnemeyValue[i] = spawnEnemyValue[i];
        }

        spawnEnemy = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;
        spawnEnemyValue = newEnemeyValue;
    }

    public void ReduceEnemyNum()
    {
        if (spawnEnemyNum == 0)
            return;

        --spawnEnemyNum;

        EnemyInitState[] newSpawnEnemys = new EnemyInitState[spawnEnemyNum]; 
        bool[] newSpawnDirLeft = new bool[spawnEnemyNum];
        float[] newSpawnDelay = new float[spawnEnemyNum];
        EnemyValue[] newEnemeyValue = new EnemyValue[spawnEnemyNum];

        //Debug.Log(spawnEnemyNum);
        //Debug.Log(spawnEnemy.Length);
        for (int i = 0; i < spawnEnemyNum; ++i)
        {
            newSpawnEnemys[i] = spawnEnemy[i];
            newSpawnDirLeft[i] = spawnDirLeft[i];
            newSpawnDelay[i] = spawnDelay[i];
            newEnemeyValue[i] = spawnEnemyValue[i];
        }

        spawnEnemy = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;
        spawnEnemyValue = newEnemeyValue;

    }

    public void ChangeEnemyNum(int _num)
    {
        if (_num < 0)
            _num = 0;

        //spawnEnemyNum = _num;
        EnemyInitState[] newSpawnEnemys = new EnemyInitState[_num];
        bool[] newSpawnDirLeft = new bool[_num];
        float[] newSpawnDelay = new float[_num];
        EnemyValue[] newEnemyValue = new EnemyValue[_num];

        if(_num > spawnEnemyNum)
        {
            for (int i = 0; i < spawnEnemyNum; ++i)
            {
                newSpawnEnemys[i] = spawnEnemy[i];
                newSpawnDirLeft[i] = spawnDirLeft[i];
                newSpawnDelay[i] = spawnDelay[i];
                newEnemyValue[i] = spawnEnemyValue[i];
            }
        }
        else
        {
            for (int i = 0; i < _num; ++i)
            {
                newSpawnEnemys[i] = spawnEnemy[i];
                newSpawnDirLeft[i] = spawnDirLeft[i];
                newSpawnDelay[i] = spawnDelay[i];
                newEnemyValue[i] = spawnEnemyValue[i];
            }
        }

        spawnEnemyNum = _num;
        spawnEnemy = newSpawnEnemys;
        spawnDirLeft = newSpawnDirLeft;
        spawnDelay = newSpawnDelay;
        spawnEnemyValue = newEnemyValue;
    }


    public void SyncVariableLength()
    {
        if (spawnEnemyNum == 0)
            return;

        //Debug.Log("spawnNum : " + spawnEnemyNum + "Asd : " + spawnEnemy.Length);
        if (spawnEnemyNum != spawnEnemy.Length)
        {
            EnemyInitState[] newSpawnEnemys = new EnemyInitState[spawnEnemyNum];
            for (int i = 0; i < newSpawnEnemys.Length; ++i)
            {
                if(i>=spawnEnemy.Length)
                    break;
                newSpawnEnemys[i] = spawnEnemy[i]; 
            }
            spawnEnemy = newSpawnEnemys;
        }
        
        if(spawnEnemyNum != spawnDirLeft.Length)
        {
            bool[] newSpawnDirLeft = new bool[spawnEnemyNum];
            for (int i = 0; i < newSpawnDirLeft.Length; ++i)
            {
                if (i >= spawnDirLeft.Length)
                    break;
                newSpawnDirLeft[i] = spawnDirLeft[i];
            }
            newSpawnDirLeft = spawnDirLeft;
        }

        if(spawnEnemyNum != spawnDelay.Length)
        {
            float[] newDelayEnemys = new float[spawnEnemyNum];
            for (int i = 0; i < newDelayEnemys.Length; ++i)
            {
                if (i >= spawnDelay.Length)
                    break;
                newDelayEnemys[i] = spawnDelay[i];
            }
            spawnDelay = newDelayEnemys;
        }


        if(spawnEnemyNum != spawnEnemyValue.Length)
        {
            EnemyValue[] newSpawnEnemyValue = new EnemyValue[spawnEnemyNum];
            for (int i = 0; i < newSpawnEnemyValue.Length; ++i)
            {
                if (i >= spawnEnemyValue.Length)
                    break;

                newSpawnEnemyValue[i] = spawnEnemyValue[i];
            }
            spawnEnemyValue = newSpawnEnemyValue;
        }

    }
	
}
