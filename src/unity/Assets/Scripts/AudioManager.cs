using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip buttonClick;
    public AudioClip collectPositivePowerup;
    public AudioClip collectNegativePowerup;
    public AudioClip collectTarget;
    public AudioClip roundFinished;

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
        ROUND_FINISHED,

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
            case SOUNDS.ROUND_FINISHED:
                mSource.PlayOneShot(roundFinished);
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
}
