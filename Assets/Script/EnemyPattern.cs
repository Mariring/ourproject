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
    public float spawnDelayMin =0f;

    [SerializeField]
    public float spawnDelayMax = 100f;



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
