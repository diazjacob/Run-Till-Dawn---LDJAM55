using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    private Rigidbody[] _rbs;
    // Start is called before the first frame update
    void Start()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
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
    }
}
