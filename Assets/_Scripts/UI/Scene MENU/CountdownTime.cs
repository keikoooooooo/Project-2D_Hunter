using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTime : MonoBehaviour
{   
    public event Action<int> E_TimeCount; // thời gian đếm còn lại
    public event Action E_EndCountdownTime;

    public GameObject panelCountdown;
    [SerializeField] TextMeshProUGUI textTime;

    [SerializeField] int CountTime;
    public int _countTime { get; private set; }


    [Header("Format 1: 00M:00S")]
    public bool Format1;
    [Header("Format 2: 00H:00M:00S")]
    public bool Format2;



    Coroutine countdownTimeCoroutine;
    IEnumerator CountdownCoroutine()
    {
        _countTime = CountTime;
        while (_countTime >= 0)
        {
            int hours   = Mathf.FloorToInt(_countTime / 3600);
            int minutes = Mathf.FloorToInt(_countTime / 60) % 60;
            int seconds = Mathf.FloorToInt(_countTime % 60);

            if(Format1)         textTime.text = string.Format("{00:00}:{01:00}", minutes, seconds);
            else if (Format2)   textTime.text = string.Format("{00:00}:{01:00}:{02:00}", hours, minutes, seconds);
            else                textTime.text = string.Format("{00:00}:{01:00}", minutes, seconds);
        
            _countTime -= 1;
            E_TimeCount?.Invoke(_countTime);
            yield return new WaitForSeconds(1f);
        }
        E_EndCountdownTime?.Invoke();
    }


    public void StartCountDown()
    {
        panelCountdown.SetActive(true);
        if (countdownTimeCoroutine != null && gameObject.activeSelf) StopCoroutine(countdownTimeCoroutine);
        countdownTimeCoroutine = StartCoroutine(CountdownCoroutine());
    }
    public void StartCountDown(int value)
    {
        panelCountdown.SetActive(true);
        CountTime = value;
        if (countdownTimeCoroutine != null && gameObject.activeSelf) StopCoroutine(countdownTimeCoroutine);
        countdownTimeCoroutine = StartCoroutine(CountdownCoroutine());
    }
    public void ResetCoroutine()
    {
        _countTime = CountTime;
    }
    public void StopCountDown()
    {
        if(countdownTimeCoroutine != null && gameObject.activeSelf) StopCoroutine(countdownTimeCoroutine);
        panelCountdown.SetActive(false);
    }


}

