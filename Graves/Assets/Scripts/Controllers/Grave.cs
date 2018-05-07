using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Graves.Character;

namespace Graves
{
    public class Grave : MonoBehaviour {

        public static Grave main;

        public List<Character.Parts.Base> Parts = new List<Character.Parts.Base>();

        void Awake()
        {
            main = this;
        }

	    // Use this for initialization
	    void Start ()
        {
		    
	    }
	
	    // Update is called once per frame
	    void Update ()
        {
		
	    }

        //
        public void BurialBody(Character.Parts.Base body)
        {
            GameObject b = Instantiate(body.gameObject);
            b.transform.parent = transform;
            b.transform.position = transform.position;

            b.SetActive(false);

            SearchParts(b.transform);

            Destroy(b);
        }

        //Partさがす
        protected void SearchParts(Transform _t)
        {
            foreach (Transform t in _t)
            {
                Character.Parts.Base p = t.gameObject.GetComponent<Character.Parts.Base>();

                if (p)
                {
                    if (p.MyTargetJoint)
                    {
                        if(p.MyPartCategory == Character.Parts.Base.PartCategory.Hand)
                            p.MyTargetPosition = new Vector2(Random.Range(-0.3f, 0.3f), 0.5f);
                        else if (p.MyPartCategory == Character.Parts.Base.PartCategory.Leg)
                            p.MyTargetPosition = new Vector2(Random.Range(-0.3f, 0.3f), 0f);
                    }

                    Character.Parts.Torso torso = t.gameObject.GetComponent<Character.Parts.Torso>();
                    if (torso)
                    {
                        if (torso.MyPartCategory == Character.Parts.Base.PartCategory.Hand || torso.MyPartCategory == Character.Parts.Base.PartCategory.Leg)
                        {
                            torso.transform.parent = transform;
                            torso.gameObject.SetActive(false);
                            Parts.Add(torso);
                        }
                    }
                    else
                    {
                        SearchParts(t);
                    }
                }
            }
        }

        //
    }
}
