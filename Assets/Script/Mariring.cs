using UnityEngine;
using System.Collections;


namespace Mariring
{

    public struct RopeState
    {
        //public bool ropeRiding;     //로프라이딩 (로프 발사 준비중)
        //public bool isRopeShooting; //로프공격중
        public bool ropeRidable;    //로프를 탈 수 있는 상태
        public bool ropeLeft;       //로프가 어느 방향이냐
        public bool inRope;         //로프 안에 있어
        public float ropeTime;      //로프 있었던 시간
        public GameObject rope;
    }


    public enum HeroState
    {
        Idle,
        Running,
        Combo_1,
        Combo_2,
        Combo_3,
        RopeRiding,
        RopeFlying,
        RopeAttack,
        FrontHit,
        BackHit,
        Fever
    }

    public enum EnemyState
    {
        Idle =0 ,
        Running,
        RunningReady,
        RunningReadyComplete,
        Ready,
        Attack,
        KnockBack,
        Hit,
        Dead,
        Rush,

    }

    public enum EnemyRushState
    {
        Ready,
        Start,
        Rush,
        Attack,
    }

    public enum AttackStyle
    {
        Hammering,
        StrongHammering,
        DropKick,
        Clothesline,
        Chop,
    }

    public enum EnemyValue
    {
        Normal = 0 ,
        Rush,
        Angry
    }

    public enum EnemyPatternLevel
    {
        Hard,
        Normal,
        Easy,
        Fever
    }


    public class AttackStyles
    {
        
        public static string GetAttackAnimationName(AttackStyle _style)
        {
            switch(_style)
            {
                case AttackStyle.Hammering:
                    return "attack";

                case AttackStyle.StrongHammering:
                    return "attack";

                case AttackStyle.Clothesline:
                    return "laliatt";

                case AttackStyle.DropKick:
                    return "dropkick";

                case AttackStyle.Chop:
                    return "chop";


            }

            return "attack";
        }


        public static float GetAttackAnimationTime(AttackStyle _atkStyle)
        {
            switch (_atkStyle)
            {

                case AttackStyle.Hammering:
                    return 0.6f;

                case AttackStyle.StrongHammering:
                    return 0f;

                case AttackStyle.DropKick:
                    return 0.8f;

                case AttackStyle.Clothesline:
                    return 0.65f;

                case AttackStyle.Chop:
                    return 0.6f;

            }

            return 0f;

        }


    }



}
