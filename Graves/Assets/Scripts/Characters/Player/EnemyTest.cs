using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
	namespace Character
	{
		public class EnemyTest : Base
		{
			float AttackRange = 0f;
			bool IsAttaking = false;

			public static List<EnemyTest> list = new List<EnemyTest>();

			protected override void Awake()
			{
				//std_brunch = 4;

				//std_armSize = new Vector2(0.08f, Random.Range(0.2f,0.3f));
				//std_bodySize = new Vector2(Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.2f));

				//std_a = new Vector2(Random.Range(0.9f, 1f), Random.Range(0.9f, 1.1f));
				//std_b = new Vector2(Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f));

				base.Awake();
			}

			// Use this for initialization
			protected override void Start()
			{
				base.Start();

				list.Add(this);
			}

			private void OnDestroy()
			{
				list.Remove(this);
			}

			// Update is called once per frame
			protected override void Update()
			{
				base.Update();

				IsAttaking = false;

				foreach (Parts.Hand hand in MyHands)
				{
					if (hand && hand.IsAttacking)
					{
						IsAttaking = true;
					}

					AttackRange = hand.MaxHandLength * 1.2f;
				}
			}

			protected override void Main()
			{
				if (Player.main && Player.main.Core.IsLive)
				{
					Player player = Player.main;
					Vector2 p = player.MyPosition - MyPosition;

					float dist = (p).magnitude;

					if(AttackRange > dist)
					{
                    
						foreach (Parts.Hand hand in MyHands)
						{
							Parts.Base part = player.MyParts[Random.Range(0, player.MyParts.Count)];
							if (part)
								hand.AttackPierce(part.transform.position);
						}
                    
					}
					else if(!IsAttaking)
					{

						if (p.x > 0f)
						{
							ChangeDirection(-1f);
						}
						else
						{
							ChangeDirection(1f);
						}

						Move(MyPosition + (player.MyPosition - MyPosition));
					}
				}
			}

			protected override void OnDead()
			{
				Graves.Main.enemyCount++;
			}
		}
	}
}
