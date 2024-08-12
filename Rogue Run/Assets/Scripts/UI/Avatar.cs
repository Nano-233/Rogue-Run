using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    public Sprite[] sprites;
    public int spritePerFrame = 6;
    public bool loop = true;
    public bool destroyOnEnd = false;

    private int _index = 0;
    private Image _image;
    private int _frame = 0;

    void Awake() {
        _image = GetComponent<Image> ();
    }

    void Update () {
        if (!loop && _index == sprites.Length) return;
        _frame ++;
        if (_frame < spritePerFrame) return;
        _image.sprite = sprites [_index];
        _frame = 0;
        _index ++;
        if (_index >= sprites.Length) {
            if (loop) _index = 0;
            if (destroyOnEnd) Destroy (gameObject);
        }
    }
}
