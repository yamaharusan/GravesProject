using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class CharacterEnemyTest : CharacterBase
    {
        float AttackRange = 0f;
        bool IsAttaking = false;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            IsAttaking = false;

            foreach (PartsHand hand in MyHands)
            {
                if (hand && hand.IsAttacking)
                {
                    IsAttaking = true;
                }

                AttackRange = hand.MaxHandLength;
            }
        }

        protected override void Main()
        {
            if (CharacterPlayer.main)
            {
                CharacterPlayer player = CharacterPlayer.main;
                Vector2 p = player.MyPosition - MyPosition;

                float dist = (p).magnitude;

                if(AttackRange > dist)
                {
                    foreach (PartsHand hand in MyHands)
                    {
                        if(hand)
                            hand.AttackPierce(player.Core.transform.position);
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
    }
}
