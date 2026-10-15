using System.Drawing;
using UnityEngine;

public class BackGroundColor : MonoBehaviour
{
    public UnityEngine.Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        color = new UnityEngine.Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        GetComponent<MeshRenderer>().material.color = color;
    }
}
