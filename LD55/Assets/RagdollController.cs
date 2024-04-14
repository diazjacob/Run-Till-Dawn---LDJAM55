using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    private Rigidbody[] _rbs;
    [SerializeField] private GameObject _armeratureRoot;
    private Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        SetRBOnOff( false );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRBOnOff(bool on )
    {
        for( int i = 0; i < _rbs.Length; i++ )
        {
            _rbs[i].isKinematic = !on;
        }

        _armeratureRoot.SetActive(on);
    }

    public void MultiplyVelocity(float f)
    {
        for( int i = 0; i < _rbs.Length; i++ )
        {
            _rbs[i].AddForce( _rbs[i].velocity.normalized * f );
        }
    }

    public void DisableAnims()
    {
        _anim.enabled = false;
    }

    public void EnableAnims()
    {
        _anim.enabled = true;
    }

    [ContextMenu("Disable Ragdoll")]
    public void RagdollOff() { SetRBOnOff( false ); }

    [ContextMenu("Enable Ragdoll")]
    public void RagdollOn() { SetRBOnOff(true); }

}
