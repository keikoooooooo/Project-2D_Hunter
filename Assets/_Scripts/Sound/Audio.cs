using UnityEngine;
// debug
public class Audio : MonoBehaviour
{
    [SerializeField] AudioName soundName;

    public void Play() => AudioManager.Instance.Play(soundName);
    public void Play(AudioName audioName) => AudioManager.Instance.Play(audioName);

}
