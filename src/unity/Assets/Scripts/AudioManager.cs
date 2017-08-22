using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip buttonClick;
    public AudioClip collectPositivePowerup;
    public AudioClip collectNegativePowerup;
    public AudioClip collectTarget;
    public AudioClip menu;
    public AudioClip backtrack1;
    public AudioClip backtrack2;
    public AudioClip backtrack3;

    public AudioClip nativeFadeInClip;
    public AudioClip nativeFadeOutClip;
    public AudioClip nativeInteractClip;
    public AudioClip nativeRejectClip;
    public AudioClip nativeSelectClip;

    public enum SOUNDS
    {
        BUTTON_CLICK,
        COLLECT_POSITIVE_POWERUP,
        COLLECT_NEGATIVE_POWERUP,
        COLLECT_TARGET,
        MENU,
        BACKTRACK1,
        BACKTRACK2,
        BACKTRACK3,

        NATIVE_FADE_IN,
        NATIVE_FADE_OUT,
        NATIVE_INTERACT,
        NATIVE_REJECT,
        NATIVE_SELECT
    }

    private AudioSource mSource;
    private AudioSource mLoopSource;
    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<AudioManager>();
            }
            return _Instance;
        }
    }

    void Start()
    {
        mLoopSource = (GetComponent<AudioSource>() == null) ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        mSource = (GetComponent<AudioSource>() == null) ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        mLoopSource.loop = true;
        mLoopSource.volume = 0.15f;
        mLoopSource.playOnAwake = true;
        

    }

    public void play(SOUNDS sound)
    {
        if (mSource == null)
        {
            return;
        }
        switch (sound)
        {
            case SOUNDS.BUTTON_CLICK:
                mSource.PlayOneShot(buttonClick);
                break;
            case SOUNDS.COLLECT_POSITIVE_POWERUP:
                mSource.PlayOneShot(collectPositivePowerup);
                break;
            case SOUNDS.COLLECT_NEGATIVE_POWERUP:
                mSource.PlayOneShot(collectNegativePowerup,0.2f);
                break;
            case SOUNDS.COLLECT_TARGET:
                mSource.PlayOneShot(collectTarget);
                break;
            case SOUNDS.MENU:
                mLoopSource.clip = menu;
                mLoopSource.Play();
                break;
            case SOUNDS.BACKTRACK1:
                mLoopSource.clip = backtrack1;
                mLoopSource.Play();
                break;
            case SOUNDS.BACKTRACK2:
                mLoopSource.clip = backtrack2;
                mLoopSource.Play();
                break;
            case SOUNDS.BACKTRACK3:
                mLoopSource.clip = backtrack3;
                mLoopSource.Play();
                break;
            case SOUNDS.NATIVE_FADE_IN:
                mSource.PlayOneShot(nativeFadeInClip);
                break;
            case SOUNDS.NATIVE_FADE_OUT:
                mSource.PlayOneShot(nativeFadeOutClip);
                break;
            case SOUNDS.NATIVE_INTERACT:
                mSource.PlayOneShot(nativeInteractClip);
                break;
            case SOUNDS.NATIVE_REJECT:
                mSource.PlayOneShot(nativeRejectClip);
                break;
            case SOUNDS.NATIVE_SELECT:
                mSource.PlayOneShot(nativeSelectClip);
                break;
        }
    }

    public void stop()
    {
        if (mSource == null)
        {
            return ;
        }
            if (mLoopSource.isPlaying)
            {
                mLoopSource.Stop();
            }
         
    }
}
