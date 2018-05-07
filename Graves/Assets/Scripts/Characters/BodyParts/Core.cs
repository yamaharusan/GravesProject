using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
	namespace Character
	{
		namespace Parts
		{
			public class Core : Torso
			{
				protected override void Awake()
				{
					base.Awake();
				}

				// Use this for initialization
				protected override void Start()
				{
					base.Start();

					if (MyRigidbody)
					{
						MyRigidbody.mass = 1.5f;
					}
				}

				// Update is called once per frame
				protected override void Update()
				{
					base.Update();

					if (MyTargetJoint)
					{
						float time = (Time.time * MyParent.MovingSpeed * 0.5f);

						//if(MyParent.MyLegs.Count > 0)
						//MyTargetPosition = Vector2.up * MyParent.MyLegs[0].MyLegLength*1.2f;

						MyTargetJoint.target =
							MyParent.MyPosition +
							MyTargetPosition+
							new Vector2(Mathf.Cos(time) * MyParent.WalkCircle.x, 0f) * 0.3f;
					}
				}

				/**初期設定**/
				protected override void Initialization()
				{
					base.Initialization();

					MyPartCategory = PartCategory.Core;

					if (MyTargetJoint)
					{

					}
				}

		#if UNITY_EDITOR

				protected override void OnDrawGizmos()
				{
					base.OnDrawGizmos();
				}
		#endif
			}
		}
	}
}
