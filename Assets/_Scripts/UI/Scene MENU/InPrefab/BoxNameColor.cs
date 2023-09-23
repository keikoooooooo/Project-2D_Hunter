using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxNameColor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] Color[] colorList;
    [SerializeField] string[] nameColor;
    [SerializeField] TextMeshProUGUI textPrice;
    [SerializeField] Button bttBuy;
    [SerializeField] Audio _audio;

    bool isBuy;
    public Button bttUse;
    public Button bttUsed;
    
    int index;
    string colorCode;

    private readonly List<int> _priceList = new List<int> { 50, 75, 100, 125, 150, 200, 250 };

 
    public void SetStats(int _index, string _textName, string _lastUsedColor, int _gem, bool _isUse)
    {
        index = _index;

        textName.text = _textName;
        textName.color = colorList[_index];
        colorCode = textName.color.ColorToHex();

        bool isCheckBuy = _gem >= _priceList[_index];      // tìm có được mua không, nếu số gem lớn hơn giá -> true
        isBuy = isCheckBuy;      

        textPrice.text = $"{_priceList[_index]}";
        textPrice.color = isCheckBuy ? Color.white : Color.red;

        string colorHex = colorList[_index].ColorToHex(); // tìm lần cuối user dùng color nào

        bttBuy.gameObject.SetActive(colorHex != _lastUsedColor && !_isUse && index != 0);
        bttUse.gameObject.SetActive(colorHex != _lastUsedColor && _isUse && index != 0);
        bttUsed.gameObject.SetActive(colorHex == _lastUsedColor && _isUse && index != 0);
    }

    public void OnClickBuyButton()
    {
        if(!isBuy)
        {
            _audio.Play();
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            MenuGameManager.Instance.PlayGemPriceAnim();
            return;
        }

        _audio.Play(AudioName.Reward);
        bttBuy.gameObject.SetActive(false);
        bttUse.gameObject.SetActive(true);

        GameManager.Instance.UserData.Gem -= _priceList[index];
        GameManager.Instance.UserData.PurchasedColors[index] = colorCode;
        GameManager.Instance.UpdateSingleData(MenuGameManager.Instance);
    }
    public void OnClickUseButton()
    {
        _audio.Play(AudioName.OnClick);

        GameManager.Instance.UserData.LastUsedColor = colorCode;
        GameManager.Instance.UserData.LastNameColor = nameColor[index];

        ProfileManager.Instance.OnClickUseBoxNameColor(colorList[index], nameColor[index]);
    }
    public void OnClickUnUsedButton()
    {
        _audio.Play(AudioName.OnClick);

        string colorCode = colorList[0].ColorToHex();
        GameManager.Instance.UserData.LastUsedColor = colorCode;
        GameManager.Instance.UserData.LastNameColor = nameColor[0];

        ProfileManager.Instance.OnClickUseBoxNameColor(colorCode.HexToColor(), nameColor[0]);
    }


}
