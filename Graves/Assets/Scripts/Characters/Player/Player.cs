using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
	namespace Character
	{
		public class Player : Base
		{
			//汎用関数群
			StdLib lib = new StdLib();

			public static Player main;

			[System.NonSerialized]
			public int MaxStaminaPoint = 100;
			[System.NonSerialized]
			public int StaminaPoint = 100;

			private Vector2 mousePosition = Vector2.zero;

			private Vector2 focusPosition = Vector2.zero;

			private Base target = null;

			private bool CheckDodge = false;

			protected override void Awake()
			{
				base.Awake();
			}

			// Use this for initialization
			protected override void Start()
			{
				base.Start();

				MyCategory = CharacterCategory.Player;
				MyActionState = ActionState.Stand;

				mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				main = this;

				ChangeDirection(1f);

				StartCoroutine(C_ChargeStamina());
			}

			// Update is called once per frame
			protected override void Update()
			{
				base.Update();

			}

			protected override void Main()
			{

				mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				if (MyActionState == ActionState.Stand)
				{
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

						StartCoroutine(C_Dodge(KeyCode.A));

						//ChangeDirection(-1f);
					}
					else if (Input.GetKey(KeyCode.D))
					{
						MyDirection = Vector2.right;
						Move(MyPosition + MyDirection);

						StartCoroutine(C_Dodge(KeyCode.D));

						//ChangeDirection(1f);
					}
				}

				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					foreach (Parts.Hand hand in MyHands)
					{
						if (!hand.IsAttacking)
						{
							if (hand && hand.MyTargetJoint && hand.MyTargetJoint.enabled)
							{
								hand.AttackTime = 0.1f;
								hand.AttackPierce(mousePosition);

                            
							}
						}
					}
				}

				if (/** (float)MaxHitPoint*0.3f < HitPoint && **/Input.GetKeyDown(KeyCode.E))
				{
					Evolve();
				}

				gui_debug_window.main.drawLine(MyActionState.ToString());
				gui_debug_window.main.drawLine(MyParts.Count.ToString());

				/*

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

				*/

				//Cameraの操作
				if (CameraController.main)
				{
					focusPosition = lib.move((Vector3)focusPosition, (Vector3)MyPosition + Vector3.up * 0.7f, 15f);

					CameraController.main.setSmooth(6f);
					Vector3 camPos = (Vector3)focusPosition;
					CameraController.main.look(camPos, camPos - Vector3.forward * 10f);
				}
			}

			private void Evolve()
			{

				Vector2 Mypositon = MyPosition;

				HitPoint = 0;
				foreach (Parts.Base parts in MyParts)
				{
					HingeJoint2D[] joints = parts.gameObject.GetComponents<HingeJoint2D>();

					foreach (HingeJoint2D j in joints)
					{
						if (!j.connectedBody)
						{
							foreach (Parts.Base p in Grave.main.Parts)
							{
								bool find = false;
								if (parts.MyPartCategory == p.MyPartCategory)
								{
									find = true;
								}

								if(parts.MyPartCategory == Parts.Base.PartCategory.Torso)
								{
									find = true;
								}

								if (find)
								{
									p.gameObject.SetActive(true);

									Vector3 lp = p.transform.localPosition;

									p.transform.parent = parts.transform;

									p.transform.localPosition = lp + transform.right * p.RootJointAnchor.x + transform.up * p.RootJointAnchor.y;

									j.enabled = true;
									j.connectedBody = p.GetComponent<Rigidbody2D>();

									break;
								}
							}
						}
					}
				}

				Initialization();
				SetHitPoints();
				RenewHitPoint();

				MyPosition = Mypositon;
			}

			public override void Move(Vector2 target)
			{
				IsWalk = true;
				StartCoroutine(StopWalk());
			}

			private IEnumerator C_ChargeStamina()
			{
				while (true)
				{
					if(StaminaPoint < MaxStaminaPoint)
					{
						StaminaPoint += 1;
					}
					else
					{
						StaminaPoint = MaxStaminaPoint;
					}

					if (StaminaPoint < 0)
					{
						StaminaPoint = 0;
					}

					yield return new WaitForSeconds(0.05f);
				}
			}

			private IEnumerator C_DodgeAction(KeyCode key)
			{
				int needSp = 35;

				if (needSp <= StaminaPoint)
				{
					StaminaPoint -= needSp;

					MyActionState = ActionState.Move;

					float time = 0f;

					while (true)
					{
						float direction = 1f;

						if (key == KeyCode.A)
						{
							direction = -1f;
						}

						Vector2 vel = (Vector2.right * direction - Vector2.up * 0.3f) * 15f * Time.deltaTime;

						MyPosition += vel;

						time += Time.deltaTime;

						if (0.08f <= time)
						{
							break;
						}

						yield return null;
					}

					yield return new WaitForSeconds(0.3f);

					MyActionState = ActionState.Stand;
				}

				yield return null;
			}

			private IEnumerator C_Dodge(KeyCode key)
			{
				yield return null;

				if (!CheckDodge)
				{
					CheckDodge = true;

					float time = 0f;

					while (true)
					{
						if (Input.GetKeyDown(key) && MyActionState == ActionState.Stand)
						{
							StartCoroutine(C_DodgeAction(key));
							break;
						}

						time += Time.deltaTime;

						if (0.3f <= time)
						{
							break;
						}

						yield return null;
					}

					CheckDodge = false;
				}

				yield return null;
			}
		}
	}
}
