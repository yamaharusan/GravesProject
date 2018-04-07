using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

/**
    キャラクターの基本的なスケール感として10cm以下を想定


**/

namespace Graves
{
    public class Main : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            CharacterEnemyTest.list.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("test");
            }
        }
    }
}
