using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Collider2D col2d;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem ps;
    public Stack stack;

    public Color color {
        set {
            spriteRenderer.color = value;
            var psMain = ps.main;
            psMain.startColor = value;
        }
    }
    public BoxShape shape {
        set {
            switch (value) {
                case BoxShape.square:
                    var scale = Random.Range(0.5f, 1.0f);
                    transform.localScale = new Vector2(scale, scale);
                    break;
                case BoxShape.tall:
                    var width = Random.Range(0.5f, 0.6f);
                    var height = Random.Range(1.25f, 1.4f);
                    transform.localScale = new Vector2(width, height);
                    break;
                case BoxShape.wide:
                    var height2 = Random.Range(0.5f, 0.6f);
                    var width2 = Random.Range(1.25f, 1.4f);
                    transform.localScale = new Vector2(width2, height2);
                    break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        col2d.enabled = false;
        spriteRenderer.enabled = false;
        ps.Play();
        stack.RemoveBox(this);
        Destroy(gameObject, 1);
    }
}

public enum BoxShape {
    wide = 0, tall, square
}
