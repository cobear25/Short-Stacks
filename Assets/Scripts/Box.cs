using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Collider2D col2d;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem ps;
    public Stack stack;
    public Rigidbody2D rigidbody2d;
    public Transform boxTop;
    public AudioSource audio;
    public AudioSource boxAudio;

    bool boxSoundEnabled = true;

    private BoxColor _color;
    public BoxColor color {
        set {
            spriteRenderer.color = stack.colors[(int)value];
            var psMain = ps.main;
            psMain.startColor = stack.colors[(int)value];
            _color = value;
        }

        get {
            return _color;
        }
    }
    private BoxShape _shape;

    public BoxShape shape {
        set {
            switch (value) {
                case BoxShape.square:
                    var scale = Random.Range(0.5f, 1.0f);
                    if (transform != null)
                        transform.localScale = new Vector2(scale, scale);
                    break;
                case BoxShape.tall:
                    var width = Random.Range(0.5f, 0.6f);
                    var height = Random.Range(1.0f, 1.25f);
                    if (transform != null)
                        transform.localScale = new Vector2(width, height);
                    break;
                case BoxShape.wide:
                    var height2 = Random.Range(0.5f, 0.6f);
                    var width2 = Random.Range(1.25f, 1.4f);
                    if (transform != null)
                        transform.localScale = new Vector2(width2, height2);
                    break;
            }
            _shape = value;
        }

        get {
            return _shape;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z = euler.z - 360;
        euler.z = Mathf.Clamp(euler.z, -25, 25);
        transform.eulerAngles = euler;
    }

    void UnFreezeRotation() {
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void Pop() {
        audio.Play();
        col2d.enabled = false;
        spriteRenderer.enabled = false;
        ps.Play();
        stack.RemoveBox(this);
        stack.CheckRules();
        Destroy(gameObject, 1);
    }

    void OnMouseDown() {
        Pop();
    }

    public bool HasRequirementType(StackRequirementType reqType) {
        bool hasType = false;
        switch (reqType) {
            case StackRequirementType.darkBrown:
                if (_color == BoxColor.darkBrown) {
                    hasType = true;
                }
                break;
            case StackRequirementType.lightBrown:
                if (_color == BoxColor.lightBrown) {
                    hasType = true;
                }
                break;
            case StackRequirementType.pink:
                if (_color == BoxColor.pink) {
                    hasType = true;
                }
                break;
            case StackRequirementType.purple:
                if (_color == BoxColor.purple) {
                    hasType = true;
                }
                break;
            case StackRequirementType.yellow:
                if (_color == BoxColor.yellow) {
                    hasType = true;
                }
                break;
            case StackRequirementType.square:
                if (_shape == BoxShape.square) {
                    hasType = true;
                }
                break;
            case StackRequirementType.wide:
                if (_shape == BoxShape.wide) {
                    hasType = true;
                }
                break;
            case StackRequirementType.tall:
                if (_shape == BoxShape.tall) {
                    hasType = true;
                }
                break;
        }
        return hasType;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        float impulse = 0F;

        foreach (ContactPoint2D point in col.contacts)
        {
            impulse += point.normalImpulse;
        }
        Debug.Log(impulse);
        if (impulse > 20 && boxSoundEnabled) {
            boxAudio.volume = Mathf.Min(impulse / 150f, 1f);
            boxAudio.Play();
            boxSoundEnabled = false;
            Invoke("EnableBoxSound", 0.3f);
        }
        stack.CheckRules();
    }

    void EnableBoxSound() {
        boxSoundEnabled = true;
    }
}

public enum BoxShape {
    wide = 0, tall, square
}

public enum BoxColor {
    darkBrown = 0, lightBrown, yellow, purple, pink
}

public struct BoxData {
    public BoxColor color;
    public BoxShape shape;
}