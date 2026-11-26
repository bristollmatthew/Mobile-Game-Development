using System.Collections;
using UnityEngine;

public class ParticleCode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool killscript = false;

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(Killscript());
    }

    IEnumerator Killscript()
    {
        if (!killscript)
        {
            killscript = true;
            yield return new WaitForSeconds(5);
            Destroy(gameObject);

        }
    }
}
