using System;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure
{
  [Serializable]
  public class GameSettings
  {
    #region LevelManager
    public int OverallLevelIndex;
    public int LevelIndex;
    #endregion

    #region MoneyManager
    public int CoinAmount;
    #endregion

    #region Shop
    public List<bool> UnlockedCharacters = new();
    public string BallName = "Player_Ball_1";
    #endregion

    #region Audio Settings
    public bool IsMusicOn = true;
    public bool IsSfxOn = true;
    #endregion
  }
}