using Assets.Scripts.EventBus;
using Assets.Scripts.FSM;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour
  {
    public static GameBootstrapper Instance { get; private set; }

    public FiniteStateMachine FiniteStateMachine { get; private set; }
    public SceneLoader SceneLoader { get; private set; }
    public MoneyManager MoneyManager { get; private set; }

    private EventBinding<EventStructs.UIButtonPressed> _uiButtonPressed;
    private EventBinding<EventStructs.WinLose> _winLose;

    private void Awake() {
      InitSingleton();

      FiniteStateMachine = new();
      SceneLoader = new();
      MoneyManager = new(this);
    }

    private void OnEnable() {
      if (Instance == this) {
        _uiButtonPressed = new EventBinding<EventStructs.UIButtonPressed>(OnUIButtonPressed);
        _winLose = new EventBinding<EventStructs.WinLose>(OnWInLose);
      }
    }

    private void OnDisable() {
      if (_uiButtonPressed != null)
        _uiButtonPressed.Remove(OnUIButtonPressed);

      if (_winLose != null)
        _winLose.Remove(OnWInLose);
    }

    private void Start() {
      if (FiniteStateMachine == null || MoneyManager == null) {
        Debug.LogError("Dependencies not initialized properly.");
        return;
      }

      FiniteStateMachine.Init(new GameMainMenuState(this));
    }

    private void OnDestroy() {
      if (MoneyManager != null) {
        MoneyManager.Dispose();
      }
    }

    private void InitSingleton() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      }
      else {
        Instance = this;

        DontDestroyOnLoad(gameObject);
      }
    }

    private void OnUIButtonPressed(EventStructs.UIButtonPressed uiButtonPressed) {
      if (SceneLoader == null || FiniteStateMachine == null || MoneyManager == null) {
        Debug.LogError("Dependencies not initialized properly.");
        return;
      }

      switch (uiButtonPressed.ButtonType) {
        case Enums.UIButtonType.Start:
          SceneLoader.LoadSceneAsync(Hashes.GameScene, () => {
            FiniteStateMachine.ChangeState(new GamePlayState(this));
          });
          break;
        case Enums.UIButtonType.Pause:
          FiniteStateMachine.ChangeState(new GamePauseState(this));
          break;
        case Enums.UIButtonType.Continue:
          FiniteStateMachine.ChangeState(new GamePlayState(this));
          break;
        case Enums.UIButtonType.Restart:
          SceneLoader.RestartSceneAsync(() => {
            FiniteStateMachine.ChangeState(new GamePlayState(this));
          });
          break;
        case Enums.UIButtonType.MainMenu:
          SceneLoader.LoadSceneAsync(Hashes.MainMenuScene, () => {
            FiniteStateMachine.ChangeState(new GameMainMenuState(this));
          });
          break;
        case Enums.UIButtonType.Shop:
          FiniteStateMachine.ChangeState(new GameShopState(this));
          break;
        case Enums.UIButtonType.NextLevel:
          SceneLoader.RestartSceneAsync(() => {
            FiniteStateMachine.ChangeState(new GamePlayState(this));
          });
          break;

      }
    }

    private void OnWInLose(EventStructs.WinLose winLose) {
      if (winLose.Win == true)
        FiniteStateMachine.ChangeState(new GameWinState(this));
      else
        FiniteStateMachine.ChangeState(new GameLoseState(this));
    }
  }
}