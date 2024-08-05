using Assets.Scripts.EventBus;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.FSM
{
  public class FiniteStateMachine
  {
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }

    public void Init(State initState) {
      CurrentState = initState;
      CurrentState.Enter();

      EventBus<EventStructs.StateChanged>.Raise(new EventStructs.StateChanged { State = CurrentState });
    }

    public void ChangeState(State newState, bool overrideState = false) {
      if (overrideState == false)
        if (newState == CurrentState) return;

      PreviousState = CurrentState;
      CurrentState.Exit();
      CurrentState = newState;
      CurrentState.Enter();

      EventBus<EventStructs.StateChanged>.Raise(new EventStructs.StateChanged { State = CurrentState });
    }

    public void ChangeStateWithDelay(State newState, float delay, MonoBehaviour monoBehaviour) {
      monoBehaviour.StartCoroutine(DoChangeStateWithDelay(newState, delay));
    }

    private IEnumerator DoChangeStateWithDelay(State newState, float delay) {
      yield return new WaitForSeconds(delay);

      ChangeState(newState);
    }
  }
}