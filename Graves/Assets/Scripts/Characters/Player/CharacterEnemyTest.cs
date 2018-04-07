using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class CharacterEnemyTest : CharacterBase
    {
        float AttackRange = 0f;
        bool IsAttaking = false;

        public static List<CharacterEnemyTest> list = new List<CharacterEnemyTest>();

        protected override void Awake()
        {
            std_partsNum = Random.Range(35,45);
            //std_armSize = new Vector2(Random.Range(0.03f,0.06f), Random.Range(0.2f, 0.25f));
            //std_bodySize = new Vector2(Random.Range(0.05f, 0.1f), Random.Range(0.15f, 0.2f));

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
                        if (hand)
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
