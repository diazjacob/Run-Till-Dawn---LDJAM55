using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _playerAnimator;

    public enum GameStartState
    {
        StartingScreen,

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class GameState
{
    public GameObject _camPos;
    public GameObject _camTarget;
    public 
}
