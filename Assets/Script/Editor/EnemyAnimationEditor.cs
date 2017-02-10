using UnityEngine;
using UnityEditor;
using System.Collections;
using Spine.Unity;
using Mariring;

[CustomEditor(typeof(EnemyAnimation))]
[System.Serializable]
public class EnemyAnimationEditor : Editor 
{

    SerializedProperty enemy;
    SerializedProperty ani;

    /*
    SerializedProperty enteranceAniName;
    SerializedProperty idleAniName;
    SerializedProperty runAniName;
    SerializedProperty runReadyAniName;
    SerializedProperty runReadyAfterAniName;
    SerializedProperty readyAniName;
    SerializedProperty attackAniName;
    SerializedProperty knockBackAniName;
    SerializedProperty hitAniName;
    SerializedProperty deadAniName;
    SerializedProperty flyingAniName;


    SerializedProperty readyRushAniName;
    SerializedProperty startRushAniName;
    SerializedProperty rushAniName;
    SerializedProperty rushAttackAniName;

    SerializedProperty angryWalkAniName;
    */


    void OnEnable()
    {

        EnemyAnimation eAni = (EnemyAnimation)target;
        eAni.ani = eAni.GetComponent<SkeletonAnimation>();
        eAni.enemy = eAni.GetComponent<Enemy>();


        #region FindProperty
        enemy = serializedObject.FindProperty("enemy");
        ani = serializedObject.FindProperty("ani");

        //enteranceAniName = serializedObject.FindProperty("enteranceAniName");
        //idleAniName = serializedObject.FindProperty("idleAniName");
        //runAniName = serializedObject.FindProperty("runAniName");
        //runReadyAniName = serializedObject.FindProperty("runReadyAniName");
        //runReadyAfterAniName = serializedObject.FindProperty("runReadyAfterAniName");
        //readyAniName = serializedObject.FindProperty("readyAniName");
        //attackAniName = serializedObject.FindProperty("attackAniName");
        //knockBackAniName = serializedObject.FindProperty("knockBackAniName");
        //hitAniName = serializedObject.FindProperty("hitAniName");
        //deadAniName = serializedObject.FindProperty("deadAniName");
        //flyingAniName = serializedObject.FindProperty("flyingAniName");
        //readyRushAniName = serializedObject.FindProperty("readyRushAniName");
        //startRushAniName = serializedObject.FindProperty("startRushAniName");
        //rushAniName = serializedObject.FindProperty("rushAniName");
        //rushAttackAniName = serializedObject.FindProperty("rushAttackAniName");
        //angryWalkAniName = serializedObject.FindProperty("angryWalkAniName");
        #endregion


    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EnemyAnimation eAni = (EnemyAnimation)target;


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enemy);
        EditorGUILayout.PropertyField(ani);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation Name", EditorStyles.boldLabel);

        eAni.enteranceAniName = EditorGUILayout.TextField("Enter Name", eAni.enteranceAniName);
        eAni.idleAniName = EditorGUILayout.TextField("Idle Name", eAni.idleAniName);
        eAni.runAniName = EditorGUILayout.TextField("Run Name", eAni.runAniName);
        eAni.runReadyAniName = EditorGUILayout.TextField("Run Ready Name", eAni.runReadyAniName);
        eAni.runReadyAfterAniName = EditorGUILayout.TextField("Run Ready After Name", eAni.runReadyAfterAniName);
        eAni.readyAniName = EditorGUILayout.TextField("Ready Name", eAni.readyAniName);
        eAni.attackAniName = EditorGUILayout.TextField("Attack Name", eAni.attackAniName);
        eAni.knockBackAniName = EditorGUILayout.TextField("KnockBack Name", eAni.knockBackAniName);
        eAni.hitAniName = EditorGUILayout.TextField("Hit Name", eAni.hitAniName);
        eAni.deadAniName = EditorGUILayout.TextField("Dead Name", eAni.deadAniName);
        eAni.flyingAniName = EditorGUILayout.TextField("Flying Name", eAni.flyingAniName);
        
        /*
        EditorGUILayout.PropertyField(enteranceAniName);
        EditorGUILayout.PropertyField(idleAniName);
        EditorGUILayout.PropertyField(runAniName);
        EditorGUILayout.PropertyField(runReadyAniName);
        EditorGUILayout.PropertyField(runReadyAfterAniName);
        EditorGUILayout.PropertyField(readyAniName);
        EditorGUILayout.PropertyField(attackAniName);
        EditorGUILayout.PropertyField(knockBackAniName);
        EditorGUILayout.PropertyField(hitAniName);
        EditorGUILayout.PropertyField(deadAniName);
        EditorGUILayout.PropertyField(flyingAniName);
        */

        if(eAni.enemy.eValue == EnemyValue.Angry)
        {

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Angry Animation Name", EditorStyles.boldLabel);

            eAni.angryWalkAniName = EditorGUILayout.TextField("Angry : Walk Name ", eAni.angryWalkAniName);
            //EditorGUILayout.PropertyField(angryWalkAniName);

        }
        else if(eAni.enemy.eValue == EnemyValue.Rush)
        {


            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rush Animation Name", EditorStyles.boldLabel);

            eAni.readyRushAniName = EditorGUILayout.TextField("Rush : Ready Name ", eAni.readyRushAniName);
            eAni.startRushAniName = EditorGUILayout.TextField("Rush : Start Attack Name ", eAni.startRushAniName);
            eAni.rushAniName = EditorGUILayout.TextField("Rush : Rush Name ", eAni.rushAniName);
            eAni.rushAttackAniName = EditorGUILayout.TextField("Rush : Attack Name ", eAni.rushAttackAniName);
            //EditorGUILayout.PropertyField(readyRushAniName);
            //EditorGUILayout.PropertyField(startRushAniName);
            //EditorGUILayout.PropertyField(rushAniName);
            //EditorGUILayout.PropertyField(rushAttackAniName);
        }

        




        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

    
    }



}
