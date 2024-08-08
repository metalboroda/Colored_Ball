using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Utility;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class MainMenuCanvas : CanvasBase
  {
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _resetButton;

    private SceneLoader _sceneLoader;

    private void Awake() {
      _sceneLoader = new();
    }

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _startButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Start
        });
      });

      _shopButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Shop
        });
      });

      _resetButton.onClick.AddListener(() => {
        ES3.DeleteFile(SettingsHashes.SavePath);
        ES3.DeleteKey(SettingsHashes.BallName, SettingsHashes.SavePath);
        ES3.DeleteKey(SettingsHashes.UnlockedCharacters, SettingsHashes.SavePath);

        _sceneLoader.RestartScene();
      });
    }
  }
}