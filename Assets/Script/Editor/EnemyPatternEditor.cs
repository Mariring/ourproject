using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyPattern))]
public class EnemyPatternEditor : Editor
{
    SerializedProperty nodeNum;
    SerializedProperty nodeData;

    SerializedProperty spawnDelayMin;
    SerializedProperty spawnDelayMax;

    private bool enemyPatternFold = false;

    EnemyPatternNode[] _enemyPatternNodes;
    private bool[] nodeInfoFold;
    private bool[] enemyInfoFold;


    //int _noteNum;

    void OnEnable()
    {

        nodeNum = serializedObject.FindProperty("nodeNum");
        nodeData = serializedObject.FindProperty("nodeData");
        spawnDelayMin = serializedObject.FindProperty("spawnDelayMin");
        spawnDelayMax = serializedObject.FindProperty("spawnDelayMax");

        nodeInfoFold = new bool[4];     //node max Num;
        enemyInfoFold = new bool[4];


        EnemyPattern enemyPattern = (EnemyPattern)target;
        for (int i = 0; i < enemyPattern.nodeNum; ++i)
        {

        }

        //_enemyPattern = new EnemyPattern[1];    //  enemyPattern minimum 
    }


    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        //base.OnInspectorGUI();    //기본 인스페거에 오버라이딩 됨
        serializedObject.Update();



        EnemyPattern enemyPattern = (EnemyPattern)target;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        //노트 추가 버튼
        if (GUILayout.Button("AddNode") == true)
        {
            enemyPattern.AddNodeNum();
        }

        //노드 지우기 버튼
        if (GUILayout.Button("ReduceNode") == true)
        {
            enemyPattern.ReduceNodeNum();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();


        //NodeNum 라벨
        EditorGUILayout.LabelField("NodeNum : " + enemyPattern.nodeNum);


        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(spawnDelayMin);
        EditorGUILayout.PropertyField(spawnDelayMax);

        if (enemyPattern.spawnDelayMin < 0)
            enemyPattern.spawnDelayMin = 0;
        if (enemyPattern.spawnDelayMax < 0)
            enemyPattern.spawnDelayMax = 0;
        if (enemyPattern.spawnDelayMin > enemyPattern.spawnDelayMax)
            enemyPattern.spawnDelayMin = enemyPattern.spawnDelayMax;



        EditorGUILayout.Space();


        enemyPatternFold = EditorGUILayout.Foldout(enemyPatternFold, "PatternInfo");

        if (enemyPatternFold)
        {
            ++EditorGUI.indentLevel;
            for (int i = 0; i < enemyPattern.nodeNum; ++i)
            {
                EditorGUILayout.Space();
                nodeInfoFold[i] = EditorGUILayout.Foldout(nodeInfoFold[i], " NodeInfo " + (i + 1));


                if (nodeInfoFold[i])
                {
                    ++EditorGUI.indentLevel;

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("AddSpawnEnemy") == true)
                    {
                        enemyPattern.nodeData[i].AddEnemyNum();
                    }
                    if (GUILayout.Button("ReduceSpawnEnemy") == true)
                    {
                        enemyPattern.nodeData[i].ReduceEnemyNum();
                    }

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();


                    float _nodeStartDelayValue = EditorGUILayout.FloatField("NodeStartDelay : ", enemyPattern.nodeData[i].nodeStartDelay);
                    if (_nodeStartDelayValue != enemyPattern.nodeData[i].nodeStartDelay)
                    {
                        if (_nodeStartDelayValue < 0)
                            _nodeStartDelayValue = 0f;
                        if (_nodeStartDelayValue > 20f)
                            _nodeStartDelayValue = 20f;
                        enemyPattern.nodeData[i].nodeStartDelay = _nodeStartDelayValue;
                    }

                    //EditorGUILayout.LabelField("SpawnEnemyNum : " );//+ enemyPattern.nodeData[i].spawnEnemyNum);
                    int _spawnEnemyNum = EditorGUILayout.IntField("SpawnEnemyNum : ", enemyPattern.nodeData[i].spawnEnemyNum);

                    if (_spawnEnemyNum != enemyPattern.nodeData[i].spawnEnemyNum)
                    {
                        enemyPattern.nodeData[i].ChangeEnemyNum(_spawnEnemyNum);
                    }
                    //int _spawnEnemyNum = EditorGUILayout.IntSlider("SpawnEnemyNum",0,30,)
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.Space();
                    enemyInfoFold[i] = EditorGUILayout.Foldout(enemyInfoFold[i], " EnemyInfo " + (i + 1));


                    if (enemyInfoFold[i])
                    {
                        for (int j = 0; j < enemyPattern.nodeData[i].spawnEnemyNum; ++j)
                        {

                            EditorGUILayout.Space();

                            ++EditorGUI.indentLevel;
                            EditorStyles.label.fontStyle = FontStyle.Bold;
                            EditorGUILayout.LabelField("※Enemy " + i + " - " + (j + 1));
                            EditorStyles.label.fontStyle = FontStyle.Normal;
                            ++EditorGUI.indentLevel;

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("SpawnDirect: ");
                            bool _isLeft = GUILayout.Toggle(enemyPattern.nodeData[i].spawnDirLeft[j], " IsLeft");
                            if (_isLeft != enemyPattern.nodeData[i].spawnDirLeft[j])
                            {
                                enemyPattern.nodeData[i].spawnDirLeft[j] = _isLeft;
                                enemyPattern.nodeData[i].spawnEnemey[j]._isLeft = _isLeft;
                            }
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("SpawnDelay: ");
                            float _spawnDelay = EditorGUILayout.Slider(enemyPattern.nodeData[i].spawnDelay[j], enemyPattern.spawnDelayMin, enemyPattern.spawnDelayMax);
                            if (_spawnDelay != enemyPattern.nodeData[i].spawnDelay[j])
                                enemyPattern.nodeData[i].spawnDelay[j] = _spawnDelay;
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Speed: ");
                            float _speed = EditorGUILayout.Slider(enemyPattern.nodeData[i].spawnEnemey[j]._speed, 0f, 100f);
                            if (_speed != enemyPattern.nodeData[i].spawnEnemey[j]._speed)
                                enemyPattern.nodeData[i].spawnEnemey[j]._speed = _speed;
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("HP: ");
                            int _hp = EditorGUILayout.IntSlider((int)enemyPattern.nodeData[i].spawnEnemey[j]._originHp, 0, 100);
                            if (_hp != enemyPattern.nodeData[i].spawnEnemey[j]._originHp)
                                enemyPattern.nodeData[i].spawnEnemey[j]._originHp = _hp;
                            EditorGUILayout.EndHorizontal();


                            EditorGUI.indentLevel -= 2;

                        }

                    }





                    --EditorGUI.indentLevel;
                }

            }


        }

        serializedObject.ApplyModifiedProperties();


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "EditEnemyPattern");
        }



    }





}
