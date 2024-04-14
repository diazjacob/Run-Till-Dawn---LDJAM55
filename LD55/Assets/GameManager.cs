using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerBike;
    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Light _sun;

    [SerializeField] private GameState[] _states;
    [SerializeField] private int _currentGameState = 0;
    [SerializeField] private int _playState;

    private bool _sunRed;
    [SerializeField] private Color _sunStartingColor;
    [SerializeField] private Color _sunEndColor;
    [SerializeField] private float _sunChangeTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState();
        _sun.color = _sunStartingColor;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            _currentGameState++;
            UpdateGameState();
        }

        if( _sunRed ) _sun = Color.Lerp( _sun.color, _sunEndColor, Time.deltaTime * _sunChangeTime );
        
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
                case 3:
                    State4();
                    break;
                case 4:
                    State5();
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
        
    }

    private void State3()
    {
        _sunRed = true;
    }

    private void State4()
    {
        _playerAnimator.SetBool( "IdleToScared", true );
        _playerCharacter.transform.rotation = Quaternion.Euler( -90, -177, 0 );
    }

    private void State5()
{
        _playerAnimator.SetBool( "Riding", true );
        _playerCharacter.transform.position = _playerBike.transform.position - new Vector3(0,1,0);
        _playerCharacter.transform.parent = _playerBike.transform;
        _playerCharacter.transform.rotation = Quaternion.Euler( 180, 180, 0 );
        _playerBike.StartGame();
    }

    public void SetSunValue( float f)
    {

    }
}

[System.Serializable]
public class GameState
{
    public GameObject _camPos;
    public GameObject _camTarget;

    public void EnterGameState() { }
}
