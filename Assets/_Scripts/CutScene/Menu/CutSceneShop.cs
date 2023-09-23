using UnityEngine;

public class CutSceneShop : MonoBehaviour
{
    public ButtonClaimReward bttBuyCharacter;
    public PlayerController playerController;
    string playerName;

    private void Start()
    {
        playerName = playerController.stats_SO.Information.CharacterName;
    }


    public void OnClickBuyCharacterButton()
    {
        GameManager.Instance.CharactersData.UnLockCharacter(playerName);
        playerController.stats_SO.IncreaseUpgradePoint(5);
        bttBuyCharacter.SetState(playerController.stats_SO.Information.Skins[0].Sprite, 1);

    }


}
