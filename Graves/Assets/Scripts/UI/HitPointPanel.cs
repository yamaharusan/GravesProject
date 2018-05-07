using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Graves
{
    public class HitPointPanel : MonoBehaviour
    {
        public RectTransform HpBar;
        public RectTransform SpBar;

        public Text HpText;
        public Text SpText;

        private int defHpBar;
        private int defSpBar;

        // Use this for initialization
        void Start()
        {
            if (HpBar)
            {
                defHpBar = (int)HpBar.sizeDelta.x;
            }

            if (SpBar)
            {
                defSpBar = (int)SpBar.sizeDelta.x;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (CharacterPlayer.main)
            {
                CharacterPlayer player = CharacterPlayer.main;

                if (player.MaxHitPoint > 0 && player.MaxStaminaPoint > 0)
                {
                    float hpp = ((float)player.HitPoint / (float)player.MaxHitPoint) * (float)defHpBar;
                    float spp = ((float)player.StaminaPoint / (float)player.MaxStaminaPoint) * (float)defSpBar;

                    if(HpBar)
                        HpBar.sizeDelta = new Vector2(hpp, HpBar.sizeDelta.y);

                    if(SpBar)
                        SpBar.sizeDelta = new Vector2(spp, SpBar.sizeDelta.y);

                    if (HpText)
                        HpText.text = "HP " + player.HitPoint + "/" + player.MaxHitPoint;

                    if (SpText)
                        SpText.text = "SP " + player.StaminaPoint + "/" + player.MaxStaminaPoint;
                }
            }
        }
    }
}