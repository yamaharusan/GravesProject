using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
・取り敢えずベースではプラーンとした抜け殻を創れるようにする

・コアから分岐する形で、子を枝葉のように生やしていく。

・親1個、子0~n個(座標、ジョイントの種類)
    ・子の無いオブジェクトはTargetJointをつける事ができる。

・命名規則
    ・

**/
namespace Graves
{
    public class PartsBase : MonoBehaviour
    {
        #region PublicMember

        /**キャラクター管理クラス**/
        [System.NonSerialized]
        public CharacterBase ParentCharacter;

        /**ルートのパーツオブジェクト**/
        [System.NonSerialized]
        public PartsBase RootParts;

        /**子パーツオブジェクト**/
        [System.NonSerialized]
        public List<PartsBase> ChildParts = new List<PartsBase>();

        /**自分の役割　手や足など**/
        public enum PartCategory
        {
            Core,
            Torso,//トルソー　胴体
            Hand,
            Leg,
            None = -1
        }
        public PartCategory MyPartCategory = PartCategory.None;

        /**HitPoint**/
        public int HitPoint = 0;

        #endregion

        #region PrivateMember



        #endregion

        #region Main

        // Use this for initialization
        private void Start()
        {
            Initialization();
        }

        // Update is called once per frame
        private void Update()
        {

        }

        //初期設定
        private void Initialization()
        {

        }

        #endregion

        #region Utility



        #endregion
    }
}
