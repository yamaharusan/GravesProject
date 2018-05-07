using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graves
{
    public class Grave : MonoBehaviour {

        public static Grave main;

        public List<PartsBase> Parts = new List<PartsBase>();

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
        public void BurialBody(PartsBase body)
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
                PartsBase p = t.gameObject.GetComponent<PartsBase>();

                if (p)
                {
                    if (p.MyTargetJoint)
                    {
                        if(p.MyPartCategory == PartsBase.PartCategory.Hand)
                            p.MyTargetPosition = new Vector2(Random.Range(-0.3f, 0.3f), 0.5f);
                        else if (p.MyPartCategory == PartsBase.PartCategory.Leg)
                            p.MyTargetPosition = new Vector2(Random.Range(-0.3f, 0.3f), 0f);
                    }

                    PartsTorso torso = t.gameObject.GetComponent<PartsTorso>();
                    if (torso)
                    {
                        if (torso.MyPartCategory == PartsBase.PartCategory.Hand || torso.MyPartCategory == PartsBase.PartCategory.Leg)
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
