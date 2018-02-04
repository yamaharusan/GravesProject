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

        /****/
        public TargetJoint2D MyTargetJoint = null;

        /**parts size**/
        [System.NonSerialized]
        public Vector2 Size = Vector2.one;

        /**キャラクター管理クラス**/
        [System.NonSerialized]
        public CharacterBase ParentCharacter;

        /**ルートのパーツオブジェクト**/
        [System.NonSerialized]
        public PartsBase RootParts;

        /**子パーツオブジェクト**/
        [System.NonSerialized]
        public List<PartsBase> ChildParts = new List<PartsBase>();

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
            if (MyPartCategory == PartCategory.Leg)
            {
                if (MyTargetJoint)
                {
                    if (MyRigidbody)
                    {
                        MyRigidbody.AddTorque(transform.up.x * 5f - MyRigidbody.angularVelocity * 0.005f );
                    }
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

        //@Private

        /**初期設定**/
        private void Initialization()
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

            //TJoint
            MyTargetJoint = GetComponent<TargetJoint2D>();

            //もし子にパーツが存在したら親子登録
            foreach ( Transform t in transform )
            {
                PartsBase p = t.gameObject.GetComponent<PartsBase>();
                if (p)
                {
                    p.DockingParts(this);
                }
            }
        }

        #endregion

        #region Utility



        #endregion
    }
}
