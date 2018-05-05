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

        CharacterBase target = null;

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

            ChangeDirection(1f);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

        }

        protected override void Main()
        {
            /*
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug
            Vector2 v = MyPosition - mousePosition;
            if (v.x > 0f)
            {
                ChangeDirection(-1f);
            }
            else
            {
                ChangeDirection(1f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                MyDirection = -Vector2.right;
                Move(MyPosition - MyDirection);
                //ChangeDirection(-1f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MyDirection = Vector2.right;
                Move(MyPosition + MyDirection);
                //ChangeDirection(1f);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                MyPosition -= (Vector2.right - Vector2.up * 0.2f) * 0.6f;
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

            if (Input.GetKey(KeyCode.Mouse0))
            {
                foreach (PartsHand hand in MyHands)
                {
                    if (hand && hand.MyTargetJoint && hand.MyTargetJoint.enabled)
                    {
                        hand.AttackTime = 0f;
                        hand.AttackPierce(mousePosition);
                    }
                }
            }
            */

            
            if (Input.GetKeyDown(KeyCode.A))
            {
                MyPosition -= (Vector2.right - Vector2.up * 0.2f) * 0.6f;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                MyPosition += (Vector2.right - Vector2.up * 0.2f) * 0.6f;
            }

            //Auto Battle
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

                if (l > 1f)
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
                else
                {
                    foreach (PartsHand hand in MyHands)
                    {
                        if (hand && hand.MyTargetJoint && hand.MyTargetJoint.enabled)
                        {
                            PartsBase p = target.Core;
                            if (p)
                            {
                                Vector2 t = p.transform.position;
                                hand.AttackTime = 0.15f;
                                hand.AttackPierce(t);
                            }
                        }
                    }
                }

                if (!target.IsLive)
                {
                    target = null;
                }

                
            }

            //SelectEnemyes
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),0.2f);

                foreach (Collider2D col in hits)
                {
                    if (col.gameObject)
                    {
                        PartsBase parts = col.gameObject.GetComponent<PartsBase>();

                        if (parts &&
                            parts.MyParent &&
                            parts.MyParent.IsLive &&
                            parts.MyParent.MyCategory == CharacterCategory.Enemy)
                        {
                            target = parts.MyParent;
                            break;
                        }
                    }
                }
            }

            gui_debug_window.main.drawLine("HP " + HitPoint + "/" + MaxHitPoint);

            gui_debug_window.main.drawLine("Parts:"+MyParts.Count);

            //Cameraの操作
            if (CameraController.main)
            {
                focusPosition = lib.move((Vector3)focusPosition,(Vector3)MyPosition + Vector3.up*0.7f,15f);

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
