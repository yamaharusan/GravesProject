using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using Graves.Character;

/**
    キャラクターの基本的なスケール感として10cm以下を想定


**/

namespace Graves
{
    public class Main : MonoBehaviour
    {
        public static bool IsGameOver = false;

        public readonly static int enemyCountMax = 108;
        public static int enemyCount = 0;

        public Animator UiBlackOut;
        public GameObject UICanvas;

        public GameObject GameOverText;

        public Text EnemyText;

        // Use this for initialization
        void Awake()
        {
            IsGameOver = false;
            enemyCount = 0;

            EnemyTest.list.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if ((!Player.main.IsLive && !IsGameOver) || enemyCount == enemyCountMax)
            {
                IsGameOver = true;
                StartCoroutine(C_LoadScene());
            }

            if (EnemyText)
            {
                EnemyText.text = enemyCount.ToString("000") + "/" + enemyCountMax;
            }
        }

        public void Test()
        {

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

        protected IEnumerator C_LoadScene()
        {
            UICanvas.SetActive(false);
            yield return new WaitForSeconds(6f);
            UiBlackOut.SetBool("IsActive", true);

            yield return new WaitForSeconds(1f);
            GameOverText.SetActive(true);

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Title");
        }
    }
}
