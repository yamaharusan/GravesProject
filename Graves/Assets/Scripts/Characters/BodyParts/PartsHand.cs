using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Graves
{
    public class PartsHand : PartsBase
    {



        // Use this for initialization
        protected override void Start()
        {
            base.Start();
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
                    if (MyRigidbody)
                    {
                        float d = Vector2.Dot(Vector2.right,transform.right);

                        MyRigidbody.AddTorque(d * 25f - MyRigidbody.angularVelocity * 0.01f);
                    }

                    MyTargetJoint.target =
                        MyParent.MyPosition +
                        MyTargetPosition;
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

            }
        }

    }
}