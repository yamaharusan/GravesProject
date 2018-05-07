using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
	namespace Character
	{
		namespace Parts
		{
			public class Weapown : Hand
			{
				protected override void Awake()
				{
					base.Awake();

					MyPartCategory = PartCategory.Weapown;
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
	}
}