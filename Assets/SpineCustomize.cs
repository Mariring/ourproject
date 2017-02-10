using UnityEngine;
using System.Collections;
using Spine.Unity;

using Spine.Unity.Modules.AttachmentTools;


namespace Spine.Unity.Examples
{


    public class SpineCustomize : MonoBehaviour
    {

        [Header("Skin")]
        public Sprite headFront;
        [SpineSlot]
        public string headFrontSlot;
        public Sprite headBack;
        public Sprite bodyTopFront;
        public Sprite bodyTopBack;
        public Sprite bodyBottomFront;
        public Sprite bodyBottomBack;
        public Sprite armLeftTop;
        public Sprite armLeftBottom;
        public Sprite armRightTop;
        public Sprite armRightBottom;
        public Sprite fistRight;
        public Sprite fistLeft;
        public Sprite legLeftTop;
        public Sprite legLeftBottom;
        public Sprite legRightTop;
        public Sprite legRightBottom;
        public Sprite footLeft;
        public Sprite footRight;

        //




        // Use this for initialization
        void Start() 
        {

            Skeleton _skeleton = this.GetComponent<SkeletonAnimation>().skeleton;
            Skin _newSkin = _skeleton.UnshareSkin(true, false, this.GetComponent<SkeletonAnimation>().AnimationState);


            //HeadFront
            RegionAttachment _newHeadFront = headFront.ToRegionAttachmentPMAClone(Shader.Find("Spine/Skeleton"));
            _newHeadFront.UpdateOffset();
            int headFrontSlotIndex = _skeleton.FindSlotIndex(headFrontSlot);
            _newSkin.AddAttachment(headFrontSlotIndex, headFront.name, _newHeadFront);
            
            



	    }


        // Update is called once per frame
        void Update()
        {

        }

    }

}