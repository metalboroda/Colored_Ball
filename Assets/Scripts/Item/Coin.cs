using Assets.Scripts.Ball;
using Assets.Scripts.EventBus;
using UnityEngine;

namespace Assets.Scripts.Item
{
  public class Coin : MonoBehaviour
  {
    [SerializeField] private int _value = 10;
    [Header("VFX")]
    [SerializeField] private GameObject _pickupParticles;
    
    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent(out BallHandler ballHandler))
        Pickup();
    }

    public void Pickup() {
      EventBus<EventStructs.CoinPickedUp>.Raise(new EventStructs.CoinPickedUp {
        Value = _value
      });

      if (_pickupParticles != null)
        Instantiate(_pickupParticles, transform.position, Quaternion.identity);

      Destroy(gameObject);
    }
  }
}