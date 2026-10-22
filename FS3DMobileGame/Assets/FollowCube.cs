using UnityEngine;

public class FollowCube : MonoBehaviour
{
    public GameObject Cube;
    public Vector3 PositionOffset;

    void Update()
    {
     Vector3 CubePosition = Cube.transform.position;
     this.transform.position = CubePosition + PositionOffset;

    }
}
