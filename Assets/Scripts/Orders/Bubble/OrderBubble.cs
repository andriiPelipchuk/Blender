using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Orders.Bubble
{
    public class OrderBubble : MonoBehaviour
    {
        [SerializeField] private Image _colorImage;
        [SerializeField] private float _shakeDuration;
        [SerializeField] private float _shakeStrength;

        public void Show(Color targetColor)
        {
            gameObject.SetActive(true);
            transform.DOShakeRotation(_shakeDuration, _shakeStrength);
            transform.DOScale(Vector3.one, _shakeDuration).From(Vector3.zero);
            _colorImage.color = targetColor;
        }
        
        
    }
}