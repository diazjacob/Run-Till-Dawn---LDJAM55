using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraPosLerpSpeed;
    [SerializeField] private float _cameraRotLerpSpeed;
    [SerializeField] private GameObject _cameraPosTarget;
    private Vector3 _camPosTarget;
    [SerializeField] private GameObject _cameraLookTarget;
    [SerializeField] private Vector3 _crashAbsoluteCAmeraOffset = new Vector3(5,10);
    public bool isPosLerping = true;
    public bool crashCam = false;

    void Start()
    {
        isPosLerping = true;
    }

    void Update()
    {
        if( crashCam ) _camPosTarget = _cameraPosTarget.transform.position + _crashAbsoluteCAmeraOffset;
        else _camPosTarget = _cameraPosTarget.transform.position;

        if( isPosLerping )
        {
            UpdateCameraPosLerp();
        }
        UpdateCameraRotLerp();

        
    }

    public void ResetCam()
    {
        transform.position = _cameraLookTarget.transform.position;
    }

    private void UpdateCameraPosLerp()
    {
        transform.position = Vector3.Lerp( transform.position, _camPosTarget, Time.deltaTime * _cameraPosLerpSpeed );
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
