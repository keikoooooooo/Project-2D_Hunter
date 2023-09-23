using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class FileHandler
{
    public static void Save<T> (T Data, FileNameData fileName, bool isEncrypt)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName.ToString());
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string jsonText = JsonUtility.ToJson(Data, true);

        if (isEncrypt)
        {
            string encryptedText = Encrypt(jsonText);
            File.WriteAllText(path, encryptedText);
            return;
        }
        File.WriteAllText(path, jsonText);
    }

    public static T Load<T>(FileNameData fileName, bool isEncrypt)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName.ToString());
        T data = default;

        if (!File.Exists(path)) 
            return data;

        string encryptedText = File.ReadAllText(path);

        if (isEncrypt)
        {
            string jsonText = Decrypt(encryptedText);
            data = JsonUtility.FromJson<T>(jsonText);
            return data;
        }
        data = JsonUtility.FromJson<T>(encryptedText);
        return data;
    }

    private static string Encrypt(string plainText)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(plainText);
        string encodedText = Convert.ToBase64String(bytesToEncode);

        const int LineLength = 50; // Số ký tự trên mỗi dòng
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encodedText.Length; i += LineLength)
        {
            int length = Mathf.Min(LineLength, encodedText.Length - i);
            sb.AppendLine(encodedText.Substring(i, length));
        }

        return sb.ToString();
    }

    private static string Decrypt(string encryptedText)
    {
        byte[] decodedBytes = Convert.FromBase64String(encryptedText);
        string decodedText = Encoding.UTF8.GetString(decodedBytes);
        return decodedText;
    }
}

public enum FileNameData
{
    UserData,
    CharacterData,
    TrophyRoad,
    BestaryData,
    SettingsData,
    ShopData, 
    DailyData
}
