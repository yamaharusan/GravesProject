using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Graves
{
    public class PartsLeg : PartsBase
    {
        /**t pos**/
        [System.NonSerialized]
        public int MyLegCount = 0;

        [System.NonSerialized]
        public bool IsGrounded = false;

        [System.NonSerialized]
        public Vector2 GroundedPosition = Vector2.zero;

        private bool grounded = false;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            //接地判定
            if (grounded)
            {
                IsGrounded = true;
                grounded = false;

                if (MyParent)
                {
                    MyParent.GroundedLegCount++;
                }

            }
            else
            {
                IsGrounded = false;
                GroundedPosition = Vector2.zero;
            }

            //Walk
            if (MyPartCategory == PartCategory.Leg && MyParent)
            {
                if (MyTargetJoint)
                {
                    if (MyRigidbody)
                    {
                        MyRigidbody.AddTorque(transform.up.x * 5f - MyRigidbody.angularVelocity * 0.005f);
                    }

                    if (MyParent.IsWalk)
                    {
                        Vector2 mtp = (transform.position + transform.TransformVector(MyTargetJoint.anchor));
                        Vector2 tjv = (MyTargetJoint.target - mtp);

                        //float tjl = Mathf.Lerp(-1f, 1f, 0.4f - tjv.y) * 10f;

                        float direction = MyParent.MyDirection.x;
                        float time = -(Time.time * MyParent.MovingSpeed) + (Mathf.PI * (2f / MyParent.LegCount) * MyLegCount);

                        //MyTargetPosition += Vector2.up * tjl * Time.deltaTime;

                        MyTargetJoint.target =
                            MyParent.MyPosition +
                            MyTargetPosition +
                            new Vector2(Mathf.Cos(time) * MyParent.WalkCircle.x * direction, Mathf.Sin(time) * MyParent.WalkCircle.y);

                        //Grounded
                        if (IsGrounded)
                        {
                            Vector2 anchorPosition = transform.position + transform.TransformVector(MyTargetJoint.anchor);
                            float dist = MyTargetJoint.target.x - anchorPosition.x;

                            MyParent.MyPosition -=
                                Vector2.right *
                                (
                                    (Mathf.Cos(time - (Time.deltaTime * MyParent.MovingSpeed)) - Mathf.Cos(time)) * 
                                    MyParent.WalkCircle.x / 
                                    MyParent.GroundedLegCount
                                ) * direction;

                            MyParent.MyPosition -= Vector2.right * dist * 0.5f;

                            //gui_debug_3dLine.main.setWidth(0.006f);
                            //gui_debug_3dLine.main.draw(transform.position + transform.TransformVector(MyTargetJoint.anchor), MyTargetJoint.target);

                            //gui_debug_3dLine.main.draw(MyParent.MyPosition, 0.1f);
                        }
                    }
                    else
                    {
                        MyTargetJoint.target =
                            MyParent.MyPosition +
                            MyTargetPosition -
                            Vector2.up * 0.2f;
                    }
                }
            }
        }

        /**初期設定**/
        protected override void Initialization()
        {
            base.Initialization();

            MyPartCategory = PartCategory.Leg;

            if (MyTargetJoint)
            {

            }
        }

        /****/
        private void OnCollisionStay2D(Collision2D other)
        {
            grounded = true;

            //設置場所の中で最も下にあるものを設置場所として扱う
            if (other.contacts.Length > 0)
            {
                GroundedPosition = other.contacts[0].point;

                for (int i = 1; i < other.contacts.Length; i++)
                {
                    if (other.contacts[i].point.y < GroundedPosition.y )
                    {
                        GroundedPosition = other.contacts[i].point;
                    }
                }
            }
        }
    }
}
