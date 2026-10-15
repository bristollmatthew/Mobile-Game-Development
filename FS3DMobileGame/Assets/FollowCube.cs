using UnityEngine;

public class FollowCube : MonoBehaviour
{
    public GameObject Cube;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     Vector3 CubePosition = Cube.transform.position;
     this.transform.position = CubePosition + new Vector3(-0.93f,0.23f,0f) ;

    }
}
