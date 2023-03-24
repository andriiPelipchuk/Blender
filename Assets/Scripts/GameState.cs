using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game State")]
public class GameState : ScriptableObject
{
    public GameSateType GameSateType;
}

public enum GameSateType {Popup, Ordering, Shaking}