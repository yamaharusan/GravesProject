using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Graves
{
    public class PartsLeg : PartsBase
    {
        public static int LegCount = 0;

        /**t pos**/
        [System.NonSerialized]
        public int MyLegCount = 0;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (MyPartCategory == PartCategory.Leg)
            {
                if (MyTargetJoint)
                {
                    if (MyRigidbody)
                    {
                        MyRigidbody.AddTorque(transform.up.x * 5f - MyRigidbody.angularVelocity * 0.005f);
                    }

                    Vector2 mtp = (transform.position + transform.TransformVector(MyTargetJoint.anchor));
                    Vector2 tjv = (MyTargetJoint.target - mtp);

                    //float tjl = Mathf.Lerp(-1f, 1f, 0.4f - tjv.y) * 10f;

                    int legs = 3;

                    float time = (Time.time * 5f) + (Mathf.PI * (2f/legs) * MyLegCount);

                    //MyTargetPosition += Vector2.up * tjl * Time.deltaTime;

                    MyTargetJoint.target = MyTargetPosition + new Vector2(Mathf.Cos(time) * 0.8f, Mathf.Sin(time)) * 0.2f; ;

                    gui_debug_3dLine.main.setWidth(0.003f);
                    gui_debug_3dLine.main.draw(transform.position + transform.TransformVector(MyTargetJoint.anchor), MyTargetJoint.target);

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
                if (MyPartCategory == PartCategory.Leg)
                {
                    MyLegCount = LegCount;
                    LegCount++;
                }
            }
        }
    }
}
