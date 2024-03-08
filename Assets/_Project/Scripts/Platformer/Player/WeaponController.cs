using DG.Tweening;
using System;
using UnityEngine;

namespace Platformer
{
    public class WeaponController
    {
        private Transform _weaponCenter;
        private GameObject _weapon;
        private GameObject _bulletPrefab;
        private GameObject _aimIndicator;

        private float _weaponOffset;
        
        private InputReader _playerInput;

        public Transform WeaponCenter { get => _weaponCenter; set => _weaponCenter = value; }
        public GameObject Weapon { get => _weapon; set => _weapon = value; }
        public GameObject BulletPrefab { get => _bulletPrefab; set => _bulletPrefab = value; }
        public float WeaponOffset { get => _weaponOffset; set => _weaponOffset = value; }
        public GameObject AimIndicator { get => _aimIndicator; set => _aimIndicator = value; }

        public WeaponController(Transform weaponCenter, GameObject weapon, GameObject bulletPrefab, InputReader playerInput, float weaponOffset, GameObject aimIndicator)
        {
            _playerInput = playerInput;

            WeaponCenter = weaponCenter;
            Weapon = weapon;
            BulletPrefab = bulletPrefab;
            WeaponOffset = weaponOffset;
            AimIndicator = aimIndicator;

            AimIndicator.SetActive(false);

            //_playerInput.Fire += HandleFire;
            _playerInput.Look += HandleAim;
            _playerInput.EnableAimingMode += EnableAimingMode;
            _playerInput.DisableAimingMode += DisableAimingMode;
        }

        private void EnableAimingMode()
        {
            HandleSlowMotion();

            AimIndicator.SetActive(true);
            AimIndicator.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            AimIndicator.transform.DOScale(1, 0.2f);
        }

        private void DisableAimingMode()
        {
            HandleSlowMotion(false);

            AimIndicator.transform.DOScale(.01f, 0.2f);
            AimIndicator.SetActive(false);
        }

        private void HandleSlowMotion(bool enable = true)
        {
            Time.timeScale = enable ? 0.5f : 1f;
        }

        private void HandleAim(Vector2 direction, bool isMouse)
        {
            if (direction == Vector2.zero) return;

            if (!isMouse)
            {
                MoveWeapon(direction);
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePos - (Vector2)WeaponCenter.position;

            MoveWeapon(directionToMouse);
        }

        private void HandleFire()
        {
            throw new NotImplementedException();
        }

        private void MoveWeapon(Vector2 direction)
        {
            // Moves the weapon around the weaponCenter based on the direction, at a distance of weaponOffset
            Weapon.transform.position = WeaponCenter.position + (Vector3)direction.normalized * WeaponOffset;

            // Rotates the weapon to face the direction
            Weapon.transform.right = direction;
        }
    }

}
