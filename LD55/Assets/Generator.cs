using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject _topLeft;
    public GameObject _bottomRight;
    public float _generateIterations;
    public float _generatePosRandomness;
    public LayerMask _generateLAYERMASK;
    public GameObject[] _grassObjects;
    public float _grassScale;
    public List<GameObject> _allInstantiatedGrass = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrass()
    {
        for(int i = 0; i < _generateIterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint() + Vector3.up * 3000;
            print( testPoint );

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, _generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                Debug.Log( "Did Hit" );

                var obj = Instantiate( _grassObjects[Random.Range( 0, _grassObjects.Length )], hit.point, Quaternion.LookRotation(Vector3.forward, hit.normal) * Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * _grassScale;

                _allInstantiatedGrass.Add( obj );

            }
        }

    }

    private Vector3 GetTestPoint()
    {
        return new Vector3(
            Mathf.Lerp( _topLeft.transform.position.x, _bottomRight.transform.position.x, Random.value ),
            1000,
            Mathf.Lerp( _topLeft.transform.position.z, _bottomRight.transform.position.z, Random.value ) );
    }
}
