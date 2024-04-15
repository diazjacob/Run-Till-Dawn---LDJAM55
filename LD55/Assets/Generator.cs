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

    public string noiseString;

    public string savedNoiseString;
    public int noiseValue = -1;

    [SerializeField] private FastNoiseUnity _noise;
    [SerializeField] private FastNoiseUnity _otherNoise;

    // Start is called before the first frame update
    void Start()
    {
        //_noise = GetComponent<FastNoiseUnity>();

        GenerateAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (savedNoiseString != noiseString )
        {
            savedNoiseString = noiseString;
            noiseValue = 0;
            foreach (char c in savedNoiseString )
            {
                noiseValue += ( int )c;

                _noise.seed = noiseValue;
                _otherNoise.seed = noiseValue;
            }
        }
    }

    private void GenerateAll()
    {
        float yVal = Mathf.PI + 100;
        int iterations = grassGen.GetGenerationIterations(savedNoiseString.Length);

        for(int i = 0; i < iterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint(yVal + i * i, i ) + Vector3.up * 3000;

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, grassGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                var obj = Instantiate( grassGen.objs[Random.Range( 0, grassGen.objs.Length )], hit.point, Quaternion.LookRotation(Vector3.forward, hit.normal) * Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * grassGen.scale;

                allInstantiatedGrass.Add( obj );

            }
        }

        yVal = 100 * Mathf.PI;
        iterations = treeGen.GetGenerationIterations( savedNoiseString.Length );

        for( int i = 0; i < iterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint( yVal + i*i, i * Mathf.PI ) + Vector3.up * 3000;

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, treeGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                var obj = Instantiate( treeGen.objs[Random.Range( 0, treeGen.objs.Length )], hit.point, Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * treeGen.scale;

                allInstantiatedTree.Add( obj );

            }
        }

        yVal = 200 * Mathf.PI;
        iterations = rockGen.GetGenerationIterations( savedNoiseString.Length );

        for( int i = 0; i < iterations; i++ )
        {
            RaycastHit hit;

            Vector3 testPoint = GetTestPoint( yVal + i * i, i) + Vector3.up * 3000;

            if( Physics.Raycast( testPoint, Vector3.down, out hit, Mathf.Infinity, rockGen.generateLAYERMASK ) )
            {
                Debug.DrawLine( hit.point, hit.point + Vector3.up, Color.yellow );

                var obj = Instantiate( rockGen.objs[Random.Range( 0, rockGen.objs.Length )], hit.point, Quaternion.LookRotation( Vector3.forward, hit.normal ) * Quaternion.AngleAxis( Random.value * 360, Vector3.up ), transform );
                obj.transform.localScale = Vector3.one * rockGen.scale;

                allInstantiatedRock.Add( obj );

            }
        }

    }

    private Vector3 GetTestPoint(float x, float z )
    {
        Debug.Log( ( _noise.fastNoise.GetNoise( x, x ) + 1 ) / 2f + " || " + ( _otherNoise.fastNoise.GetNoise( z, z ) + 1 ) / 2f );
        return new Vector3(
            Mathf.Lerp( topLeft.transform.position.x, bottomRight.transform.position.x, (_noise.fastNoise.GetNoise(x,x) + 1)/2f ),
            1000,
            Mathf.Lerp( topLeft.transform.position.z, bottomRight.transform.position.z, (_otherNoise.fastNoise.GetNoise(z,z) + 1 ) / 2f ) );
    }
}

[System.Serializable]
public class GenerationGroup
{
    public int generateBaseIterations;
    public int generateIterationAdditions;
    public float generatePosRandomness;
    public LayerMask generateLAYERMASK;
    public GameObject[] objs;
    public float scale;

    public int GetGenerationIterations( int num)
    {
        return generateBaseIterations + (num * generateIterationAdditions);
    }
}
