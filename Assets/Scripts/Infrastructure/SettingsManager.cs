using UnityEngine;
using System.IO;

namespace Assets.Scripts.Infrastructure
{
  public class SettingsManager : MonoBehaviour
  {
    private const string SettingsFileName = "settings.json";
    private static readonly string SettingsFolder = "Saves";

    private static string GetFilePath() {
      string filePath;

      if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
        filePath = Path.Combine(Application.persistentDataPath, SettingsFileName);
      }
      else {
        string folderPath = Path.Combine(Application.dataPath, SettingsFolder);
        if (!Directory.Exists(folderPath)) {
          Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, SettingsFileName);
      }

      return filePath;
    }

    public static T LoadSettings<T>() {
      T settings = default;
      string filePath = GetFilePath();

      if (File.Exists(filePath)) {
        string json = File.ReadAllText(filePath);
        settings = JsonUtility.FromJson<T>(json);
      }

      return settings;
    }

    public static void SaveSettings<T>(T settings) {
      string json = JsonUtility.ToJson(settings);
      string filePath = GetFilePath();
      File.WriteAllText(filePath, json);
    }

    public static void ResetSettings() {
      string filePath = GetFilePath();

      if (File.Exists(filePath)) {
        File.Delete(filePath);
      }
    }
  }
}