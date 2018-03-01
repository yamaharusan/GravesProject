using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
・取り敢えずベースではプラーンとした抜け殻を創れるようにする

・コアから分岐する形で、子を枝葉のように生やしていく。

・親1個、子0~n個(座標、ジョイントの種類)
    ・子の無いオブジェクトはTargetJointをつける事ができる。

・命名規則
    ・UpperCamel

**/

namespace Graves
{
    public class PartsBase : MonoBehaviour
    {
        #region Members

        //@Public
        /**rigidbody**/
        [System.NonSerialized]
        public Rigidbody2D MyRigidbody;

        /**boxcollider**/
        public BoxCollider2D MyBoxCollider;

        /**SpriteRenderer**/
        public SpriteRenderer MySpriteRenderer;

        /**parts size**/
        [System.NonSerialized]
        public Vector2 Size = Vector2.one;

        /**キャラクター管理クラス**/
        [System.NonSerialized]
        public CharacterBase MyParent;

        /**ルートのパーツオブジェクト**/
        [System.NonSerialized]
        public PartsBase RootParts;

        /**子パーツオブジェクト**/
        [System.NonSerialized]
        public List<PartsBase> ChildParts = new List<PartsBase>();

        /**Target Joint**/
        [System.NonSerialized]
        public TargetJoint2D MyTargetJoint = null;
        [System.NonSerialized]
        public Vector2 MyTargetPosition = Vector2.zero;
        [System.NonSerialized]
        public float MyTargetDefaultFrequency;

        /**Hinge Joint**/
        public struct HingeData
        {
            public Vector2 defaulAnchorPos;
            public HingeJoint2D hingeJoint;
        }
        public List<HingeData> MyHingeJointList = new List<HingeData>();

        /**肉体の役割とか**/
        public enum PartCategory
        {
            Core,
            Torso,//トルソー　胴体
            Hand,
            Leg,
            None = -1
        }

        /**自分の役割　手や足など**/
        public PartCategory MyPartCategory = PartCategory.None;

        /**HitPoint**/
        public int HitPoint = 0;

        //@Private

        #endregion

        #region Methods

        //@Protected

        protected virtual void Start()
        {
            Initialization();
        }

        protected virtual void Update()
        {
            if (MyPartCategory == PartCategory.Core)
            {
                float time = (Time.time * MyParent.MovingSpeed);

                MyTargetJoint.target =
                    MyParent.MyPosition +
                    MyTargetPosition + 
                    new Vector2(Mathf.Cos(time) * MyParent.WalkCircle.x, Mathf.Sin(time) * MyParent.WalkCircle.y) * 0.1f; ;
            }

            if (MyTargetJoint)
            {
                gui_debug_3dLine.main.setWidth(0.005f);
                gui_debug_3dLine.main.draw(MyTargetJoint.target,0.025f);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MyTargetJoint.frequency /= 1.5f;
                }


            }
        }

        //@Public

        /**この関数を呼んだ相手を親に登録**/
        public void DockingParts(PartsBase parent)
        {
            parent.ChildParts.Add(this);
            RootParts = parent;
        }

        /**ボディが破綻しそうになっていたら矯正する**/
        public void CorrectBodyPosition()
        {
            //自分の親につながるヒンジの位置を保存するべき
            foreach (HingeData h in MyHingeJointList)
            {
                //ボディが破綻しそうになっていたらデフォルトの位置に矯正する
                Vector2 a = h.hingeJoint.connectedAnchor - h.defaulAnchorPos;
                if (a.x != 0f || a.y != 0f)
                {
                    //Debug.Log(a);
                    Transform t = h.hingeJoint.connectedBody.gameObject.transform;
                    Vector3 v = t.up * a.y + t.right * a.x;

                    h.hingeJoint.connectedBody.gameObject.transform.position += v;
                }
            }
        }

        //@Private

        /**初期設定**/
        protected virtual void Initialization()
        {
            //GetRigidBody
            MyRigidbody = GetComponent<Rigidbody2D>();

            //Colllider
            MyBoxCollider = GetComponent<BoxCollider2D>();

            //Sprite
            MySpriteRenderer = GetComponent<SpriteRenderer>();

            //SetSize
            if (MyBoxCollider)
            {
                Size = MyBoxCollider.size;

                if (MySpriteRenderer)
                {
                    MySpriteRenderer.size = Size;
                }
            }
            
            //もし子にパーツが存在したら親子登録
            foreach ( Transform t in transform )
            {
                PartsBase p = t.gameObject.GetComponent<PartsBase>();
                if (p)
                {
                    p.DockingParts(this);
                }
            }

            //TJoint
            MyTargetJoint = GetComponent<TargetJoint2D>();
            if (MyTargetJoint)
            {
                MyTargetPosition = MyTargetJoint.target;
                MyTargetDefaultFrequency = MyTargetJoint.frequency;
            }

            //HJoint
            HingeJoint2D[] hJoints = GetComponents<HingeJoint2D>();
            foreach ( HingeJoint2D hj in hJoints )
            {
                HingeData h = new HingeData();
                h.hingeJoint = hj;
                h.defaulAnchorPos = hj.connectedAnchor;

                MyHingeJointList.Add(h);
            }

        }

        #endregion

        #region Utility



        #endregion
    }
}
