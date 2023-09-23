using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxKilled : MonoBehaviour, IPool<BoxKilled> 
{
    public Image avatarPlayer;
    public Image avaterEnemy;

    PlayerController _player;
    void Start()
    {
        _player = GamePlayManager.Instance.player;
        avatarPlayer.sprite = _player.stats_SO.Information.Skins[0].Sprite;
    }

    Action<BoxKilled> action;   
    public void Init(Action <BoxKilled> action) => this.action = action;
    public void SetStats(Sprite Enemy)
    {
        avaterEnemy.sprite = Enemy;
    }

    public void Action() => action(this);
}
