using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header( "Physics Forces" )]
    [SerializeField] private float _speedAddWhenGrounded = 10;
    [SerializeField] private float _turningAuthority = 1;
    [SerializeField] private Vector3 _startingVelocity;

    [Header("Raycast & Rotation")]
    [SerializeField] private float _rotationLerpSpeed;
    [SerializeField] private LayerMask _castLayerMask;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private float _isGroundedDistanceThreshold = 2;

    [Header("Turning")]
    [SerializeField] private GameObject _frontWheel;
    [SerializeField] private GameObject _backWheel;
    [SerializeField] private float _wheelMaxAngle = 20;
    [SerializeField] private float _totalDownhillTurnAngle = 60;
    [SerializeField] private Vector3 _overallWheelOffset;
    [SerializeField] private float _frontWheelTurnSpeed;
    [SerializeField] private float _bikeBodyTurnCoeff;
    [SerializeField] private float _turnWheelGroundingCoeff;

    [Header( "Crash Settings" )]
    [SerializeField] private float _minimumCrashVelocityChange;
    [SerializeField] private bool _hasCrashed;
    [SerializeField] private float _gameStartCrashdetectionDelay = 5f;
    private float _startupTimer;
    private Vector3 _lastFrameVelocity;

    private float _currentFrontWheelAngle;
    private float _currentIdealFrontWheelAngle;

    [SerializeField] private float _baseFOV;
    [SerializeField] private float _FOVCoeff;
    [SerializeField] private float _FOVCap = 150f;

    private Vector3 _groundNormal;
    private Quaternion _groundIdealRot;
    private float _currentGroundDistance;
    [SerializeField] private bool _isGrounded;

    [SerializeField] private Camera _cam;
    private Rigidbody _rb;
    [SerializeField] private CameraController _camControl;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.velocity = _startingVelocity;

        //Cursor control and hiding
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }


    void Update()
    {
        if(!_hasCrashed )
        {
            UpdateSteering();
            UpdateBodyRotation();

            UpdateSpeed();

            _cam.fieldOfView = Mathf.Clamp( _baseFOV + _rb.velocity.magnitude * _FOVCoeff, _baseFOV, _FOVCap );
        }
        else
        {
            _rb.freezeRotation = false;
            _camControl.isPosLerping = false;
        }

        if( _startupTimer > _gameStartCrashdetectionDelay )
            CheckHasCrashed();
        else
        {
            _startupTimer += Time.deltaTime;
            _lastFrameVelocity = _rb.velocity;
        }
        
    }

    private void UpdateSteering()
    {
        bool isTurning = false;
        if( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ))
        {
            _currentIdealFrontWheelAngle = -_wheelMaxAngle;
            isTurning = true;
            Debug.Log( "Turning LEFT" );
        }
        if( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
        {
            _currentIdealFrontWheelAngle = _wheelMaxAngle;
            isTurning = true;
            Debug.Log( "Turning RIGHT" );
        }

        if( !isTurning || Vector3.Angle(-transform.forward, -Vector3.forward) > _totalDownhillTurnAngle) _currentIdealFrontWheelAngle = 0;

        _currentFrontWheelAngle = Mathf.Lerp( _currentFrontWheelAngle, _currentIdealFrontWheelAngle, Time.deltaTime * _frontWheelTurnSpeed );

        _frontWheel.transform.rotation = transform.rotation * Quaternion.Euler( _overallWheelOffset.x, _overallWheelOffset.y, _overallWheelOffset.z ) * Quaternion.AngleAxis( _currentFrontWheelAngle, Vector3.right );
    }

    private void UpdateBodyRotation()
    {
        RaycastHit hit;

        Debug.DrawRay( transform.position, Vector3.down, Color.blue );

        if( Physics.Raycast( transform.position, Vector3.down, out hit, _raycastDistance, _castLayerMask ) )
        {
            Debug.DrawLine( hit.point, hit.point+Vector3.up, Color.yellow );
            Debug.DrawLine( hit.point, hit.point + hit.normal, Color.red );
            Debug.DrawLine( transform.position, transform.position + (Vector3.down*_raycastDistance), Color.white );

            Debug.Log( "Did Hit" );

            _groundNormal = hit.normal;
            _currentGroundDistance = hit.distance;
        }
        else
        {
            _currentGroundDistance = float.PositiveInfinity;
            _groundNormal = Vector3.up;
        }

        _groundIdealRot = Quaternion.AngleAxis( Vector3.Angle(_groundNormal, Vector3.up) - (_turnWheelGroundingCoeff * Vector3.Angle( -transform.forward, -Vector3.forward )), -transform.right );

        Quaternion newRot = Quaternion.Lerp( transform.rotation, _groundIdealRot, _rotationLerpSpeed * Time.deltaTime );

        transform.rotation = newRot * Quaternion.AngleAxis(_bikeBodyTurnCoeff * _currentFrontWheelAngle * Time.deltaTime, Vector3.up);

    }

    private void UpdateSpeed()
    {
        _isGrounded = ( _currentGroundDistance < _isGroundedDistanceThreshold );
        Debug.Log( _currentGroundDistance + " || " + _isGroundedDistanceThreshold );


        if( _isGrounded )
        {
            _rb.AddForce( -transform.forward * _speedAddWhenGrounded * Time.deltaTime * _turningAuthority * Mathf.Clamp01( Vector3.Dot( _rb.velocity, -transform.forward ) ) );
            _rb.AddForce( -_rb.velocity * _turningAuthority * Mathf.Clamp01( Vector3.Dot( _rb.velocity, -transform.forward ) ) * Time.deltaTime );
        }

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.white );
    }

    private void CheckHasCrashed()
    {
        if( ( _rb.velocity - _lastFrameVelocity ).magnitude > _minimumCrashVelocityChange ) _hasCrashed = true;
        _lastFrameVelocity = _rb.velocity;
    }
}
