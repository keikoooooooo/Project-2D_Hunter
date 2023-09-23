using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    [Header("----------- Audio Source -----------")]
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    [Header("----------- Audio Clip -----------")]
    [SerializeField] AudioClip Menu;
    [SerializeField] AudioClip Gameplay;
    [SerializeField] AudioClip OnClick;
    [SerializeField] AudioClip BackOnClick;
    [SerializeField] AudioClip Defeat;
    [SerializeField] AudioClip Victory;
    [SerializeField] AudioClip Reward;
    [SerializeField] AudioClip Upgrade;
    [SerializeField] AudioClip Error;
    [SerializeField] AudioClip Collect;

    public void Play(AudioName audioName)
    {
        switch (audioName)
        {
            // Music
            case AudioName.Menu:
                MusicSource.clip = Menu;
                MusicSource.Play();
                break;
            case AudioName.Gameplay:
                MusicSource.clip = Gameplay;
                MusicSource.Play();
                break;
            // SFX
            case AudioName.OnClick:
                SFXSource.PlayOneShot(OnClick);
                break;
            case AudioName.BackOnClick:
                SFXSource.PlayOneShot(BackOnClick);
                break;
            case AudioName.Defeat:
                SFXSource.PlayOneShot(Defeat);
                break;
            case AudioName.Victory:
                SFXSource.PlayOneShot(Victory);
                break;
            case AudioName.Reward:
                SFXSource.PlayOneShot(Reward);
                break;
            case AudioName.Upgrade:
                SFXSource.PlayOneShot(Upgrade);
                break;
            case AudioName.Error:
                SFXSource.PlayOneShot(Error);
                break;
            case AudioName.Collect:
                SFXSource.PlayOneShot(Collect);
                break;
        }
    }
    public void Mute(AudioMode audioMode, bool isMute)
    {
        switch (audioMode)
        {
            case AudioMode.Music:
                MusicSource.mute = isMute;
                break;
            case AudioMode.SFX:
                SFXSource.mute = isMute;
                break;
        }
    }


}

public enum AudioName
{
    Menu,
    Gameplay,
    OnClick,
    BackOnClick,
    Defeat,
    Victory,
    Reward,
    Upgrade,
    Error,
    Collect
}
public enum AudioMode
{
    Music,
    SFX
}

