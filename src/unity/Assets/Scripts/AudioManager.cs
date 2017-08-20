using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip buttonClick;
    public AudioClip collectPositivePowerup;
    public AudioClip collectNegativePowerup;
    public AudioClip collectTarget;
    public AudioClip menu;
    public AudioClip backtrack;

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
        BACKTRACK,

        NATIVE_FADE_IN,
        NATIVE_FADE_OUT,
        NATIVE_INTERACT,
        NATIVE_REJECT,
        NATIVE_SELECT
    }

    private AudioSource mSource;
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
        mSource = (GetComponent<AudioSource>() == null) ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
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
                mSource.PlayOneShot(collectNegativePowerup);
                break;
            case SOUNDS.COLLECT_TARGET:
                mSource.PlayOneShot(collectTarget);
                break;
            case SOUNDS.MENU:
                mSource.PlayOneShot(menu);
                //TODO: schleife!!! Also Song hört nach 7 min auf
                break;
            case SOUNDS.BACKTRACK:
                mSource.PlayOneShot(backtrack, 0.2f);
                //TODO: schleife!!! Also Song hört nach 5 min auf
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
            if (mSource.isPlaying)
            {
                mSource.Stop();
            }
         
    }
}
