using UnityEngine;
using DG.Tweening;
using Assets.Scripts.EventBus;
using Assets.Scripts.Memento;

namespace Assets.Scripts.Grid
{
  public class Tile : MonoBehaviour
  {
    [Header("Visual Settings")]
    [SerializeField] private float _colorChangeDuration = 0.25f;
    [Space]
    [SerializeField] private float _settlingY = -0.05f;
    [SerializeField] private float _settlingDuration = 0.15f;
    [Space]
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _paintedMaterial;
    [Header("Sound")]
    [SerializeField] private AudioClip _paintSound;

    public bool IsPainted { get; private set; } = false;

    private float _startPositionY;
    private float _minPitch = 0.95f;
    private float _maxPitch = 1.05f;

    private AudioSource _audioSource;
    private Renderer _renderer;

    private void Awake() {
      _renderer = GetComponentInChildren<Renderer>();
      _audioSource = GetComponent<AudioSource>();
    }

    void Start() {
      _startPositionY = transform.position.y;

      UpdateTile();
    }

    public void PaintTile() {
      IsPainted = true;

      UpdateTile();
      SettleAnimation();
      PlayPaintSound();

      EventBus<EventStructs.TilePainted>.Raise(new EventStructs.TilePainted {
        Tile = this,
        IsPainted = IsPainted,
      });
    }

    public void ResetTile() {
      IsPainted = false;

      UpdateTile();
    }

    private void UpdateTile() {
      Material targetMaterial = IsPainted ? _paintedMaterial : _defaultMaterial;

      _renderer.material.DOColor(targetMaterial.color, _colorChangeDuration);
    }

    private void SettleAnimation() {
      Sequence mySequence = DOTween.Sequence();

      mySequence.Append(transform.DOMoveY(_settlingY, _settlingDuration));
      mySequence.Append(transform.DOMoveY(_startPositionY, _settlingDuration));

      mySequence.Play();
    }

    public TileMemento CreateMemento() {
      return new TileMemento(transform.position, IsPainted);
    }

    public void RestoreMemento(TileMemento memento) {
      IsPainted = memento.IsPainted;

      UpdateTile();
    }

    private void PlayPaintSound() {
      _audioSource.pitch = Random.Range(_minPitch, _maxPitch);

      _audioSource.PlayOneShot(_paintSound);
    }
  }
}