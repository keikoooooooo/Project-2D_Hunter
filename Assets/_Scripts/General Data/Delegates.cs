using UnityEngine;

public class Delegates 
{
    
    public delegate void HealthChangedEventHandler(int health); // khi máu trong status thay đổi ?

    public delegate void TextEventHandler(TextHandler textHandler, Vector3 pos, int amount);

    public delegate void AbilitySelectEventHandler(AbilityType abilityType); // khi user chọn 1 ability trong gameplay ?

    public delegate void LoadingScreenSuccessfully();

}
