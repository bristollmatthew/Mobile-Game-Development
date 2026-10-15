using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    public GameObject PlayerObject;
    public InputActionAsset inputActions;
    public InputAction Spawn;

    void Start()
    {
        Spawn = inputActions.FindAction("Spawn");
    }
    void Update()
    {
        if (Spawn.WasPressedThisFrame())
        {
            Instantiate(PlayerObject);
        }
    }
}
