using Assets.Scripts.Enums;
using Assets.Scripts.FSM;
using Assets.Scripts.Grid;

namespace Assets.Scripts.EventBus
{
  public class EventStructs
  {
    #region FSM
    public struct StateChanged : IEvent
    {
      public State State;
    }
    #endregion

    #region Audio
    public struct AudioSwitchedEvent : IEvent { }
    #endregion

    #region UI
    public struct UIButtonPressed : IEvent
    {
      public UIButtonType ButtonType;
      public ShopButtonType ShopButtonType;
    }
    #endregion

    #region Player
    public struct NowhereToGo : IEvent { }
    #endregion

    #region Game
    public struct WinLose : IEvent
    {
      public bool Win;
    }
    #endregion

    #region MoneyManager
    public struct UpdateCoinAmount : IEvent
    {
      public int Amount;
    }
    #endregion

    #region Tile
    public struct TilePainted : IEvent
    {
      public Tile Tile;
      public bool IsPainted;
    }
    #endregion

    #region Item
    public struct CoinPickedUp : IEvent
    {
      public int Value;
    }
    #endregion

    #region Shop
    public struct ShopCharacterSpawned : IEvent
    {
      public string Name;
      public int Price;
      public bool Unlocked;
    }
    #endregion
  }
}