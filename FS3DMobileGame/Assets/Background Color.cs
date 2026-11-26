using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BackgroundColor : MonoBehaviour
{

    [SerializeField] public GameObject Platform1;
    [SerializeField] public GameObject Platform2;

    private UnityEngine.Color Platform1Color;
    private UnityEngine.Color Platform2Color;
    private UnityEngine.Color OldColor;
    private bool RoutineActive;

    // Update is called once per frame
    void Update()
    {
        OldColor = GetComponent<MeshRenderer>().material.color;
        Platform1Color = Platform1.GetComponent<MeshRenderer>().material.color;
        Platform2Color = Platform2.GetComponent<MeshRenderer>().material.color;
        Color MixedColor = new Color((Platform1Color.r + Platform2Color.r) / 1.5f, (Platform1Color.g + Platform2Color.g) / 1.5f, (Platform1Color.b + Platform2Color.b) / 1.5f, 1);
        StartCoroutine(ColorChange(OldColor, MixedColor));
    }
    IEnumerator ColorChange(Color StartColor, Color EndColor)
    {
        if (!RoutineActive)
        {
            RoutineActive = true;
            for (float i = 0; i < 1;)
            {
                GetComponent<MeshRenderer>().material.color = Color.Lerp(StartColor, EndColor, i);
                yield return new WaitForSeconds(0.1f);
                i = i + 0.1f;

            }
            RoutineActive = false;
        }


    }
}
