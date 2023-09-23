using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxAvatarUser : MonoBehaviour, IPool<BoxAvatarUser>
{
    [SerializeField] Button bttSelect;
    [SerializeField] Image avatar;

    int index;

    public void Init(Action<BoxAvatarUser> returnAction) {}


    public void SetStats(int idx, Sprite sprite)
    {
        index = idx;
        avatar.sprite = sprite;
    }


    public void OnlickSelectAvatar()
    {
        GameManager.Instance.UserData.LastAvatarIndex = index;
        ProfileManager.Instance.avatar.sprite = avatar.sprite;
        MenuGameManager.Instance.avatar.sprite = avatar.sprite;
    }


}
