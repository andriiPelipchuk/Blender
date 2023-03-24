using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Orders.Popup
{
    public class OrderCompletePopup : MonoBehaviour
    {
        public event Action RestartClicked;
        public event Action NextClicked;
        
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _rateText;
        [SerializeField] private Slider _restultSlider;
        [SerializeField] private Transform _popupPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Ease _openEase;
        [Space]
        [SerializeField] private float _minSuccessRate;

        [SerializeField] private string _winMessage;
        [SerializeField] private string _looseMessage;


        private float _resultRate;

        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _nextButton.onClick.AddListener(OnNextClicked);
        }


        public void OpenPopup(float resultRate)
        {
            _resultRate = resultRate;

            _restultSlider.value = 0;
            _rateText.text = "0%";
            _resultText.text = string.Empty;
            
            _restartButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(false);

            gameObject.SetActive(true);
            
            _popupPanel.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(_openEase).onComplete += OnPopupOpened;
        }

        public void Close()
        {
            _popupPanel.DOScale(Vector3.zero, 0.3f).SetEase(_openEase).OnComplete(() => gameObject.SetActive(false));
        }

        private void OnPopupOpened()
        {
            _restultSlider.DOValue(_resultRate, 1f).From(0);
            var rateValue = 0f;

            var duration = _resultRate;
            
            DOTween
                .To(() => rateValue, x => rateValue = x, _resultRate, duration)
                .OnUpdate(() => _rateText.text = $"{(rateValue * 100f):0}%")
                .OnComplete(OnResultReady);
        }

        private void OnResultReady()
        {
            var success = _resultRate >= _minSuccessRate;
            _restartButton.gameObject.SetActive(!success);
            _nextButton.gameObject.SetActive(success);

            _resultText.text = success ? _winMessage : _looseMessage;
        }

        private void OnRestartClicked()
        {
            RestartClicked?.Invoke();
            Close();
        }
        
        private void OnNextClicked()
        {
            NextClicked?.Invoke();
            Close();
        }
    }
}