using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class PopTextController : MonoBehaviour
    {
        public static PopTextController main;

        public GameObject Text;

        private void Start()
        {
            main = this;
        }

        public PopText print(string str,Vector3 pos)
        {
            if (Text)
            {
                GameObject obj = Instantiate(Text);
                PopText pText = obj.GetComponent<PopText>();

                if (pText)
                {
                    obj.transform.SetParent(transform);
                    obj.transform.localScale = Vector3.one;
                    pText.SetEtc(str,pos);
                    return pText;
                }
                else
                {
                    Destroy(obj);
                    return null;
                }
            }
            return null;
        }

    }
}
