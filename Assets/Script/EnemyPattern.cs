using UnityEngine;
using System.Collections;



[AddComponentMenu("Maririg/EnemyPattern")]
[System.Serializable]
public class EnemyPattern : MonoBehaviour 
{

    //[ContextMenuItem("SetNodeDelayArray","NodeDelaySet")]
    [Tooltip("이 패턴에 들어갈 사이클 수")]
    [Range(1,4)]
    [SerializeField]
    public int nodeNum;      //노드개수

    [SerializeField]
    public EnemyPatternNode[] nodeData = new EnemyPatternNode[1];


    [SerializeField]
    public bool patternSettingFold = false;
    [SerializeField]
    public float spawnDelayMin =0f;
    [SerializeField]
    public float spawnDelayMax = 100f;
    [SerializeField]
    public int enemyHpMin = 1;
    [SerializeField]
    public int enemyHpMax = 50;
    [SerializeField]
    public float enemySpeedMin = 1f;
    [SerializeField]
    public float enemySpeedMax = 30f;


    #region EditorVariable;
    //에디터 코드 안에서 쓰고싶은데 어떻게 하지 ㅠ

    public bool enemyPatternFold =true;  //에디터 폴더 
    public bool[] nodeInfoFold = new bool[4];
    public bool[] enemyInfoFold = new bool[4];

    #endregion


    public void AddNodeNum()
    {
        if(nodeNum < 4)
            ++nodeNum;
        else
            return;

        EnemyPatternNode[] newEnemyPatternNodes = new EnemyPatternNode[nodeNum];

        for(int i=0;i<nodeNum-1;++i)
        {
            newEnemyPatternNodes[i] = nodeData[i];
        }

        nodeData = newEnemyPatternNodes;
    }

    public void ReduceNodeNum()
    {
        if (nodeNum > 1)
            --nodeNum;
        else
            return;


        EnemyPatternNode[] newEnemyPatternNodes = new EnemyPatternNode[nodeNum];

        for(int i=0;i<nodeNum;++i)
        {
            newEnemyPatternNodes[i] = nodeData[i];
        }

        nodeData = newEnemyPatternNodes;
        
    }
    



}
