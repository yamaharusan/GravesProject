using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class CharacterPlayer : CharacterBase
    {
        //汎用関数群
        StdLib lib = new StdLib();

        public static CharacterPlayer main;

        private Vector2 mousePosition = Vector2.zero;

        private Vector2 focusPosition = Vector2.zero;

        private Vector2 defaultCorePosition = Vector2.zero;

        CharacterEnemyTest target = null;

        protected override void Awake()
        {
            base.Awake();
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            MyCategory = CharacterCategory.Player;

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            main = this;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

        }

        protected override void Main()
        {
            //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /*

            //Debug
            if (Input.GetKey(KeyCode.A))
            {
                MyDirection = -Vector2.right;
                Move(MyPosition - MyDirection);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MyDirection = Vector2.right;
                Move(MyPosition + MyDirection);
            }

            if (Core)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Core.MyTargetPosition = defaultCorePosition / 2f;


                }
                else
                {
                    if (defaultCorePosition == Vector2.zero)
                    {
                        defaultCorePosition = Core.MyTargetPosition;
                    }
                    Core.MyTargetPosition = defaultCorePosition;

                }
            }

            */

            if (target)
            {
                float l = (target.Core.transform.position - Core.transform.position).magnitude;

                Vector2 mp = MyPosition - target.MyPosition;

                if (mp.x > 0f)
                {
                    ChangeDirection(-1f);
                }
                else
                {
                    ChangeDirection(1f);
                }

                if (l > 1.5f)
                {
                    if (Core.transform.position.x - target.Core.transform.position.x > 0f)
                    {
                        MyDirection = -Vector2.right;
                        Move(MyPosition - MyDirection);
                    }
                    else
                    {
                        MyDirection = Vector2.right;
                        Move(MyPosition + MyDirection);
                    }
                }

                foreach (PartsHand hand in MyHands)
                {
                    if (hand)
                    {
                        Vector2 p = hand.transform.position;
                        Vector2 t = target.Core.transform.position;

                        if (hand.IsLive && (t - p).magnitude < hand.MaxHandLength * 1.2f)
                        {
                            hand.AttackTime = 0f;
                            hand.AttackPierce(t);
                        }
                    }
                }
            }
            else
            {
                if (CharacterEnemyTest.list.Count > 0) {
                    target = CharacterEnemyTest.list[Random.Range(0, CharacterEnemyTest.list.Count)];
                }
            }

            if (CameraController.main)
            {
                //Vector2 v = MyPosition - mousePosition;

                focusPosition = lib.move((Vector3)focusPosition,Core.transform.position + Vector3.up * 0.5f,15f);

                CameraController.main.setSmooth(6f);
                Vector3 camPos = (Vector3)focusPosition;
                CameraController.main.look(camPos, camPos - Vector3.forward * 10f);
            }
        }

        public override void Move(Vector2 target)
        {
            IsWalk = true;
            StartCoroutine(StopWalk());
        }

    }
}
