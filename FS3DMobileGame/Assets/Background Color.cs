using UnityEngine;

public class BackgroundColor : MonoBehaviour
{

    [SerializeField] public GameObject Platform1;
    [SerializeField] public GameObject Platform2;

    private UnityEngine.Color Platform1Color;
    private UnityEngine.Color Platform2Color;

    // Update is called once per frame
    void Update()
    {
        Platform1Color = Platform1.GetComponent<MeshRenderer>().material.color;
        Platform2Color = Platform2.GetComponent<MeshRenderer>().material.color;
        Color MixedColor = new Color((Platform1Color.r + Platform2Color.r) / 1.5f, (Platform1Color.g + Platform2Color.g) / 1.5f, (Platform1Color.b + Platform2Color.b) / 1.5f, 1);
        GetComponent<MeshRenderer>().material.color = MixedColor;
    }
}
