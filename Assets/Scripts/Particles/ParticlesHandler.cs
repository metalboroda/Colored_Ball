using UnityEngine;

namespace Assets.Scripts.Particles
{
  [RequireComponent(typeof(AudioSource))]
  public class ParticlesHandler : MonoBehaviour
  {
    [SerializeField] private float _destroyTime = 3f;
    [Header("Audio")]
    [SerializeField] private AudioClip _particlesSound;

    private float _minPitch = 0.95f;
    private float _maxPitch = 1.05f;

    private AudioSource _audioSource;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
      PlayParticlesSound();
      Destroy(gameObject, _destroyTime);
    }

    private void PlayParticlesSound() {
      _audioSource.pitch = Random.Range(_minPitch, _maxPitch);

      _audioSource.PlayOneShot(_particlesSound);
    }
  }
}