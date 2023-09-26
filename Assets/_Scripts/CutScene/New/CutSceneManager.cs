using UnityEngine;

public class CutSceneManager : SingletonManager<CutSceneManager>
{

    [SerializeField] CutSceneIconAnimation cutSceneIconAnimation;
    [SerializeField] Audio _audio;
    public CutScenePlayer player;
    public CutSceneEnemy enemy1;
    public CutSceneEnemy enemy2;

    public GameObject Trigger1, Trigger2;


    private void Start()
    {
        enemy1.E_EnemyDie += SpawnEnemy2;
        _audio.Play();
    }

    private void OnDestroy()
    {
        enemy1.E_EnemyDie -= SpawnEnemy2;
    }


    public void SpawnEnemy2()
    {
        var fx_Circle = SpawnVFX.Instance.Get_CircleFX(enemy2.transform.position);
        fx_Circle.E_EndCircleEffect += GetEnemy;
    }


    private void GetEnemy(Fx_Circle fx)
    {
        enemy2.gameObject.SetActive(true);
        fx.E_EndCircleEffect -= GetEnemy;
        enemy2.E_EnemyDie += EndManuals;
    }

    private void EndManuals()
    {
        Debug.Log("hết hướng dẫn");
        _audio.Play(AudioName.Reward);
        cutSceneIconAnimation.EnablePanelTextGreat();
        cutSceneIconAnimation.PlayAnimation(CutSceneAnimationName.Default);
        enemy2.E_EnemyDie -= EndManuals;
        Invoke(nameof(SkipManuals), 1f);
    }
    public void HandlerEnemy1Die()
    {

        Trigger1.SetActive(false);
        Trigger2.SetActive(true);
    }

    public void CheckTrigger()
    {
        if(enemy1.gameObject.activeSelf)
        {
            Trigger1.SetActive(true); 
            Trigger2.SetActive(false);
        }
        else
        {
            Trigger1.SetActive(false);
            Trigger2.SetActive(true);
        }
    }



    public void SkipManuals() => LoadingManager.Instance.LoadScene("MenuGame");


}
