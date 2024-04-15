using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject topLeft;
    public GameObject bottomRight;
    public GenerationGroup grassGen;
    public GenerationGroup treeGen;
    public GenerationGroup rockGen;
    public List<GameObject> allInstantiatedGrass = new List<GameObject>();
    public List<GameObject> allInstantiatedTree = new List<GameObject>();
    public List<GameObject> allInstantiatedRock = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateAll()
    {
        for(int i = 0; i < grassGen.generateIterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint() + Vector3.up * 3000;
            print( testPoint );

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, grassGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                Debug.Log( "Did Hit" );

                var obj = Instantiate( grassGen.objs[Random.Range( 0, grassGen.objs.Length )], hit.point, Quaternion.LookRotation(Vector3.forward, hit.normal) * Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * grassGen.scale;

                allInstantiatedGrass.Add( obj );

            }
        }
        for( int i = 0; i < treeGen.generateIterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint() + Vector3.up * 3000;
            print( testPoint );

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, treeGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                Debug.Log( "Did Hit" );

                var obj = Instantiate( treeGen.objs[Random.Range( 0, treeGen.objs.Length )], hit.point, Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * treeGen.scale;

                allInstantiatedTree.Add( obj );

            }
        }
        for( int i = 0; i < rockGen.generateIterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint() + Vector3.up * 3000;
            print( testPoint );

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, rockGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                Debug.Log( "Did Hit" );

                var obj = Instantiate( rockGen.objs[Random.Range( 0, rockGen.objs.Length )], hit.point, Quaternion.LookRotation( Vector3.forward, hit.normal ) * Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * rockGen.scale;

                allInstantiatedRock.Add( obj );

            }
        }

    }

    private Vector3 GetTestPoint()
    {
        return new Vector3(
            Mathf.Lerp( topLeft.transform.position.x, bottomRight.transform.position.x, Random.value ),
            1000,
            Mathf.Lerp( topLeft.transform.position.z, bottomRight.transform.position.z, Random.value ) );
    }
}

[System.Serializable]
public class GenerationGroup
{
    public float generateIterations;
    public float generatePosRandomness;
    public LayerMask generateLAYERMASK;
    public GameObject[] objs;
    public float scale;
}
