﻿using UnityEngine;

namespace MainMode.Items
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CrystalDust : ConsumablesItem
    {
        [SerializeField] private CrystalSheild _wallCrystalPrefab;

        private CrystalSheild crystalSheild;

        private void OnEnable()
        {
            UseAction += Actvate;
            ResetAction += ResetWall;
        }
        private void OnDisable()
        {
            UseAction -= Actvate;
            ResetAction -= ResetWall;
        }
        private void Actvate()
        {
            if (!crystalSheild)
            {
                crystalSheild = Instantiate(_wallCrystalPrefab.gameObject).GetComponent<CrystalSheild>();
                crystalSheild.transform.parent = transform;
            }
            crystalSheild.ShowItem();
            crystalSheild.transform.position = user.Position + (Vector2)user.transform.up;
        }

        private void ResetWall()
        {
            if (crystalSheild)
                crystalSheild.Explosion();
        }
    }
}
