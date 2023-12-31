﻿using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestiaryManager : MonoBehaviour, IData
{
    
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    private Spine.AnimationState animationState;
    [Space(10)]
    [SerializeField]
    private BoxBestiary boxBestiaryPrefab;
    [SerializeField] private Transform contentSpawn;
    private List<BoxBestiary> boxBestiaryList;

    private BestiaryData bestiaryData;
    private Coroutine _iconCoroutine;

    #region Private Methods

    private void Awake()
    {
        Initialization();     
        
    }

    private void OnEnable()
    {
        DataReference.Register_IData(this);
        _iconCoroutine = StartCoroutine(IconCoroutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(_iconCoroutine);
    }

    #endregion



    #region Public Methods

    private void Initialization()
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

    private void SpawnBoxBestiary()
    {
        boxBestiaryList = new List<BoxBestiary>();
        foreach (var data in bestiaryData.DataKills)
        {
            BoxBestiary box = Instantiate(boxBestiaryPrefab, contentSpawn);
            box.Initialized(data);
            boxBestiaryList.Add(box);
        }
    }

    private IEnumerator IconCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            animationState.SetAnimation(0, "idle", false);
        }
    }


    #endregion


}
