using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Graves
{
    public class AnimaBase : MonoBehaviour {
        private Texture2D m_texture;
        private Sprite m_texture_sprite;
        private SpriteRenderer m_spriteRenderer;

        [SerializeField,Range(0f,1f)]
        float m_sin = 0f, m_cos = 1f, m_tan = 1f, m_scale = 0f;

        private void Start() {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            CreateTexture( new Vector2( 0f , 0f ), m_sin, m_cos, m_tan, m_scale);
        }

        private void CreateTexture(Vector2 a, float sin, float cos, float tan, float v)
        {
            Destroy(m_texture);
            m_texture = new Texture2D(320, 160, TextureFormat.ARGB32, false);

            for (int y = 0; y < m_texture.height; y++)
            {
                for (int x = 0; x < m_texture.width; x++)
                {
                    float lx = x - m_texture.width / 2, ly = y - m_texture.height / 2;

                    float c = (Mathf.PerlinNoise((float)lx * sin + a.x, (float)ly * sin + a.y)) * v;

                    float r = ( 1f / (Mathf.Sin((lx + a.x) * sin * c) * 3f * sin + Mathf.Cos((ly + a.y) * cos * c) * 2f * cos + Mathf.Tan((lx + a.x) * (ly + a.y) * tan * c) * 1f * tan) ) * v;
                    float g = ( 1f / (Mathf.Sin((lx + a.x) * sin * c) * 2f * sin + Mathf.Cos((ly + a.y) * cos * c) * 1f * cos + Mathf.Tan((lx + a.x) * (ly + a.y) * tan * c) * 3f * tan) ) * v;
                    float b = ( 1f / (Mathf.Sin((lx + a.x) * sin * c) * 1f * sin + Mathf.Cos((ly + a.y) * cos * c) * 3f * cos + Mathf.Tan((lx + a.x) * (ly + a.y) * tan * c) * 2f * tan) ) * v;

                    Color col = new Color(r, g, b, 1f);

                    m_texture.SetPixel(x, y, col);
                }
            }

            m_texture.Apply();
            m_texture.filterMode = FilterMode.Point;

            m_texture_sprite = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), new Vector2(0.5f, 0.5f));
            m_spriteRenderer.sprite = m_texture_sprite;
        }
    }
}