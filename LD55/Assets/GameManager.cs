using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerBike;
    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameState[] _states;
    [SerializeField] private int _currentGameState = 0;
    [SerializeField] private int _playState;


    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            _currentGameState++;
            UpdateGameState();
        }
    }

    private void UpdateGameState()
    {
        if( _currentGameState > -1 && _currentGameState < _states.Length )
        {
            _cameraController.SetCameraPosAndLook( _states[_currentGameState]._camPos, _states[_currentGameState]._camTarget );
            switch( _currentGameState )
            {
                case 0:
                    State1();
                    break;
                case 1:
                    State2();
                    break;
                case 2:
                    State3();
                    break;
            }
        }
        else Debug.Log( "WHAT, THE GAME STATE IS BADD" );

    }

    private void State1()
    {
        _playerCharacter.transform.rotation = Quaternion.Euler( -90, -177, 120 );
    }

    private void State2()
    {
        _playerAnimator.SetBool( "IdleToScared", true );
        _playerCharacter.transform.rotation = Quaternion.Euler( -90, -177, 0 );
    }

    private void State3()
{
        _playerBike.StartGame();
    }
}

[System.Serializable]
public class GameState
{
    public GameObject _camPos;
    public GameObject _camTarget;

    public void EnterGameState() { }
}
