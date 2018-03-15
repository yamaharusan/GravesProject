using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Graves
{
    public class PopText : MonoBehaviour
    {
        //private StdLib lib = new StdLib();

        private RectTransform rect;
        public Text text;

        //private Vector3 position = Vector3.zero;

        [System.NonSerialized]
        public float lifeSpan = 0.5f;

        private void Start()
        {
            StartCoroutine(FadeText());
        }

        // Update is called once per frame
        private void Update()
        {
            if (rect)
            {
                rect.position += Vector3.up * Time.deltaTime * 25f;
            }
        }

        private IEnumerator FadeText()
        {
            yield return new WaitForSeconds(lifeSpan);

            while (true)
            {
                if (text) {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * 2f);
                }

                if (text.color.a <= 0f)
                {
                    Destroy(gameObject);
                }

                yield return null;
            }
        }

        /****/
        public void SetEtc(string str,Vector3 position)
        {
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

            rect = GetComponent<RectTransform>();
            if (rect)
            {
                rect.position = pos;
                position = pos;
            }

            text = GetComponent<Text>();
            if (text)
                text.text = str;
        }
    }
}
