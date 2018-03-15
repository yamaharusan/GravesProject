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

        [System.NonSerialized]
        public PhysicsMaterial2D deadMaterial;

        /**boxcollider**/
        [System.NonSerialized]
        public BoxCollider2D MyBoxCollider;

        /**SpriteRenderer**/
        [System.NonSerialized]
        public SpriteRenderer MySpriteRenderer;

        /**parts size**/
        //[System.NonSerialized]
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
        public Vector2 DefaultMyTargetPosition = Vector2.zero;

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
        [System.NonSerialized]
        public PartCategory MyPartCategory = PartCategory.None;

        /**HitPoint**/
        [System.NonSerialized]
        public int HitPoint = 20;

        [System.NonSerialized]
        public bool IsLive = true;

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
            if (MyTargetJoint)
            {
                gui_debug_3dLine.main.setWidth(0.01f);
                //gui_debug_3dLine.main.draw(MyTargetJoint.target, 0.05f);
            }

            if (IsLive)
            {
                if (HitPoint <= 0)
                {
                    //子も道連れ
                    foreach (PartsBase part in ChildParts)
                    {
                        part.HitPoint = 0;
                    }

                    //Destroy TargetJoint
                    if (MyTargetJoint)
                    {
                        Destroy(MyTargetJoint);
                    }

                    //Change Physics Material
                    if (MyRigidbody)
                    {
                        //gameObject.layer = 0;
                        MyRigidbody.sharedMaterial = deadMaterial;
                    }

                    Destroy(gameObject,5f);

                    //おれはしんだなむ
                    IsLive = false;
                }
            }
        }

        //@Public
        /**Attack**/
        public void AddDamage(int damage, Ray ray, float force)
        {
            PopText text = PopTextController.main.print(damage.ToString(), ray.origin);

            if (MyParent)
            {
                if(MyParent.MyCategory == CharacterBase.CharacterCategory.Player)
                {
                    text.text.color = Color.red;
                    CameraController.main.effect1(0.2f);
                }
            }

            HitPoint -= damage;
            if (HitPoint <= 0)
            {
                //親のヒンジを消す
                if (transform.parent)
                {
                    HingeJoint2D[] joints = transform.parent.gameObject.GetComponents<HingeJoint2D>();

                    foreach (HingeJoint2D j in joints)
                    {
                        if (j.connectedBody == MyRigidbody)
                        {
                            Destroy(j);
                        }
                    }
                }

                transform.parent = null;
            }

            if (MyRigidbody)
            {
                MyRigidbody.AddForceAtPosition(ray.direction * (force * damage), ray.origin);
            }
        }

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
                if (h.hingeJoint)
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

                DefaultMyTargetPosition = MyTargetPosition;

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

            //HitPoint
            HitPoint = MyParent.StandardHitPoint * 2;
            IsLive = true;
        }

        #endregion

        #region Utility



        #endregion

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            
        }

        #endif
    }
}
