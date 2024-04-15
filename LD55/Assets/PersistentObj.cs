using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentObj : MonoBehaviour
{

    public string levelString;
    

    void Start()
    {
        DontDestroyOnLoad( gameObject );
        SceneManager.LoadScene( "OutdoorsScene" );
    }

    void Update()
    {
        
    }
}
