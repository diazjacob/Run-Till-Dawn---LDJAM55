using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerBike;
    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private HDAdditionalLightData _sun;

    [SerializeField] private GameState[] _states;
    [SerializeField] private int _currentGameState = 0;
    [SerializeField] private int _playState;

    private bool _sunRed;
    [SerializeField] private Color _sunStartingColor;
    [SerializeField] private Color _sunEndColor;
    [SerializeField] private float _sunChangeTime = 3f;

    [SerializeField] private Material _moon;

    private bool _pentGlow;
    [SerializeField] private Color _pentagramStartingColor;
    [SerializeField] private Color _pentagramEndColor;
    [SerializeField] private float _pentChangeTime = 3f;

    [SerializeField] private Material _pent;

    private bool _demonGrow;
    [SerializeField] private GameObject _demon;
    [SerializeField] private float _demonSize;
    [SerializeField] private float _demonChangeTime;

    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _wind;


    public GameObject _dialog1a;
    public GameObject _dialog1b;

    public GameObject _dialog2a;

    public GameObject _dialog3a;
    public GameObject _dialog3b;

    public Animator _idle1;
    public Animator _idle2;

    public GameObject _particles;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState();
        _moon.SetColor( "_EmissiveColor", _sunStartingColor );
        _pent.SetColor( "_EmissiveColor", _pentagramStartingColor );
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) && _currentGameState < 5 && PersistentObj.instance.HasStarted)
        {
            _currentGameState++;
            UpdateGameState();
        }

        if( _sunRed )
        {
            _moon.SetColor("_EmissiveColor", Color.Lerp( _moon.GetColor( "_EmissiveColor" ), _sunEndColor, Time.deltaTime * _sunChangeTime ) );
        }
        if( _pentGlow )
        {
            _pent.SetColor( "_EmissiveColor", Color.Lerp( _pent.GetColor( "_EmissiveColor" ), _pentagramEndColor, Time.deltaTime * _pentChangeTime ) );
        }
        if( _demonGrow )
        {
            _demon.transform.localScale = Vector3.Lerp( _demon.transform.localScale, _demonSize * Vector3.one, Time.deltaTime * _demonChangeTime );
            _wind.volume = Mathf.Lerp( _wind.volume, 0, Time.deltaTime );
        }

        if( _playerBike._hasCrashed && !_playerBike._won) _music.pitch = 0.8f;
        else _music.pitch = 1f;

#if !UNITY_EDITOR
        if( Input.GetKeyDown( KeyCode.Q ) ) Application.Quit();
#endif
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

        _dialog1a.SetActive( false );
        _dialog1b.SetActive( false );

        _dialog2a.SetActive( false );

        _dialog3a.SetActive( false );
        _dialog3b.SetActive( false );
    }

    private void State2()
    {
        _pentGlow = true;

        _dialog1a.SetActive( true );
        _dialog1b.SetActive( false );

        _dialog2a.SetActive( true );

        _dialog3a.SetActive( false );
        _dialog3b.SetActive( false );

        _particles.SetActive( true );
    }

    private void State3()
    {
        _sunRed = true;
        _demonGrow = true;
        _demon.GetComponent<AudioSource>().Play();
        _music.Play();

        _dialog1a.SetActive( false );
        _dialog1b.SetActive( false );

        _dialog2a.SetActive( false );

        _dialog3a.SetActive( true );
        _dialog3b.SetActive( true );
        _idle1.SetBool( "IdleToScared", true );
        _idle2.SetBool( "IdleToScared", true );
    }

    private void State4()
    {
        _playerAnimator.SetBool( "IdleToScared", true );
        _playerCharacter.transform.rotation = Quaternion.Euler( -90, -177, 0 );
        
    }

    public void State5()
    {
        _playerAnimator.SetBool( "Riding", true );
        _playerCharacter.transform.position = _playerBike.transform.position - new Vector3(0,1,0);
        _playerCharacter.transform.parent = _playerBike.transform;
        _playerCharacter.transform.rotation = Quaternion.Euler( 180, 180, 0 );
        _playerBike.StartGame();
        _cameraController.SetCameraPosAndLook( _states[4]._camPos, _states[4]._camTarget );
        PersistentObj.instance.SetUI( 0, false );
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
