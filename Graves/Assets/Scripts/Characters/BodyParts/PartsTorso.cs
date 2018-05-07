using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class PartsTorso : PartsBase
    {
        /**子パーツオブジェクトの接続座標ローカル**/
        [System.NonSerialized]
        public List<Vector2> ChildPartsPositions = new List<Vector2>();

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (MyRigidbody)
            {
                MyRigidbody.mass = 0.5f;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        /**初期設定**/
        protected override void Initialization()
        {
            base.Initialization();

            if (MyPartCategory == PartCategory.None)
            {
                MyPartCategory = PartCategory.Torso;
            }
        }

        //肉体生成
        public PartsBase CreateBody(PartCategory c, int num)
        {
            if (num > -1 && ChildPartsPositions.Count > num && MonsterCreator.main)
            {
                //既に生えてたらだめ
                HingeJoint2D[] hJoints = GetComponents<HingeJoint2D>();

                if (hJoints.Length > num && hJoints[num].anchor == ChildPartsPositions[num])
                {
                    return null;
                }

                //Debug.Log("c");

                GameObject obj = null;

                //カテゴリに合致してないとだめ
                switch (c)
                {
                    case PartCategory.Hand:
                        if (MonsterCreator.main.PartsHand)
                            obj = Instantiate(MonsterCreator.main.PartsHand);
                        break;
                    case PartCategory.Leg:
                        if (MonsterCreator.main.PartsLeg)
                            obj = Instantiate(MonsterCreator.main.PartsLeg);
                        break;
                    case PartCategory.Torso:
                        if (MonsterCreator.main.PartsTorso)
                            obj = Instantiate(MonsterCreator.main.PartsTorso);
                        break;
                    default:
                        return null;
                }

                if (obj != null)
                {
                    //登録
                    Vector2 p = ChildPartsPositions[num];
                    PartsBase parts = obj.GetComponent<PartsBase>();
                    Rigidbody2D rb =obj.GetComponent<Rigidbody2D>();

                    if (parts)
                    {
                        obj.transform.SetParent(transform);
                        obj.transform.rotation = Quaternion.Euler(0,0,0);
                        obj.transform.localPosition = -p - Vector2.up * (parts.Size.y);
                        obj.layer = gameObject.layer;

                        HingeJoint2D h = gameObject.AddComponent<HingeJoint2D>();
                        h.anchor = p;
                        h.connectedBody = rb;

                        parts.DockingParts(this);
                        MyParent.RegisterParts(parts);

                        return parts;
                    }
                }
            }
            return null;
        }

        //間に肉体
        public PartsBase CreateBetweenBody(PartCategory c, int num)
        {
            /*
            if (c == PartCategory.Torso && num > -1 && ChildPartsPositions.Count > num && MonsterCreator.main)
            {
                GameObject obj = null;
                HingeJoint2D hinge = null;

                //特定する
                HingeJoint2D[] hJoints = GetComponents<HingeJoint2D>();
                foreach (HingeJoint2D hj in hJoints)
                {
                    if (hj.connectedBody && hj.anchor == ChildPartsPositions[num])
                    {
                        obj = hj.connectedBody.gameObject;
                        hinge = hj;
                        break;
                    }
                }

                //あったら
                if(obj != null)
                {
                    Vector2 p = ChildPartsPositions[num];
                    PartsBase parts = obj.GetComponent<PartsBase>();

                    //親子登録解除
                    parts.RootParts = null;

                    foreach (PartsBase pa in ChildParts)
                    {
                        if (parts == pa)
                        {
                            ChildParts.Remove(pa);
                            break;
                        }
                    }

                    //kesu
                    DestroyImmediate(hinge);

                    //hayasu
                    PartsBase cParts = CreateBody(c, num);
                    PartsTorso ts = cParts.gameObject.GetComponent<PartsTorso>();
                    ts.ChildPartsPositions = ChildPartsPositions;

                    //oya
                    obj.transform.SetParent(cParts.transform);
                    obj.transform.localPosition = p + Vector2.up * (cParts.Size.y / 2f);

                    //hinge
                    HingeJoint2D h = cParts.gameObject.AddComponent<HingeJoint2D>();
                    h.anchor = p;
                    h.connectedBody = obj.GetComponent<Rigidbody2D>();
                }
            }
            */
            return null;
        }

        public override void SetBodySize(Vector2 size)
        {
            base.SetBodySize(size);
            if (RootParts)
            {
                HingeJoint2D[] hJoints = RootParts.GetComponents<HingeJoint2D>();
                foreach (HingeJoint2D hj in hJoints)
                {
                    if(hj.connectedBody == MyRigidbody)
                    {
                        transform.localPosition = hj.anchor - Vector2.up * Size.y/2f;
                        break;
                    }
                }
            }
            else
            {
                transform.localPosition = Vector2.zero;
            }

            for (int i=0;i< ChildPartsPositions.Count;i++)
            {
                Vector2 v = new Vector2(size.x / ChildPartsPositions[i].x, size.y / ChildPartsPositions[i].y);

                ChildPartsPositions[i] =
                    new Vector2(ChildPartsPositions[i].x * v.x, ChildPartsPositions[i].y * v.y);
            }
        }
        
#if UNITY_EDITOR
        
        protected virtual void OnDrawGizmos()
        {
            /*
            Gizmos.color = Color.green;
            int count = 0;

            foreach (Vector2 v in ChildPartsPositions)
            {
                Vector2 p = transform.position + transform.right * v.x + transform.up * v.y;
                Gizmos.DrawWireSphere(p, 0.04f);

                UnityEditor.Handles.Label(p, "parts:" + count);

                count++;
            }
            */
        }

#endif

    }
}
