using System;
using System.Collections;
using Food;
using Orders;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private OrderManager _orderManager;
    [SerializeField] private StartGamePopup _startGamePopup;
    [SerializeField] private GameState _gameState;

    [SerializeField] private float _waitBeforeStart;

    private void Awake()
    {
        _startGamePopup.StartClicked += OnStartGame;

        _gameState.GameSateType = GameSateType.Popup;
    }

    private void Start()
    {
        StartCoroutine(WaitALittleAndStart());
    }

    private IEnumerator WaitALittleAndStart()
    {
        yield return new WaitForSeconds(_waitBeforeStart);
        
        _startGamePopup.OpenPopup();
    }
    
    private void OnStartGame()
    {
        _orderManager.BeginOrders();
    }
}