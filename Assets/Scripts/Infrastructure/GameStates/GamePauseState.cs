using UnityEngine;

namespace Assets.Scripts.Infrastructure.GameStates
{
  public class GamePauseState : GameBaseState
  {
    public GamePauseState(GameBootstrapper gameBootstrapper) : base(gameBootstrapper) { }

    public override void Enter() {
      Time.timeScale = 0;
    }

    public override void Exit() {
      Time.timeScale = 1;
    }
  }
}