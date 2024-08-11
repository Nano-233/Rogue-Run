using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroClip : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;

    private bool _firstStop = false;

    private Color _startColor;
    private float _timeElapsed;

    public RawImage img;
    // Start is called before the first frame update
    void Start()
    {
        _startColor = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying)
        {
            if (_firstStop)
            {
                StartCoroutine(Fade());
                _timeElapsed += Time.deltaTime;
                float newAlpha = _startColor.a *  (1 - _timeElapsed / 1); //fades the sprite
                img.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
            }
        }
        else
        {
            _firstStop = true;
            
        }
    }
    
    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
