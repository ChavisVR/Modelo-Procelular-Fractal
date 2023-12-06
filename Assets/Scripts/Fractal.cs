using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{

    public Mesh[] meshes;
    public Material material;

    public int ProfundidaMaxima;

    public float VelMaxRotacion;

    public float maxTwist;

    private float VelRotacion;

    private int Profundidad;

    private Material[,] materiales;
    private void InirializeMateriales()
    {
        materiales = new Material[ProfundidaMaxima + 1, 2];
        for(int i =0; i<= ProfundidaMaxima; i++)
        {
            float t = i / (ProfundidaMaxima - 1f);
            t *= t;
            materiales[i, 0] = new Material(material);
            materiales[i, 0].color = Color.Lerp(Color.white, Color.yellow, (float)i / ProfundidaMaxima);
            materiales[i, 1] = new Material(material);
            materiales[i, 1].color = Color.Lerp(Color.white, Color.cyan, (float)i / ProfundidaMaxima);
        }
        materiales[ProfundidaMaxima, 0].color = Color.magenta;
        materiales[ProfundidaMaxima, 1].color = Color.red;
    }

    private void Start()
    {
        VelRotacion = Random.Range(-VelMaxRotacion, VelMaxRotacion);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if(materiales == null)
        {
            InirializeMateriales();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materiales[Profundidad, Random.Range(0,2)];
        
        if(Profundidad< ProfundidaMaxima)
        {

            StartCoroutine(CrearHijo());

        } 
    }

    private void Update()
    {
        transform.Rotate(0f, VelRotacion * Time.deltaTime, 0f);
    }

    private static Vector3[] DireccionHijo =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };
    private static Quaternion[] OrientacionHijo =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(90f, 0f, 0f),
    };


    public float spawnProbalibility;

    private IEnumerator CrearHijo()
    {
        for(int i = 0; i<DireccionHijo.Length; i++)
        {
            if(Random.value< spawnProbalibility) 
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialized(this, i);
            }
            
        }
    }

    public float EscalaHijo;

    private void Initialized(Fractal parent, int childIndex)
    {

        meshes = parent.meshes;
        materiales = parent.materiales;
        ProfundidaMaxima = parent.ProfundidaMaxima;
        Profundidad = parent.Profundidad + 1;
        EscalaHijo = parent.EscalaHijo;
        transform.parent = parent.transform;
        spawnProbalibility = parent.spawnProbalibility;
        VelMaxRotacion = parent.VelMaxRotacion;
        maxTwist = parent.maxTwist;
        transform.localScale = Vector3.one * EscalaHijo;
        transform.localPosition = DireccionHijo[childIndex] * (0.5f + 0.5f * EscalaHijo);
        transform.localRotation = OrientacionHijo[childIndex];

    }
}
