using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
なんかこう，バケモノをいい感じに作り出すメソッドとかをまとめたい
**/

namespace Graves
{
    public class MonsterCreator : MonoBehaviour
    {
        public static MonsterCreator main;

        public GameObject Core;
        public GameObject PartsTorso;
        public GameObject PartsHand;
        public GameObject PartsLeg;
        public GameObject eye;

        public GameObject blood;
        public GameObject continualBlood;
        public GameObject longBlood;

        private void Awake()
        {
            main = this;
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

    }
}