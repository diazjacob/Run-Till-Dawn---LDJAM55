using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraPosLerpSpeed;
    [SerializeField] private float _cameraRotLerpSpeed;
    [SerializeField] private GameObject _cameraPosTarget;
    [SerializeField] private GameObject _cameraLookTarget;
    public bool isPosLerping = true;

    void Start()
    {
        isPosLerping = true;
    }

    void Update()
    {
        if( isPosLerping )
        {
            UpdateCameraPosLerp();
        }
        UpdateCameraRotLerp();
    }

    private void UpdateCameraPosLerp()
    {
        transform.position = Vector3.Lerp( transform.position, _cameraPosTarget.transform.position, Time.deltaTime * _cameraPosLerpSpeed );
    }

    private void UpdateCameraRotLerp()
    {
        transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.LookRotation( _cameraLookTarget.transform.position - transform.position, Vector3.up ), Time.deltaTime * _cameraRotLerpSpeed);
    }

    public void SetCameraPosAndLook(GameObject pos, GameObject look )
    {
        if( pos == null || look == null ) Debug.Log( "ERRORROR: CAMERA SUPPLIED INVALID LOOK OR POS TARGET OBJECTs" );

        _cameraPosTarget = pos;
        _cameraLookTarget = look;
    }
}
