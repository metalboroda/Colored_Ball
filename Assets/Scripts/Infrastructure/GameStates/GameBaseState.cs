using Assets.Scripts.FSM;

namespace Assets.Scripts.Infrastructure.GameStates
{
  public class GameBaseState : State
  {
    protected GameBootstrapper GameBootstrapper;

    public GameBaseState(GameBootstrapper gameBootstrapper) {
      GameBootstrapper = gameBootstrapper;
    }
  }
}