using Assets.Scripts.Infrastructure;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EditorCustom
{
  public class SettingsMenu
  {
    [MenuItem("Tools/Reset Settings")]
    private static void ResetSettings() {
      SettingsManager.ResetSettings();
      Debug.Log("Settings have been reset.");
    }
  }
}