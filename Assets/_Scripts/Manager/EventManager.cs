using System;

public static class EventManager 
{
    private static readonly int count = Enum.GetNames(typeof(EventName)).Length; 

    private static readonly Action[] events = new Action[count]; // tạo 1 mảng có số phần tử = phần tử trong enum


    public static void CallEvent(EventName eventName) => events[(int)eventName]?.Invoke(); // gọi event

    public static void SubscribeEvent(EventName eventName, Action action) => events[(int)eventName] += action; // đăng kí event 
    public static void UnSubscribeEvent(EventName eventName, Action action) => events[(int)eventName] -= action; // xóa event
}


public enum EventName
{    
    // Button Menu Game



}