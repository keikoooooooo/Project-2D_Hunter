﻿using UnityEngine;


public class SpawnVFX : SingletonManager<SpawnVFX>
{

    [Space][SerializeField] Fx_Text textHandlerPrefab; // text damge, heal, +xp
    [Space][SerializeField] Fx_Partical fxEnemyDiePrefab;    // fx khi 1 enemy chết
    [Space][SerializeField] Fx_Circle circlePrefab; // fx khi chuẩn bị spawn 1 enemy 
    [Space][SerializeField] Fx_XP xpItemPrefab; // fx item khi tiêu diệt enemy
    [Space][SerializeField] Fx_Partical fxPlayerDiePrefab; // fx khi 1 player chết

    ObjectPool<Fx_Text> poolTextHandler;
    ObjectPool<Fx_Partical> poolFxEnemyDie;
    ObjectPool<Fx_Circle> poolCircleSpawn;
    ObjectPool<Fx_XP> poolXPItem;
    ObjectPool<Fx_Partical> poolFxPlayerDie;

    void OnEnable() => Initialization();

    void Initialization() // khởi tạo ?
    {
        poolTextHandler = new ObjectPool<Fx_Text>(textHandlerPrefab, transform, 1);
        poolFxEnemyDie = new ObjectPool<Fx_Partical>(fxEnemyDiePrefab, transform, 0);
        poolCircleSpawn = new ObjectPool<Fx_Circle>(circlePrefab, transform, 2);
        poolXPItem = new ObjectPool<Fx_XP>(xpItemPrefab, transform, 0);
        poolFxPlayerDie = new ObjectPool<Fx_Partical>(fxPlayerDiePrefab, transform, 1);
    }

    #region Public Methods
    Vector2 RandomPos(Vector2 pos) => new Vector2(Random.Range(pos.x - 1f, pos.x + 1f), Random.Range(pos.y - .3f, pos.y + .3f)); // rand 1 vị trí ngẫu nhiên 

    public Fx_Circle Get_CircleFX(Vector2 pos)
    {
        Fx_Circle fx_Circle = poolCircleSpawn.Get();
        fx_Circle.SetStats(pos);
        return fx_Circle;
    }
    public void Get_TextHandler(TextHandler textHandler, Vector3 pos, float value) => poolTextHandler.Get().SetStats(textHandler, pos, RandomPos(pos), value);

    public void Get_FXEnemyDie(Vector2 pos) // spawn các fx khi enemy die
    {
        poolFxEnemyDie.Get().SetStats(RandomPos(pos));
        poolXPItem.Get().SetStats(pos);
    }
    public Fx_Partical Get_FXPlayerDie(Vector2 pos)
    {
        Fx_Partical fx = poolFxPlayerDie.Get();
        fx.SetStats(pos);
        return fx;
    }

    #endregion


}
