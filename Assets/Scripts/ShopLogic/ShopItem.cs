using System;
using UnityEngine;

[Serializable]
public class ShopItem
{
  [SerializeField] public GameObject Prefab;
  [SerializeField] public int Price;
  [SerializeField] public bool Unlocked;
}