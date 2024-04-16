using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PersistentObj : MonoBehaviour
{
    public static PersistentObj instance;

    public string levelString;
    public PlayerController playerController;

    public GameObject _changeStringUI;
    public TMP_InputField _in;

    public GameObject _startMenuUI;

    public GameObject promptUI;
    public GameObject crashUI;
    public GameObject winUI;

    public TMP_Text _progressUI;
    public TMP_Text _levelStringTExt;

    public bool HasStarted = false;

    public bool globablInit = false;

    public TMP_Text _winUIText;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad( gameObject );
        //playerController = FindObjectOfType<PlayerController>();

        //Cursor control and hiding
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !globablInit )
        {
            globablInit = true;
            SceneManager.LoadScene( "OutdoorsScene" );
            _startMenuUI.SetActive( false );
            _changeStringUI.SetActive( true );
            SetUI( 2, false );
        }

        if( Input.GetKeyDown( KeyCode.Return ) )
        {
            
            string realText = _in.text.ToLower().Trim();
            if(realText.Length > 0 )
            {
                HasStarted = true;
                SaveString();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                SetUI( 0, true );
            }

        }
        if( Input.GetKeyDown( KeyCode.Escape ) )
        {
            SetUI( 2, false );
            HasStarted = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _changeStringUI.SetActive( true );
            winUI.SetActive( false );
        }
    }

    public void InputChanged()
    {
        if( _in.text.Length > 8 ) _in.text = _in.text.Substring( 0, 8 );
        
    }

    public void SaveString()
    {
        _changeStringUI.SetActive( false );

        string realText = _in.text.ToLower().Trim();
        levelString = realText;
        Generator.Instance.noiseString = realText;
        _levelStringTExt.text = realText;
       //SceneManager.LoadScene( "OutdoorsScene" );
    }

    public void SetUI(int i, bool b )
    {
        if( i == 0 )
        {
            promptUI.SetActive( b );
        }
        if( i == 1 )
        {
            crashUI.SetActive( b );
        }
        if( i == 2 )
        {
            winUI.SetActive( b );
            if (b)
                _winUIText.text = "You Escaped the demons on level \"" + levelString + "\". a difficulty of " + levelString.Length;
        }
    }

    public void ChangeProgressUI(string ui )
    {
        _progressUI.text = ui;
    }
}
