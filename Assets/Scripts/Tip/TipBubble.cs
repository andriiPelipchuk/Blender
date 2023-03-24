using DG.Tweening;
using UnityEngine;

namespace Tip
{
    public class TipBubble : MonoBehaviour
    {
        private Sequence _sequence;
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            if (_sequence != null && _sequence.IsActive())
            {
                _sequence.Kill();
                _sequence = null;
            }
            
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.InOutExpo))
                .AppendInterval(5f)
                .Append(transform.DOScale(Vector3.zero, 0.3f)).SetEase(Ease.InOutExpo)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}