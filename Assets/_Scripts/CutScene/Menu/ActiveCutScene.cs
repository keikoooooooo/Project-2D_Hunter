using UnityEngine;

public class ActiveCutScene : SingletonManager<ActiveCutScene>, IData
{
    public GameObject panelCutScene;
    bool isNewAccount = false;


    void OnEnable()
    {
        DataReference.Register_IData(this);
    }


    public void GETData(GameManager gameManager)
    {
        isNewAccount = gameManager.UserData.isNewAccount;
        UpdateData();
    }

    public void UpdateData()
    {
        if(isNewAccount)
        {
            panelCutScene.SetActive(true);
            GameManager.Instance.UserData.isNewAccount = false;
        }
    }

    public void UnRegisterIData() => DataReference.Unregister_IData(this);

}
