using Cinemachine;
using System;
using UnityEngine;

public class GamePlayManager : SingletonManager<GamePlayManager>
{
    public event Action<PlayerController> E_ActivePlayer;

    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] GameObject Map1;
    [SerializeField] Transform environment;
    [SerializeField] StartGame startGame;

    [HideInInspector] public PlayerController player;
    [HideInInspector] public int LastUsedCharacterSkin;


    private void OnEnable()
    {
        SpawnEnvironment();
        SpawnPlayer();

        cinemachineCamera.Follow = player.transform;

        if(LoadingManager.Instance != null) LoadingManager.Instance.E_LoadingSuccess += Initialized;

        AudioManager.Instance.Play(AudioName.Gameplay);
    }

    private void OnDestroy()
    {
        if (LoadingManager.Instance != null) LoadingManager.Instance.E_LoadingSuccess -= Initialized;
    }

    private void Initialized()
    {
        startGame.Begin(); // gọi animation bắt đầu game
    }


    private void SpawnEnvironment()
    {
        GameObject map = Instantiate(Map1, Vector3.zero, Quaternion.identity); // tạo map ra scene
        map.transform.SetParent(environment);
    }

    private void SpawnPlayer()
    {
        player = Instantiate(GameManager.Instance.CharactersData.PlayerController, Vector3.zero, Quaternion.identity); // tạo player ra scene
        LastUsedCharacterSkin = GameManager.Instance.CharactersData.LastUsedCharacterSkin;
        player.PlayerAnimation.SetSkin(LastUsedCharacterSkin);  // set skin đã lưu ở lần chơi trước
        player.isPaused = true;
        E_ActivePlayer?.Invoke(player);
    }
    public void SetCameraSize(int value)
    {
        cinemachineCamera.m_Lens.OrthographicSize = value switch
        {
            1 => 7,
            2 => 9,
            3 => 11,
            _ => throw new System.NotImplementedException()
        };
    }



}
