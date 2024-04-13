using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _driftParticles;
    [SerializeField] [Range(0,1f)] private float _turnStrenghToCauseDriftParticles;
    [SerializeField] private float _minDriftVelocity = 2f;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float turnDiff = Mathf.Clamp01( Vector3.Dot( _rb.velocity.normalized, -transform.forward.normalized ) );

        if( _rb.velocity.magnitude > _minDriftVelocity && turnDiff < _turnStrenghToCauseDriftParticles ) _driftParticles.Play();
        else _driftParticles.Pause();
    }
}
