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
        public float attackTime = 0f;

        private float HandLength = 0f;
        private Vector2 TargetPosition = Vector2.zero;
        private Vector2 TargetDirection = Vector2.zero;

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
                    if (IsAttacking)
                        TargetDirection = (TargetPosition - (Vector2)transform.position).normalized;
                    else
                        TargetDirection = Vector2.up;

                    if (MyRigidbody)
                    {
                        float d = Vector2.Dot(TargetDirection, transform.right);
                        MyRigidbody.AddTorque(d * 15f - MyRigidbody.angularVelocity * 0.01f);
                    }

                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        AttackPierce(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }

                    MyTargetJoint.target =
                        MyParent.MyPosition
                        + MyTargetPosition
                        + TargetDirection * HandLength;
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

        protected float GetLength(Transform t)
        {
            if (t.parent)
            {
                return GetLength(t.parent);
            }
            return 0f;
        }



        public void AttackPierce(Vector2 target)
        {
            if (!IsAttacking)
            {
                IsAttacking = true;
                StartCoroutine(C_AttackPierce(target));
            }
        }

        protected IEnumerator C_AttackPierce(Vector2 target)
        {
            TargetPosition = target;
            HandLength = -0.1f;

            yield return new WaitForSeconds(1f);

            TargetPosition = TargetDirection * 10f;
            HandLength = 1.5f;

            yield return new WaitForSeconds(0.5f);

            HandLength = -0.1f;

            yield return new WaitForSeconds(0.2f);

            IsAttacking = false;
        }

        public void AttackSlash()
        {

        }

    }
}