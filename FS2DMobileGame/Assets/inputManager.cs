using UnityEngine;

public class inputManager : MonoBehaviour
{
    public GameObject PlayerObject;
    public void OnSpawnButton()
    {
        Instantiate(PlayerObject);
    }
}
