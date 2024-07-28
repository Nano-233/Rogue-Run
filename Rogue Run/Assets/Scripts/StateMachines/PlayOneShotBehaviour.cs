using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false, playWhileLoop = false;
    
    //delay timer
    public float playDelay = 0.3f;
    public float loopDelay = 0.2f;
    private float _timeSinceEntered = 0;
    private float _timeSinceLooped = 0;
    private bool _delaySoundPlayed = false;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //plays the sound clip on entering the state
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }

        _timeSinceEntered = 0f;
        _delaySoundPlayed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //plays clip after a delay
        if (playAfterDelay && !_delaySoundPlayed)
        {
            _timeSinceEntered += Time.deltaTime;
            if (_timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                _delaySoundPlayed = true;
            }
        }

        if (playWhileLoop)
        {
            _timeSinceLooped += Time.deltaTime;
            if (_timeSinceLooped > loopDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                _timeSinceLooped = 0f;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //plays the sound clip on exiting the state
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }
    }

}
