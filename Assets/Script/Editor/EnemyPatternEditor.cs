using UnityEngine;
using UnityEditor;
using System.Collections;
using Mariring;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyPattern))]
[System.Serializable]
public class EnemyPatternEditor : Editor
{
    [SerializeField]
    SerializedProperty nodeNum;
    [SerializeField]
    SerializedProperty nodeData;

    [SerializeField]
    SerializedProperty spawnDelayMin;
    [SerializeField]
    SerializedProperty spawnDelayMax;

    [SerializeField]
    EnemyPatternNode[] _enemyPatternNodes;

    [SerializeField]
    SerializedProperty enemyPatternFold;
    [SerializeField]
    SerializedProperty nodeInfoFold;
    [SerializeField]
    SerializedProperty enemyInfoFold;



    void OnEnable()
    {

        nodeNum = serializedObject.FindProperty("nodeNum");
        nodeData = serializedObject.FindProperty("nodeData");
        spawnDelayMin = serializedObject.FindProperty("spawnDelayMin");
        spawnDelayMax = serializedObject.FindProperty("spawnDelayMax");

        enemyInfoFold = serializedObject.FindProperty("enemyInfoFold");
        nodeInfoFold = serializedObject.FindProperty("nodeInfoFold");
        enemyInfoFold = serializedObject.FindProperty("enemyInfoFold");

        EnemyPattern enemyPattern = (EnemyPattern)target;

    }


    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        //base.OnInspectorGUI();    //기본 인스페거에 오버라이딩 됨
        serializedObject.Update();

        EditorGUIUtility.LookLikeInspector();

        EnemyPattern enemyPattern = (EnemyPattern)target;

        EditorGUILayout.Space();


        #region PatternMinMaxSetting

        enemyPattern.patternSettingFold = EditorGUILayout.Foldout(enemyPattern.patternSettingFold, "Pattern Setting");
        if (enemyPattern.patternSettingFold)
        {

            ++EditorGUI.indentLevel;
            EditorGUILayout.LabelField("SpawnDelay MinMax Setting", EditorStyles.boldLabel);
            EditorGUILayout.MinMaxSlider(ref enemyPattern.spawnDelayMin, ref enemyPattern.spawnDelayMax, 0f, 50f);
            EditorGUILayout.BeginHorizontal();
            enemyPattern.spawnDelayMin = EditorGUILayout.FloatField(enemyPattern.spawnDelayMin);
            enemyPattern.spawnDelayMax = EditorGUILayout.FloatField(enemyPattern.spawnDelayMax);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Hp MinMax Setting", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            enemyPattern.enemyHpMin = EditorGUILayout.IntField(enemyPattern.enemyHpMin);
            enemyPattern.enemyHpMax = EditorGUILayout.IntField(enemyPattern.enemyHpMax);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Speed MinMax Setting", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            enemyPattern.enemySpeedMin = EditorGUILayout.FloatField(enemyPattern.enemySpeedMin);
            enemyPattern.enemySpeedMax = EditorGUILayout.FloatField(enemyPattern.enemySpeedMax);
            EditorGUILayout.EndHorizontal();


            --EditorGUI.indentLevel;
         
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        #endregion


        #region NodeSetting

        EditorGUILayout.LabelField("Node Setting", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        //노트 추가 버튼
        if (GUILayout.Button("노드 추가") == true)
        {
            enemyPattern.AddNodeNum();
        }

        //노드 지우기 버튼
        if (GUILayout.Button("노드 제거") == true)
        {
            enemyPattern.ReduceNodeNum();
        }

        EditorGUILayout.EndHorizontal();


        //NodeNum 라벨
        EditorGUILayout.LabelField("Node Count : " + enemyPattern.nodeNum);


        EditorGUILayout.Space();

        #endregion


        enemyPattern.enemyPatternFold = EditorGUILayout.Foldout(enemyPattern.enemyPatternFold, "Pattern Info");

        //enemyPatternFold = EditorGUILayout.Foldout(enemyPatternFold, "PatternInfo");

        if (enemyPattern.enemyPatternFold)
        {
            ++EditorGUI.indentLevel;
            for (int i = 0; i < enemyPattern.nodeNum; ++i)
            {
                EditorGUILayout.Space();
                enemyPattern.nodeInfoFold[i] = EditorGUILayout.Foldout(enemyPattern.nodeInfoFold[i], " Node Info " + (i + 1));


                if (enemyPattern.nodeInfoFold[i])
                {
                    ++EditorGUI.indentLevel;

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("적 하나 추가") == true)
                    {
                        enemyPattern.nodeData[i].AddEnemyNum();
                    }
                    if (GUILayout.Button("적 하나 제거") == true)
                    {
                        enemyPattern.nodeData[i].ReduceEnemyNum();
                    }

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();


                    float _nodeStartDelayValue = EditorGUILayout.FloatField("노드 시작 딜레이 : ", enemyPattern.nodeData[i].nodeStartDelay);
                    if (_nodeStartDelayValue != enemyPattern.nodeData[i].nodeStartDelay)
                    {
                        if (_nodeStartDelayValue < 0)
                            _nodeStartDelayValue = 0f;
                        if (_nodeStartDelayValue > 20f)
                            _nodeStartDelayValue = 20f;
                        enemyPattern.nodeData[i].nodeStartDelay = _nodeStartDelayValue;
                    }

                    int _spawnEnemyNum = EditorGUILayout.IntField("나오는 적 수 : ", enemyPattern.nodeData[i].spawnEnemyNum);

                    if(_spawnEnemyNum<=0)
                    {
                        _spawnEnemyNum = 1;
                    }
                    if (_spawnEnemyNum != enemyPattern.nodeData[i].spawnEnemyNum)
                    {
                        enemyPattern.nodeData[i].ChangeEnemyNum(_spawnEnemyNum);
                    }


                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.Space();
                    enemyPattern.enemyInfoFold[i] = EditorGUILayout.Foldout(enemyPattern.enemyInfoFold[i], " 적 정보 " + (i + 1));


                    if (enemyPattern.enemyInfoFold[i])
                    {
                        for (int j = 0; j < enemyPattern.nodeData[i].spawnEnemyNum; ++j)
                        {
                            enemyPattern.nodeData[i].SyncVariableLength();

                            EditorGUILayout.Space();

                            ++EditorGUI.indentLevel;
                            EditorStyles.label.fontStyle = FontStyle.Bold;
                            EditorGUILayout.LabelField("※Enemy " + i + " - " + (j + 1));
                            EditorStyles.label.fontStyle = FontStyle.Normal;
                            ++EditorGUI.indentLevel;

                            enemyPattern.nodeData[i].spawnEnemyValue[j] = (EnemyValue)EditorGUILayout.EnumPopup("적 종류", enemyPattern.nodeData[i].spawnEnemyValue[j]);

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("스폰 방향: ");
                            //Debug.Log(enemyPattern.nodeData[i].spawnDirLeft.Length);
                            //Debug.Log(j);
                            bool _isLeft = GUILayout.Toggle(enemyPattern.nodeData[i].spawnDirLeft[j], " 왼쪽");
                            if (_isLeft != enemyPattern.nodeData[i].spawnDirLeft[j])
                            {
                                enemyPattern.nodeData[i].spawnDirLeft[j] = _isLeft;
                                enemyPattern.nodeData[i].spawnEnemy[j]._isLeft = _isLeft;
                            }
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("스폰 딜레이: ");
                            float _spawnDelay = EditorGUILayout.Slider(enemyPattern.nodeData[i].spawnDelay[j], enemyPattern.spawnDelayMin, enemyPattern.spawnDelayMax);
                            if (_spawnDelay != enemyPattern.nodeData[i].spawnDelay[j])
                                enemyPattern.nodeData[i].spawnDelay[j] = _spawnDelay;
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("적 속도: ");
                            float _speed = EditorGUILayout.Slider(enemyPattern.nodeData[i].spawnEnemy[j]._speed, enemyPattern.enemySpeedMin, enemyPattern.enemySpeedMax);
                            if (_speed != enemyPattern.nodeData[i].spawnEnemy[j]._speed)
                                enemyPattern.nodeData[i].spawnEnemy[j]._speed = _speed;
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("적 체력: ");
                            int _hp = EditorGUILayout.IntSlider((int)enemyPattern.nodeData[i].spawnEnemy[j]._originHp, enemyPattern.enemyHpMin, enemyPattern.enemyHpMax);
                            if (_hp != enemyPattern.nodeData[i].spawnEnemy[j]._originHp)
                                enemyPattern.nodeData[i].spawnEnemy[j]._originHp = _hp;
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
            EditorUtility.SetDirty(target);
        }



    }





}
