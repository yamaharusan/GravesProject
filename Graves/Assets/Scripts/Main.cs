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
        void Awake()
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

        public void Test()
        {
            Debug.Log("Test!!!");
        }

        public static GameObject MouseRaycastSelecter()
        {
            RaycastHit2D hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit)
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
