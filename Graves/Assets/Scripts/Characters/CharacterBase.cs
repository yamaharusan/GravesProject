using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
・自分の子ゲームオブジェクトを検索していき、自分の肉体の組成を取得する
    ・遺伝子データによるキャラクターの生成は別のクラスで行う

**/

namespace Graves
{
    public class CharacterBase : MonoBehaviour
    {
        //@Public
        /**Parts**/
        [System.NonSerialized]
        public List<PartsBase> MyParts = new List<PartsBase>();

        [System.NonSerialized]
        public PartsCore Core;

        [System.NonSerialized]
        public List<PartsLeg> MyLegs = new List<PartsLeg>();

        [System.NonSerialized]
        public List<PartsHand> MyHands = new List<PartsHand>();

        protected int std_partsNum = 45;
        protected Vector2 std_armSize = new Vector2(0.04f,0.35f);
        protected Vector2 std_bodySize = new Vector2(0.16f, 0.16f);

        /**Movement**/
        [System.NonSerialized]
        public Vector2 MyPosition = Vector2.zero;

        [System.NonSerialized]
        public Vector2 MyDirection = Vector2.right;

        [System.NonSerialized]
        public bool IsWalk = false;

        [System.NonSerialized]
        public bool IsChangeDirection = false;

        [System.NonSerialized]
        public int LegCount = 0;

        [System.NonSerialized]
        public int HandCount = 0;

        [System.NonSerialized]
        public int GroundedLegCount = 0;

        //[System.NonSerialized]
        public float MovingSpeed = 8f;

        //[System.NonSerialized]
        public Vector2 WalkCircle = new Vector2(0.2f,0.2f);

        /**Battle**/
        public enum CharacterCategory
        {
            Player,
            Enemy,
            None = -1
        }

        /**自分の役割**/
        [System.NonSerialized]
        public CharacterCategory MyCategory = CharacterCategory.Enemy;

        [System.NonSerialized]
        public int EnemyLayer = 0;

        public string EnemyLayerName = "Enemy";

        public int StandardHitPoint = 10;

        //@Private
        private float glavity = 0f;

        Dictionary<PartsBase.PartCategory, float> PartsDict = new Dictionary<PartsBase.PartCategory, float>() {
            { PartsBase.PartCategory.Torso, 60f},
            { PartsBase.PartCategory.Leg,30f},
            { PartsBase.PartCategory.Hand,10f}
        };

        protected virtual void Awake()
        {
            Initialization();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            //gui_debug_3dLine.main.setWidth(0.01f);
            //gui_debug_3dLine.main.draw(MyPosition, 0.085f);

            //おれはしんだのか
            CheckDead();

            //
            Main();

            //いどう
            Movement();
        }

        protected virtual void Main()
        {
        
        }

        protected virtual void CheckDead()
        {
            RefreshPartsInfo();

            if (HandCount == 0)
            {
                MovingSpeed = 2f;
            }

            if (LegCount == 0 || HandCount == 0)
            {
                if (Core.IsLive)
                {
                    Core.HitPoint = 0;
                }

                Destroy(this);
            }
        }

        private void RefreshPartsInfo()
        {
            LegCount = 0;
            foreach (PartsLeg leg in MyLegs)
            {
                if (leg.IsLive)
                {
                    leg.MyLegCount = LegCount;
                    LegCount++;
                }
            }

            HandCount = 0;
            foreach (PartsHand hand in MyHands)
            {
                if (hand.IsLive)
                {
                    HandCount++;
                }
            }
        }

        /**移動に関する処理**/
        protected virtual void Movement()
        {
            MyDirection.Normalize();

            //地形に追従して上下移動する処理
            if (GroundedLegCount > 0)//接地判定
            {
                Vector2 avrLegPos = Vector2.zero;

                foreach (PartsLeg leg in MyLegs)
                {
                    if (leg.IsGrounded)
                    {
                        avrLegPos += leg.GroundedPosition;
                    }
                }

                avrLegPos /= GroundedLegCount;//AVERAGE

                MyPosition += Vector2.up * (avrLegPos.y - MyPosition.y) * 0.25f;

                glavity = 0f;

            }
            else
            {
                if (glavity < 0.2f)
                    glavity += Time.deltaTime * 0.1f;

                MyPosition -= new Vector2(0f, glavity);
            }

            GroundedLegCount = 0;
        }

        public virtual void Move(Vector2 target)
        {
            Vector2 p = target - MyPosition;
            if (p.x > 0f)
            {
                MyDirection =  Vector2.right;
            }
            else
            {
                MyDirection = -Vector2.right;
            }

            //ChangeDirection(MyDirection.x);

            IsWalk = true;

            StartCoroutine(StopWalk());
        }

        protected IEnumerator StopWalk()
        {
            yield return null;
            yield return null;
            IsWalk = false;
        }

        //音


        //初期化
        protected virtual void Initialization()
        {
            //自分の子にあるパーツを全て登録
            SearchParts(transform);

            if (Core && MyParts.Count < 2)
            {
                body(false,Core,3,std_armSize,std_bodySize);
            }

            //
            MyPosition = transform.position;

            //
            EnemyLayer = (int)Mathf.Pow(2,LayerMask.NameToLayer(EnemyLayerName));

        }

        private void body(bool bo,PartsTorso p, int num, Vector2 a, Vector2 b)
        {
            Vector2 r = new Vector2(0.8f, 0.9f);

            b = new Vector2(b.x * r.x, b.y * r.y);

            if (MyParts.Count > std_partsNum)
                return;

            if (num < 1)
            {
                p.ChildPartsPositions.Add(-Vector2.up * p.Size.y / 2f);
                if (MyHands.Count < 3)
                {
                    p.CreateBody(PartsBase.PartCategory.Hand, 0).SetBodySize(a);
                }
                else
                {
                    p.CreateBody(PartsBase.PartCategory.Leg, 0).SetBodySize(a);
                }
            }
            else
            {
                if (!bo)
                {
                    for (int i = 0; i < num*3; i++)
                    {
                        p.ChildPartsPositions.Add(new Vector2(Random.Range(-p.Size.x, p.Size.x) / 2f, Random.Range(-p.Size.y, p.Size.y) / 2f));

                        PartsTorso pt = p.CreateBody(PartsBase.PartCategory.Torso, i).GetComponent<PartsTorso>() ;
                        if (pt)
                        {
                            pt.SetBodySize(b);
                            body(ProbabilityCalclator.DetectFromPercent(45f), pt, num, a, b);
                        }
                    }
                }
                else
                {
                    a = new Vector2(a.x * r.x, a.y * r.y);

                    p.ChildPartsPositions.Add(-Vector2.up * p.Size.y / 2f);
                    PartsTorso pt = p.CreateBody(PartsBase.PartCategory.Torso, 0).GetComponent<PartsTorso>();
                    if (pt)
                    {
                        pt.SetBodySize(a);
                        body(true, pt, num-1, a, b);
                    }
                }
            }
        }

        public void RegisterParts(PartsBase p)
        {
            MyParts.Add(p);

            //親に設定
            p.MyParent = this;

            //Core
            PartsCore core = p.gameObject.GetComponent<PartsCore>();
            if (core)
            {
                Core = core;
            }
            else
            {
                //脚の設定
                PartsLeg leg = p.gameObject.GetComponent<PartsLeg>();
                if (leg)
                {
                    leg.MyLegCount = LegCount;
                    LegCount++;
                    MyLegs.Add(leg);
                }
                else
                {
                    //手の設定
                    PartsHand hand = p.gameObject.GetComponent<PartsHand>();
                    if (hand)
                    {
                        MyHands.Add(hand);
                    }
                }
            }
        }

        //Partさがす
        protected void SearchParts(Transform _t)
        {
            foreach (Transform t in _t)
            {
                PartsBase p = t.gameObject.GetComponent<PartsBase>();

                if (p)
                {
                    //Debug.Log("find:" + MyParts.Count + ":" + p.gameObject.name);
                    RegisterParts(p);
                    SearchParts(t);
                }
            }
        }

        #region Utility

        protected void ChangeDirection(float d)
        {
            if(! (Mathf.Abs(Mathf.Lerp(-1f, 1f, transform.localScale.x) + Mathf.Lerp(-1f,1f,d)) > 1f))
                Mirror(d);
        }

        //ボディの反転処理
        protected void Mirror(float d)
        {
            //反転

            if (MyParts.Count > 0)
            {

                bool isChanged = false;
                Vector2 temp_pos = MyParts[0].transform.position;
                transform.position = MyPosition;
                MyParts[0].transform.position = temp_pos;

                if (d > 0)
                {
                    if (transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        MyDirection = Vector2.right;
                        isChanged = true;
                    }
                }
                else if (d < 0)
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        MyDirection = -Vector2.right;
                        isChanged = true;
                    }
                }

                //変更適用
                foreach (PartsBase p in MyParts)
                {
                    //
                    if (p.MyRigidbody)
                    {
                        //ボディの破綻を補正
                        p.CorrectBodyPosition();

                        //もし反転していたら
                        if (isChanged)
                        {

                            //ヒンジジョイントのリミットを反転
                            foreach (PartsBase.HingeData h in p.MyHingeJointList)
                            {
                                if (h.hingeJoint && h.hingeJoint.useLimits)
                                {
                                    JointAngleLimits2D lim = h.hingeJoint.limits;
                                    float l = -lim.min;
                                    lim.min = -h.hingeJoint.limits.max;
                                    lim.max = l;

                                    h.hingeJoint.limits = lim;
                                }
                            }

                            //ターゲットジョイントの位置を反転
                            if (p.MyTargetJoint)
                            {
                                Vector2 v = p.MyTargetPosition;
                                p.MyTargetPosition = new Vector3(-v.x, v.y);
                            }


                        }
                    }
                }

                IsChangeDirection = true;
                StartCoroutine(RestoreIsChangeDirection());

            }
        }

        private IEnumerator RestoreIsChangeDirection()
        {
            yield return null;
            IsChangeDirection = false;
        }

        #endregion
    }
}
