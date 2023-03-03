using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class ShotingController : IShoting
    {
        [SerializeField] Crosshair _crosshair;
        [Header("Enemy")]
        public Vector2 enemyCrosshairSize = new Vector2(100, 100);
        public Color enemyCrosshairColor = Color.red;
        public float enemySmoothSpeed = 0.1f;
        [Header("Default")]
        public Vector2 defaultCrosshairSize = new Vector2(100, 100);
        public Color defaultCrosshairColor = Color.white;
        public float defaultSmoothSpeed = 0.1f;

        private void Update()
        {
            _crosshair.SetSize(defaultCrosshairSize, defaultSmoothSpeed);
            _crosshair.SetColor(defaultCrosshairColor, defaultSmoothSpeed);
        }
    }

}