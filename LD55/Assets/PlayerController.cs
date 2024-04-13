using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _walkForce;
    [SerializeField] private float _sprintMultiplier;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _lookSensitivity;
    [SerializeField] private bool _invertLookVert = true;

    private Camera _cam;
    private Rigidbody _rb;
    private Vector2 _prevMousePosition;

    void Start()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }


    void Update()
    {
        CheckMovement();
        CheckLooking();
    }

    private void CheckMovement()
    {
        Vector3 forward = Vector3.ProjectOnPlane( _cam.transform.forward, Vector3.up ).normalized;
        Vector3 right = Vector3.Cross( forward, Vector3.up );

        float sprintMultiplier = Input.GetKey( KeyCode.LeftShift ) ? _sprintMultiplier : 1;

        if( Input.GetKey( KeyCode.W ) )
            _rb.AddForce( forward * _walkForce * sprintMultiplier );
        if( Input.GetKey( KeyCode.S ) )
            _rb.AddForce( -forward * _walkForce * sprintMultiplier );
        if( Input.GetKey( KeyCode.A ) )
            _rb.AddForce( right * _walkForce * sprintMultiplier );
        if( Input.GetKey( KeyCode.D ) )
            _rb.AddForce( -right * _walkForce * sprintMultiplier );
        if( Input.GetKeyDown( KeyCode.Space ) )
            _rb.AddForce( Vector3.up * _jumpForce );
    }
    /// <summary>
    /// TEST?????
    /// </summary>

    private void CheckLooking()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * _lookSensitivity;

        _cam.transform.rotation = _cam.transform.rotation * Quaternion.Euler( mouseDelta.y * (_invertLookVert ? -1 : 1), mouseDelta.x, 0);
        _cam.transform.rotation = Quaternion.Euler( _cam.transform.rotation.eulerAngles.x, _cam.transform.rotation.eulerAngles.y, 0 );
    }
}
