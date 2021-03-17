using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtaCetin
{
    public class PlayerInputSystem : MonoBehaviour
    {
        private float _lastFrameFingerPositionX;

        public bool GetScreenTouching { get; private set; }

        public float MoveFactorX { get; private set; }

        private void Update()
        {
            if (Input.touchCount > 0)
                GetScreenTouching = true;

            if (Input.GetMouseButtonDown(0))
            {
                GetScreenTouching = true;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                MoveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                GetScreenTouching = false;
                MoveFactorX = 0f;
            }
        }
    }
}