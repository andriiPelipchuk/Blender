using System;
using UnityEngine;

namespace Blender
{
    public class ShakeButton : MonoBehaviour
    {
        [SerializeField] private Blender _blender;
        
        private void OnMouseDown()
        {
            _blender.Shake();
        }
    }
}