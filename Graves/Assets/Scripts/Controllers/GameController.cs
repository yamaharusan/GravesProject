﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Instantiateは重いので予め呼び出して使いまわす。
つくらない方がいいもっと、細かく分離したほうがいい(スコア・シーン遷移など)
**/

namespace Graves
{
    public class GameController : MonoBehaviour
    {
        public Animator UiBlackOut;

        // Use this for initialization
        void Start()
        {
            UiBlackOut.SetBool("IsActive", false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
