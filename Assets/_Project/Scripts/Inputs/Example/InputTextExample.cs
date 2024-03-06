using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Platformer
{
    /**
     * Demo script to show how to use the InputReader
     */
    public class InputTextExample : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private InputReader inputReader;

        private Vector2 _movement = new();
        private bool _aiming = false;
        private bool _shooting = false;
        private bool _jumping = false;

        private Vector2 _look = new();
        private bool _isMouse = false;

        private void Start()
        {
            UpdateText();

            inputReader.Move += v => {
                _movement = v;
                UpdateText();
            };

            inputReader.EnableAimingMode += () =>
            {
                _aiming = true;
                UpdateText();
            };

            inputReader.DisableAimingMode += () =>
            {
                _aiming = false;
                UpdateText();
            };

            inputReader.Fire += () =>
            {
                _shooting = true;
                UpdateText();

                Task.Delay(500).ContinueWith(_ =>
                {
                    _shooting = false;
                    UpdateText();
                });
            };

            inputReader.Jump += () =>
            {
                _jumping = true;
                UpdateText();

                Task.Delay(500).ContinueWith(_ =>
                {
                    _jumping = false;
                    UpdateText();
                });
            };

            inputReader.Look += (value, isMouse) =>
            {
                _isMouse = isMouse;

                if (!isMouse) _look = value;
                else _look = GetMousePosition();

                UpdateText();
            };
        }

        private Vector2 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, _look);
            
        }

        private void UpdateText()
        {
            textMeshPro.text = $"Is Moving : {_movement}\r\nIs Aiming Enabled : {_aiming}\r\nShooting : {_shooting}\r\nIs Jumping : {_jumping}\r\nLook : {_look}, isMouse {_isMouse}\r\n\n\n (Try using a Controller!)";
        }
    }
}
