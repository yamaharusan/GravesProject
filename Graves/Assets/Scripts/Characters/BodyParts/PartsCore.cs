using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class PartsCore : PartsBase
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

            if (MyTargetJoint)
            {
                float time = (Time.time * MyParent.MovingSpeed);

                MyTargetJoint.target =
                    MyParent.MyPosition +
                    MyTargetPosition +
                    new Vector2(Mathf.Cos(time) * MyParent.WalkCircle.x, Mathf.Sin(time) * MyParent.WalkCircle.y) * 0.1f;
            }
        }

        /**初期設定**/
        protected override void Initialization()
        {
            base.Initialization();

            MyPartCategory = PartCategory.Core;

            HitPoint = MyParent.StandardHitPoint * 2;

            if (MyTargetJoint)
            {

            }
        }
    }
}
