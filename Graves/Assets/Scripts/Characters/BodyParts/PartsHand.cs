using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class PartsHand : PartsBase
    {
        [System.NonSerialized]
        public bool IsAttacking = false;

        [System.NonSerialized]
        public bool EnableSword = false;

        private bool IsParrySword = false;

        [System.NonSerialized]
        public int AttackDamage = 20;

        [System.NonSerialized]
        public float AttackTime = 0.4f;

        [System.NonSerialized]
        public float MaxHandLength = 0f;

        private List<GameObject> attackedParts = new List<GameObject>();

        private float HandLength = 0f;

        private Vector2 TargetPosition = Vector2.zero;
        private Vector2 TargetDirection = Vector2.zero;

        private List<FixedJoint2D> FixedJointList = new List<FixedJoint2D>();

        protected override void Awake()
        {
            base.Awake();
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            MaxHandLength = GetLength(transform);
            HitPoint = MyParent.StandardHitPoint * 4;

            if (MyParent)
            {
                Vector2 v = new Vector2(
                    Random.Range(-MaxHandLength, MaxHandLength),
                    Random.Range(-MaxHandLength, MaxHandLength)) * 0.3f;

                MyTargetPosition = MyParent.Core.MyTargetPosition/2f + v;
            }

            MyRigidbody.mass = 1f;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            //Walk
            if (MyParent)
            {
                if (MyTargetJoint)
                {
                    if (IsAttacking)
                        TargetDirection = (TargetPosition - (Vector2)transform.position).normalized;
                    else
                        TargetDirection = Vector2.right * MyParent.transform.localScale.x;

                    if (MyRigidbody)
                    {
                        float d = Vector2.Dot(TargetDirection, transform.right);
                        MyRigidbody.AddTorque(-d * 5f - MyRigidbody.angularVelocity * 0.001f);
                    }

                    MyTargetJoint.target =
                        MyParent.MyPosition
                        + MyTargetPosition
                        + TargetDirection * HandLength;

                    //攻撃判定
                    JudgeAttack();
                }

                if (MyParent.IsChangeDirection)
                {
                    foreach (FixedJoint2D f in FixedJointList)
                    {
                        if (f)
                        {
                            Destroy(f);
                        }
                    }
                    FixedJointList.Clear();
                }

            }
        }

        /**初期設定**/
        protected override void Initialization()
        {
            base.Initialization();

            MyPartCategory = PartCategory.Hand;

            if (MyTargetJoint)
            {
                //MyTargetJoint.frequency = 0f;
            }
        }

        protected void JudgeAttack()
        {
            if (EnableSword && !IsParrySword)
            {

                Vector2 u = transform.up;
                Vector2 p = (Vector2)transform.position + u * (Size.y / 2f);

                //レイキャストで当たったかどうか判定
                RaycastHit2D[] hit = Physics2D.RaycastAll(p, -u, Size.y, MyParent.EnemyLayer);

                //当たったら
                if (hit.Length > 0)
                {
                    foreach (RaycastHit2D ray in hit)
                    {
                        bool found = false;

                        GameObject obj = ray.transform.gameObject;
                        foreach (GameObject f_obj in attackedParts)
                        {
                            if (obj == f_obj)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            //gui_debug_3dLine.main.draw(ray.point,0.2f);

                            PartsBase part = obj.GetComponent<PartsBase>();
                            if (part && (part.IsLive || true))
                            {
                                int damageAttenuation = 1;
                                if (attackedParts.Count > 0)
                                {
                                    damageAttenuation = attackedParts.Count + 1;
                                }

                                /*
                                //pari-
                                if (part.MyPartCategory == PartCategory.Hand)
                                {
                                    PartsHand hand = GetComponent<PartsHand>();
                                    if (hand)
                                    {
                                        if(hand.EnableSword)
                                            hand.StartCoroutine(hand.C_Parry(0.2f));
                                    }
                                }
                                */

                                Ray r = new Ray(ray.point, MyRigidbody.velocity);
                                part.AddDamage(AttackDamage / damageAttenuation, r, 2f);

                                if (true || part.HitPoint <= 0)
                                {
                                    if (GetComponent<FixedJoint2D>() == null)
                                    {
                                        FixedJoint2D f = gameObject.AddComponent<FixedJoint2D>();
                                        f.connectedBody = part.MyRigidbody;

                                        FixedJointList.Add(f);

                                        if (part.HitPoint > 0)
                                        {

                                            if (part.MyPartCategory != PartCategory.Core)
                                            {
                                                Destroy(f, 1f);
                                            }
                                            else
                                            {
                                                Destroy(f, 0.5f);
                                            }
                                        }
                                        else
                                        {
                                            Destroy(f, 3f);
                                        }
                                    }
                                }
                                attackedParts.Add(obj);

                                if (MyParent)
                                {
                                    if (MyParent.MyCategory == CharacterBase.CharacterCategory.Player)
                                    {
                                        CameraController.main.effect1(0.05f);
                                    }
                                }
                            }
                        }
                    }
                }

                gui_debug_3dLine.main.setColor(Color.red);
                gui_debug_3dLine.main.draw(p, p - u * Size.y);
            }
        }

        public void AttackPierce(Vector2 target)
        {
            if (IsLive && !IsAttacking)
            {
                IsAttacking = true;
                StartCoroutine(C_AttackPierce(target));
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }

        public IEnumerator C_Parry(float time)
        {
            IsParrySword = true;
            if(MyTargetJoint)
                MyTargetJoint.frequency = 0f;

            yield return new WaitForSeconds(time);

            IsParrySword = false;
            if(MyTargetJoint)
                MyTargetJoint.frequency = MyTargetDefaultFrequency;
        }

        protected IEnumerator C_AttackPierce(Vector2 target)
        {
            TargetPosition = target;
            HandLength = -0.1f;

            FixedJoint2D[] fs = GetComponents<FixedJoint2D>();
            foreach (FixedJoint2D j in fs)
            {
                Destroy(j);
            }

            attackedParts.Clear();

            yield return new WaitForSeconds(AttackTime);

            TargetPosition = TargetDirection * 10f;
            HandLength = MaxHandLength * 1.05f;
            EnableSword = true;

            yield return new WaitForSeconds(0.3f);

            EnableSword = false;

            yield return new WaitForSeconds(AttackTime);

            HandLength = -0.1f;

            yield return new WaitForSeconds(0.2f);

            IsAttacking = false;
        }

        public void AttackSlash()
        {

        }

    }
}