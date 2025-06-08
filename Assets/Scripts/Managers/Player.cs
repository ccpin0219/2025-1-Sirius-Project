using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Character> allies = new();

    public void Init()
    {
        allies.Clear();
        allies.AddRange(FindObjectsOfType<Character>());
    }

    public void StartPlayerTurn(Character currentCharacter)
    {
        Debug.Log($"[플레이어] {currentCharacter.characterName} 턴 시작");

    }
}
