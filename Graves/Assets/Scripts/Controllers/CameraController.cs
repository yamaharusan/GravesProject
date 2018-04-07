using UnityEngine;
using System.Collections;

/**
カメラ制御用汎用スクリプト。
    ・いろんなゲームで使いまわしてるから秘伝のタレと化してきている。いつか整理したい。
**/

namespace Graves
{
    public class CameraController : MonoBehaviour
    {
        //汎用関数群
        StdLib Lib = new StdLib();

        //ImageEffectへのアクセッサ
        //public MotionBlur mBlur;
        //public BloomOptimized mBloom;

        public static CameraController main;

        //カメラ
        public Camera _camera;

        //座標保存用
        private Vector3 target = Vector3.zero;
        private Vector3 lTarget = Vector3.zero;
        private Vector3 pos;
        private Vector3 lPos;

        private float smooth = 1.0f;
        private float defs = 0f;

        /**camera effect value**/
        private Vector2 e1 = Vector2.zero;
        private float e1c = 0f;

        // Use this for initialization
        void Start()
        {
            main = this;

            _camera = GetComponent<Camera>();

            pos = gameObject.transform.position;
            lPos = gameObject.transform.position;

            defs = _camera.orthographicSize;
        }

        // Update is called once per frame
        void Update()
        {

            //mBlur.blurAmount = Lib.limit((1f - Main.timeScale) * 2f, 0f, 0.85f);

            if (smooth != 1.0f)
            {
                lTarget = Lib.move(lTarget, target, smooth);
                lPos = Lib.move(lPos, pos, smooth);
            }
            else {
                lTarget = target;
                lPos = pos;
            }

            Effects();
            transform.position = lPos + (Vector3)e1;
            //transform.LookAt(lTarget);

            smooth = 1.0f;
        }

        void Effects()
        {
            if (e1c > 0f)
            {
                e1c -= Time.deltaTime;
                e1 = Lib.move(e1, new Vector2(Random.Range(-e1c, e1c), Random.Range(-e1c, e1c)) / 1.5f, 1.2f);
            }
            else {
                e1 = Lib.move(e1, Vector2.zero, 1.2f);
            }

            /*
            if (e2c > 0f)
            {
                e2c -= Time.deltaTime;
                mBloom.intensity = Lib.move(mBloom.intensity, 1f, 1f);
            }
            else {
                mBloom.intensity = 0f;
            }
            */

            if (e3c > 0f)
            {
                e3c -= Time.deltaTime;
                _camera.orthographicSize = Lib.move(_camera.orthographicSize, defs + 0.2f, 1.1f);
            }
            else {

                _camera.orthographicSize = Lib.move(_camera.orthographicSize, defs, 1.2f);
            }
        }

        public void effect1(float t)
        {
            e1c = t;
        }

        //float e2c = 0f;
        public void effect2(float t)
        {
            //e2c = t;
        }

        float e3c = 0f;
        public void effect3(float t)
        {
            e3c = t;
        }

        public void setOrthographicSize(float n)
        {
            defs = n;
        }

        public void setSmooth(float s)
        {
            smooth = s;
        }

        public void look(Vector3 t, Vector3 p)
        {
            target = t;
            pos = p;
        }

        public void look(Vector3 t, float ax, float ay, float len)
        {
            Vector3 r = Quaternion.Euler(-ax, ay, 0) * Vector3.forward;
            target = t;
            pos = t + (r * len);
        }
    }
}