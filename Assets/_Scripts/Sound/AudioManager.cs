using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    [Header("----------- Audio Source -----------")]
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    [Header("----------- Audio Clip -----------")]
    [SerializeField] private AudioClip Menu;
    [SerializeField] private AudioClip Gameplay;
    [SerializeField] private AudioClip OnClick;
    [SerializeField] private AudioClip BackOnClick;
    [SerializeField] private AudioClip Defeat;
    [SerializeField] private AudioClip Victory;
    [SerializeField] private AudioClip Reward;
    [SerializeField] private AudioClip Upgrade;
    [SerializeField] private AudioClip Error;
    [SerializeField] private AudioClip Collect;

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

