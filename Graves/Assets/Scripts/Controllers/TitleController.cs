using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Graves
{
    public class TitleController : MonoBehaviour
    {
        public Animator UiBlackOut;

        void Start()
        {
            UiBlackOut.SetBool("IsActive", false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                UiBlackOut.SetBool("IsActive", true);
                StartCoroutine(C_LoadScene());
            }
        }

        protected IEnumerator C_LoadScene()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("test");
        }
    }
}