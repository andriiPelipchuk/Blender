using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePopup : MonoBehaviour
{
    public event Action StartClicked;
    
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Transform _panel;
    [SerializeField] private Ease _openEase;

    private void Awake()
    {
        _startGameButton.onClick.AddListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        Close();
        
        StartClicked?.Invoke();
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);

        _panel.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(_openEase);
    }

    public void Close()
    {
        _panel.DOScale(Vector3.zero, 0.3f).SetEase(_openEase).OnComplete(() => gameObject.SetActive(false));
    }
}