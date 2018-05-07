using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
・取り敢えずベースではプラーンとした抜け殻を創れるようにする

・コアから分岐する形で、子を枝葉のように生やしていく。

・親1個、子0~n個(座標、ジョイントの種類)
    ・子の無いオブジェクトはTargetJointをつける事ができる。

・命名規則
    ・UpperCamel

**/

namespace Graves
{
	namespace Character
	{
		namespace Parts
		{
			public class Base : MonoBehaviour
			{
				#region Members

				protected StdLib lib = new StdLib();

				//@Public
				/**rigidbody**/
				[System.NonSerialized]
				public Rigidbody2D MyRigidbody;

				[System.NonSerialized]
				public PhysicsMaterial2D deadMaterial;

				/**boxcollider**/
				[System.NonSerialized]
				public BoxCollider2D MyBoxCollider;

				/**SpriteRenderer**/
				[System.NonSerialized]
				public SpriteRenderer MySpriteRenderer;

				/**parts size**/
				[System.NonSerialized]
				public Vector2 Size = Vector2.one;

				/**キャラクター管理クラス**/
				[System.NonSerialized]
				public Character.Base MyParent;

				/**ルートのパーツオブジェクト**/
				[System.NonSerialized]
				public Base RootParts;

				[System.NonSerialized]
				public Vector2 RootJointAnchor;

				/**子パーツオブジェクト**/
				[System.NonSerialized]
				public List<Base> ChildParts = new List<Base>();

				/**Target Joint**/
				[System.NonSerialized]
				public TargetJoint2D MyTargetJoint = null;

				[System.NonSerialized]
				public Vector2 MyTargetPosition = Vector2.zero;

				[System.NonSerialized]
				public Vector2 DefaultMyTargetPosition = Vector2.zero;

				[System.NonSerialized]
				public float MyTargetDefaultFrequency;

				/**Hinge Joint**/
				public struct HingeData
				{
					public int num;
					public Vector2 defaultAnchorPos;
					public Vector2 defaultConnectedAnchorPos;
					public HingeJoint2D hingeJoint;
				}

				public List<HingeData> MyHingeJointList = new List<HingeData>();

				/**肉体の役割とか**/
				public enum PartCategory
				{
					Core,
					Torso,//トルソー　胴体
					Hand,
					Leg,
					Weapown,
					None = -1
				}

				/**自分の役割　手や足など**/
				public PartCategory MyPartCategory = PartCategory.None;

				/**HitPoint**/
				[System.NonSerialized]
				public int MaxHitPoint = 0;
				// [System.NonSerialized]
				public int HitPoint = 0;

				[System.NonSerialized]
				public bool IsLive = true;

				[System.NonSerialized]
				public List<ParticleSystem> continualBloodList = new List<ParticleSystem>();

				//@Private

				#endregion

				#region Methods

				//@Protected

				protected virtual void Awake()
				{
					Initialization();
				}

				protected virtual void Start()
				{
					if (MyRigidbody)
					{
						float r = 0.004f / (Size.x * Size.y);
						MyRigidbody.mass = r;

						MyRigidbody.angularDrag = 6f;
						MyRigidbody.drag = 2f;
					}

					if (MyTargetJoint)
					{
						MyTargetPosition = MyTargetJoint.target;

						DefaultMyTargetPosition = MyTargetPosition;

						MyTargetDefaultFrequency = MyTargetJoint.frequency;

						//MyTargetJoint.maxForce = 10000f;
					}
				}

				protected virtual void Update()
				{
					if (MyTargetJoint)
					{
						if (MyPartCategory == PartCategory.Core || MyPartCategory == PartCategory.Hand || MyPartCategory == PartCategory.Leg) {
							gui_debug_3dLine.main.setWidth(0.005f);
							gui_debug_3dLine.main.setColor(new Color(1f,1f,1f,0.3f));
							gui_debug_3dLine.main.draw(transform.position, MyParent.MyPosition + Vector2.up * 5f);
						}
					}

					if (IsLive)
					{
						if (HitPoint <= 0)
						{
							//子も道連れ
							foreach (Base part in ChildParts)
							{
								part.HitPoint = 0;
							}

							//Destroy TargetJoint
							if (MyTargetJoint)
							{
								Destroy(MyTargetJoint);
							}

							//Change Physics Material
							if (MyRigidbody)
							{
								//gameObject.layer = 0;
								MyRigidbody.sharedMaterial = deadMaterial;
								MyRigidbody.velocity = Vector2.zero;
								MyRigidbody.mass = 0.1f;
							}

							//親のりすとから自分を消す
							for (int i = 0; i < MyParent.MyParts.Count; i++)
							{
								if (this == MyParent.MyParts[i])
								{
									MyParent.MyParts.Remove(this);
								}
							}

							Destroy(gameObject,UnityEngine.Random.Range(2f,5f));

							//おれはしんだなむ
							IsLive = false;
						}
					}

					/*
					foreach (HingeData data in MyHingeJointList)
					{
						Vector2 v = transform.right * data.hingeJoint.anchor.x + transform.up * data.hingeJoint.anchor.y;
						gui_debug_3dLine.main.draw((Vector2)transform.position + v,0.05f);
					}
					*/
				}

				private void OnDestroy()
				{
					//Particle分離
					foreach (ParticleSystem p in continualBloodList)
					{
						if (p)
						{
							p.transform.parent = null;
							p.loop = false;
						}
					}

					GameObject damageEffect = Instantiate(MonsterCreator.main.blood);
					damageEffect.transform.position = transform.position;
				}

				//@Public

        
				public void ConnectParts(Base parts,HingeJoint2D joint)
				{

				}

				public void ConnectBetweenParts(Base parts, HingeJoint2D joint)
				{

				}

				/**Attack**/
				public void AddDamage(int damage, Ray ray, float force)
				{
					if (damage > 0 && MyPartCategory != PartCategory.Weapown)
					{
						if (MyParent)
						{
							if (MyParent.MyCategory == Character.Base.CharacterCategory.Player)
							{
								CameraController.main.effect1(0.15f);
							}
						}

						HitPoint -= damage;

						if (HitPoint <= 0 && ( MyPartCategory == PartCategory.Core || MyPartCategory == PartCategory.Torso) )
						{
							int sDamage = -HitPoint + 1;

							List<Base> partsList = new List<Base>();

							foreach (Base p in MyParent.MyParts)
							{
								if (p.HitPoint > 1 && p.IsLive)
								{
									partsList.Add(p);
								}
							}

							if (partsList.Count > 0)
							{
								//余剰ダメージを他のパーツに押し付けてダメージ分散
								sDamage = DispersionDamage(sDamage,ref partsList, damage / partsList.Count);

								//それでも余ってたらむしり取る
								if (sDamage > -1)
								{
									foreach (Base p in partsList)
									{
										int d = lib.limit(sDamage, 0, p.HitPoint - 1);

										if (d > sDamage)
										{
											d = sDamage;
										}

										sDamage -= d;
										p.HitPoint -= d;

										if (sDamage < 1)
											break;
									}
								}
							}

							//借金を清算できたら生き残れる
							if (sDamage <= 0)
							{
								HitPoint = 1;
							}
							else
							{
								HitPoint = 0;
							}
						}

						if (HitPoint <= 0)
						{
							//親のヒンジを消す
							if (transform.parent)
							{
								HingeJoint2D[] joints = transform.parent.gameObject.GetComponents<HingeJoint2D>();

								foreach (HingeJoint2D j in joints)
								{
									if (j.connectedBody == MyRigidbody)
									{
										j.enabled = false;
										j.connectedBody = null;

										//けむり
										GameObject damageEffect = Instantiate(MonsterCreator.main.continualBlood);

										RootParts.continualBloodList.Add(damageEffect.GetComponent<ParticleSystem>());

										damageEffect.transform.parent = transform.parent;

										damageEffect.transform.localPosition = j.anchor;

										Vector2 v = transform.right * j.anchor.x + transform.up * j.anchor.y;
										damageEffect.transform.rotation = Quaternion.FromToRotation(Vector2.up, -v.normalized);

									}
								}
							}

							transform.parent = null;
						}

						/*
						if (MyRigidbody && HitPoint > 0)
						{
							MyRigidbody.AddForceAtPosition(ray.direction * force * damage * 0.8f, ray.origin, ForceMode2D.Impulse);
						}
						*/
                
					}
				}

				private int DispersionDamage(int sDamage,ref List<Base> partsList, int damage)
				{
					int d = sDamage;

					if (damage > 0)
					{
						bool canPay = false;

						for (int i = 0; i < partsList.Count; i++)
						{
							if (partsList[i] && partsList[i].HitPoint > damage)
							{
								canPay = true;

								partsList[i].HitPoint -= damage;
								d -= damage;

								if (d <= 0)
								{
									break;
								}
							}

						}

						if (d > 0 && canPay)
						{
							d = DispersionDamage(d, ref partsList, damage);
						}
					}
					return d;
				}

				/**この関数を呼んだ相手を親に登録**/
				public void DockingParts(Base parent)
				{
					parent.ChildParts.Add(this);
					RootParts = parent;
				}

				/**ボディが破綻しそうになっていたら矯正する**/
				public void CorrectBodyPosition()
				{
					//自分の親につながるヒンジの位置を保存するべき
					foreach (HingeData h in MyHingeJointList)
					{
						if (h.hingeJoint && h.hingeJoint.enabled)
						{
							//ボディが破綻しそうになっていたらデフォルトの位置に矯正する
							Vector2 a = h.hingeJoint.connectedAnchor - h.defaultConnectedAnchorPos;
							if (a.x != 0f || a.y != 0f)
							{
								//Debug.Log(a);
								Transform t = h.hingeJoint.connectedBody.gameObject.transform;
								Vector3 v = t.up * a.y + t.right * a.x;

								h.hingeJoint.connectedBody.gameObject.transform.position += v;
							}
						}
					}
				}

				//@Private

				/**初期設定**/
				protected virtual void Initialization()
				{
					//GetRigidBody
					MyRigidbody = GetComponent<Rigidbody2D>();

					//Colllider
					MyBoxCollider = GetComponent<BoxCollider2D>();

					//Sprite
					MySpriteRenderer = GetComponent<SpriteRenderer>();

					//SetSize
					if (MySpriteRenderer && MySpriteRenderer.drawMode != SpriteDrawMode.Simple)
					{
						Size = MySpriteRenderer.size;

						if (MyBoxCollider)
						{
							MyBoxCollider.size = Size;
						}
					}
					else
					{
						if (MyBoxCollider)
						{
							Size = MyBoxCollider.size;
						}
					}

					//もし子にパーツが存在したら親子登録
					foreach ( Transform t in transform )
					{
						Base p = t.gameObject.GetComponent<Base>();
						if (p)
						{
							p.DockingParts(this);
						}
					}

					//TJoint
					MyTargetJoint = GetComponent<TargetJoint2D>();

					if (MyRigidbody)
					{
					   // MyRigidbody.mass = 0.05f;
					}

					//HJoint
					HingeJoint2D[] hJoints = GetComponents<HingeJoint2D>();
					foreach (HingeJoint2D hj in hJoints)
					{
						//接続先がないやつはオフ
						if (!hj.connectedBody)
						{
							hj.enabled = false;
						}
						else
						{
							Base parts = hj.connectedBody.gameObject.GetComponent<Base>();
							if (parts)
							{
								parts.RootJointAnchor = hj.connectedAnchor;
							}
						}

						//登録
						HingeData h = new HingeData();
						h.num = MyHingeJointList.Count;
						h.hingeJoint = hj;
						h.defaultConnectedAnchorPos = hj.connectedAnchor;
						h.defaultAnchorPos = hj.anchor;

						MyHingeJointList.Add(h);
					}

					IsLive = true;
				}

				//setSize
				public virtual void SetBodySize(Vector2 size)
				{
					Size = size;

					if (MySpriteRenderer)
					{
						MySpriteRenderer.size = Size;
					}

					if (MyBoxCollider)
					{
						MyBoxCollider.size = Size;
					}
				}

				public float GetLength(Transform t)
				{
					float length = 0f;
					Transform t_p = t;
					Vector2 temp = t_p.position;

					while (t_p.parent)
					{
						Rigidbody2D rb = t_p.GetComponent<Rigidbody2D>();

						t_p = t_p.parent;

						HingeJoint2D[] joints = t_p.gameObject.GetComponents<HingeJoint2D>();

						if (rb && joints.Length > 0)
						{
							foreach (HingeJoint2D j in joints)
							{
								if (j.connectedBody == rb)
								{
									Vector2 p = t_p.position + t_p.TransformVector(j.anchor);

									length += (temp - p).magnitude;
									temp = p;

									//gui_debug_3dLine.main.draw(temp, 0.1f);
									break;
								}
							}

							if(joints.Length != 1)
							{
								break;
							}
						}
						else
						{
							break;
						}
					}

					return length;
				}

				#endregion

				#region Utility



				#endregion

		#if UNITY_EDITOR

				private void OnDrawGizmos()
				{
            
				}

				#endif
			}
		}
	}
}
