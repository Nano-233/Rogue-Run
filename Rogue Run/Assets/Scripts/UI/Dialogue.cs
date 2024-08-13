using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; //text display of dialogue
    public string[] lines; //lines of dialogue
    public float textSpeed; //speed of text
    public Image avatar;
    public GameObject leftAvatar;
    public GameObject text;

    private PlayerController _controller;

    private AudioSource _source;
    private bool _fade;
    private int _index; //index of text in lines
    private Color _startColor; //of avatar
    private float _timeElapsed;
    private Image _thisImage;
    private Color og; //of ui
    private bool _fadeStarted;

    private bool _fadeIn = true;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        _startColor = avatar.color;
        _controller.StopControls(false);
        AudioSource.PlayClipAtPoint(_source.clip, new Vector3(-50, 0, 0), _source.volume);
    }

    private void Awake()
    {
        _controller = FindObjectOfType<PlayerController>();
        _thisImage = GetComponent<Image>();
        _source = GetComponent<AudioSource>();
        og = _thisImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        //if mousedown
        if (Input.GetMouseButtonDown(0))
        {
            //if done, go to nextline
            if (textComponent.text == lines[_index])
            {
                NextLine();
            }
            //otherwise finish current line
            else
            {
                if (!_fade)
                {
                    StopAllCoroutines();
                }
                
                textComponent.text = lines[_index];
            }
        }

        if (_fadeIn)
        {
            _timeElapsed += Time.deltaTime;
            float newAlpha = _startColor.a *  (_timeElapsed); //fades the sprite
            avatar.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
            if (_timeElapsed > 1)
            {
                _fadeIn = false;
                _timeElapsed = 0;
            }
        }

        if (_fade)
        {
            _thisImage.color = new Color(og.r, og.g, og.b, 0);
            if (!_fadeStarted)
            {
                StartCoroutine(Fade());
            }
            _timeElapsed += Time.deltaTime;
            float newAlpha = _startColor.a *  (1 - _timeElapsed / 1); //fades the sprite
            avatar.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
        }
    }

    private void StartDialogue()
    {
        _index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //puts in characters based on textspeed
        foreach (char c in lines[_index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        //if not end, go to next line
        if (_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        //otherwise close dialogue box
        else
        {
            _fade = true;
            leftAvatar.SetActive(false);
            text.SetActive(false);
            //reset
            textComponent.text = string.Empty;
        }
    }
    
    private IEnumerator Fade()
    {
        AudioSource.PlayClipAtPoint(_source.clip, new Vector3(-50, 0, 0), _source.volume);
        _fadeStarted = true;
        yield return new WaitForSeconds(1);
        _controller.StopControls(true);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}