using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtaCetin
{
    public class PlayerInputSystem : MonoBehaviour
    {
        private float _lastFrameFingerPositionX;
        private float _moveFactorX;

        private bool _isTouchingScreen;

        public bool GetScreenTouching => _isTouchingScreen;

        public float MoveFactorX => _moveFactorX;

        private void Update()
        {
            if (Input.touchCount > 0)
                _isTouchingScreen = true;

            if (Input.GetMouseButtonDown(0))
            {
                _isTouchingScreen = true;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isTouchingScreen = false;
                _moveFactorX = 0f;
            }
        }
    }
}