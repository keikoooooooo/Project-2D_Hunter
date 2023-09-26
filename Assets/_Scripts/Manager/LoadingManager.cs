using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Delegates;

public class LoadingManager : SingletonManager<LoadingManager>
{
    public event LoadingScreenSuccessfully E_LoadingSuccess;

    [SerializeField] private GameObject panelLoading;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI textAnim;
    [SerializeField] private TextMeshProUGUI textProgress;

    private float _target;
    private Coroutine _fillCoroutine;


    public async void LoadScene(string sceneName)
    {
        panelLoading.SetActive(true);
        _target = 0;
        fill.fillAmount = 0;

        if(_fillCoroutine != null)  StopCoroutine(_fillCoroutine);
        _fillCoroutine = StartCoroutine(FillCoroutine());

        DataReference.ClearData();
        var scene = SceneManager.LoadSceneAsync(sceneName);
        do
        {
            await Task.Delay(500);
            _target = scene.progress;
        } while (scene.progress < .9f);
        fill.DOFillAmount(_target, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            fill.DOFillAmount(1f, 2f).SetEase(Ease.Linear);
        });

        await Task.Delay(2300);
        DOTween.Kill(fill);
        fill.fillAmount = 0;
        panelLoading.SetActive(false);
        E_LoadingSuccess?.Invoke(); // gọi event khi load xong
    }

    private IEnumerator FillCoroutine()
    {
        int _temp = 0;
        while (true)
        {
            textProgress.text = $"{Mathf.FloorToInt(fill.fillAmount * 100)}%";
            textAnim.text = _temp switch
            {
                1 => $".",
                2 => $"..",
                3 => $"...",
                _ => $""
            };

            _temp += 1;
            if (_temp > 3) _temp = 0;
            if(fill.fillAmount >= 1f) StopCoroutine(_fillCoroutine);

            yield return new WaitForSeconds(.08f);
        }
    }

}
