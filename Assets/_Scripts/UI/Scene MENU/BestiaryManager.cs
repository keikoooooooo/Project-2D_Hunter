using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestiaryManager : MonoBehaviour, IData
{
    
    [SerializeField] GameObject panelBestiary;
    [SerializeField] SkeletonGraphic skeletonGraphic;
    Spine.AnimationState animationState;
    [Space(10)]
    [SerializeField] BoxBestiary boxBestiaryPrefab;
    [SerializeField] Transform contentSpawn;
    List<BoxBestiary> boxBestiaryList;

    private BestiaryData bestiaryData;
    Coroutine _iconCoroutine;

    #region Private Methods
    void Awake()
    {
        Initialization();     
        
    }
    void OnEnable()
    {
        DataReference.Register_IData(this);
        _iconCoroutine = StartCoroutine(IconCoroutine());
    }
    void OnDestroy()
    {
        StopCoroutine(_iconCoroutine);
    }

    #endregion



    #region Public Methods
    void Initialization()
    {
        animationState = skeletonGraphic.AnimationState;
    }
    public void GETData(GameManager gameManager)
    {
        bestiaryData = gameManager.BestiaryData;
        SpawnBoxBestiary();
    }
    public void UpdateData()
    {
        foreach (var box in boxBestiaryList)
        {
            box.UpdateData();
        }
    }
    void SpawnBoxBestiary()
    {
        boxBestiaryList = new List<BoxBestiary>();
        foreach (var data in bestiaryData.DataKills)
        {
            BoxBestiary box = Instantiate(boxBestiaryPrefab, contentSpawn);
            box.Initialized(data);
            boxBestiaryList.Add(box);
        }
    }
    IEnumerator IconCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            animationState.SetAnimation(0, "idle", false);
        }
    }


    #endregion


}
