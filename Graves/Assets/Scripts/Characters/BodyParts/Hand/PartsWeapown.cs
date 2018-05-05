using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class PartsWeapown : PartsHand
    {
        protected override void Awake()
        {
            base.Awake();

            MyPartCategory = PartCategory.Weapown;

            AttackDamage *= 3;
        }

        protected override void Start()
        {
            base.Start();

            MyTargetJoint.frequency = MyTargetDefaultFrequency;
            MyTargetJoint.enabled = true;

            //MyRigidbody.mass = 2f;

        }

        protected override void Update()
        {
            base.Update();
        }
    }
}