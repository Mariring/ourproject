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


        [Header("Repack")]
        public Shader repackedShader;
        public Texture2D runtimeAtlas;
        public Material runtimeMaterial;
        //




        // Use this for initialization
        void Start() 
        {
            SkeletonAnimation _ani = this.GetComponent<SkeletonAnimation>();
            Skeleton _skeleton = _ani.skeleton;
            Skin _newSkin = _skeleton.UnshareSkin(true, false, _ani.AnimationState);


            //HeadFront Setting
            RegionAttachment _newHeadFront = headFront.ToRegionAttachmentPMAClone(Shader.Find("Spine/Skeleton"));
            _newHeadFront.SetScale(1f, 1f);
            _newHeadFront.UpdateOffset();
            int headFrontSlotIndex = _skeleton.FindSlotIndex(headFrontSlot);
            _newSkin.AddAttachment(headFrontSlotIndex, headFront.name, _newHeadFront);

            //repack
            _newSkin = _newSkin.GetRepackedSkin("repacked", repackedShader, out runtimeMaterial, out runtimeAtlas);

            _skeleton.SetSkin(_newSkin);
            _skeleton.SetToSetupPose();
            //_skeleton.SetToSetupPose();
            _skeleton.SetAttachment(headFrontSlot,headFront.name);

            
           

	    }


        // Update is called once per frame
        void Update()
        {

        }

    }

}