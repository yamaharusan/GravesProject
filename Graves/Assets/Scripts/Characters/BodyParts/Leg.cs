using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Graves
{
	namespace Character
	{
		namespace Parts
		{
			public class Leg : Base
			{
				/**t pos**/
				[System.NonSerialized]
				public int MyLegCount = 0;

				[System.NonSerialized]
				public bool IsGrounded = false;

				[System.NonSerialized]
				public Vector2 GroundedPosition = Vector2.zero;

				private bool grounded = false;

				[System.NonSerialized]
				public float MyLegLength;

				private float MyMovingSpeed;

				private Vector2 MyWalkCircle;

				protected override void Awake()
				{
					base.Awake();
				}

				// Use this for initialization
				protected override void Start()
				{
					base.Start();

					MyLegLength = GetLength(transform);

					//Debug.Log(MyLegLength);

					/*
					if (MyTargetJoint)
					{
						MyTargetPosition = new Vector2(Random.Range(-MyLegLength, MyLegLength) / 6f, 0f);
					}
					*/

					MyMovingSpeed = MyParent.MovingSpeed;// * (MyLegLength / 2f);

					MyWalkCircle = MyParent.WalkCircle;// * (MyLegLength / 0.6f);

					MyRigidbody.mass = 1f;
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
								MyRigidbody.AddTorque(transform.up.x * 5f - MyRigidbody.angularVelocity * 0.001f);
							}

							if (MyParent.IsWalk)
							{
								//Vector2 mtp = (transform.position + transform.TransformVector(MyTargetJoint.anchor));

								//float tjl = Mathf.Lerp(-1f, 1f, 0.4f - tjv.y) * 10f;

								float direction = MyParent.MyDirection.x;
								float time = -(Time.time * MyMovingSpeed) + (Mathf.PI * (2f / MyParent.LegCount) * MyLegCount);

								//MyTargetPosition += Vector2.up * tjl * Time.deltaTime;

								MyTargetJoint.target =
									MyParent.MyPosition +
									MyTargetPosition +
									new Vector2(Mathf.Cos(time) * MyWalkCircle.x * direction, Mathf.Sin(time) * MyWalkCircle.y);

								//Grounded
								if (IsGrounded)
								{
									Vector2 anchorPosition = transform.position + transform.TransformVector(MyTargetJoint.anchor);
									float dist = MyTargetJoint.target.x - anchorPosition.x;

									MyParent.MyPosition -=
										Vector2.right *
										(
											(Mathf.Cos(time - (Time.deltaTime * MyMovingSpeed)) - Mathf.Cos(time)) * 
											MyWalkCircle.x / 
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
									Vector2.up * 0.05f;
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
						MyTargetJoint.anchor = -Vector2.up * Size.y / 2f;
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
	}
}
