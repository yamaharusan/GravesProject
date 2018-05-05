using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class StatusManager : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        public class Abilities
        {
            public int MaxHitPoint;
            public int HitPoint;

            public int MaxStaminaPoint;
            public int StaminaPoint;

            public int STR;
            public int VIT;
            public int DEX;
            public int AGI;

            public Abilities()
            {

            }

            public void LevelUp(ExperienceData data)
            {

            }
        }

        //経験したものを
        public class ExperienceData
        {
            public float STR;
            public float VIT;
            public float DEX;
            public float AGI;

            public ExperienceData()
            {

            }
        }
    }
}