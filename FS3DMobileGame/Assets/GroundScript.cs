using System.Collections;
using System.Linq;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    float lastShrinkStartTime;
    bool isShrinking;
    float shrinkDuration = 2;
    float startHight = 0;
    float startWidth = 0;
    float widthDecrease = 0.3f;
    float hightChange = 0;

    private bool RoutineActive;
    public void StartShrinking()
    {
        if (isShrinking)
            return;

        lastShrinkStartTime = Time.time;
        isShrinking = true;
        startWidth = gameObject.transform.localScale.x;
        startHight = gameObject.transform.localScale.y;
        hightChange = Random.Range(-5, 5);
        if (startHight - hightChange < 0)
        {
            hightChange = -2;
        }
        else if(startHight + hightChange > 22)
        { 
            hightChange = 2;
        }
    }


    void Update()
    {
        if (isShrinking)
        {
            if (Time.time - lastShrinkStartTime > shrinkDuration || gameObject.transform.localScale.x == startWidth - widthDecrease)
            {
                isShrinking = false;
            }
            else
            {
                //Math.f lerp doesn't work here don't suggest it
                float HValue = startHight - (hightChange * ((Time.time - lastShrinkStartTime) / shrinkDuration));
                float WValue = startWidth - (widthDecrease * ((Time.time - lastShrinkStartTime) / shrinkDuration));
                gameObject.transform.localScale = new Vector3(WValue, HValue, gameObject.transform.localScale.z);

            }
        }


    }
    public IEnumerator ColorChange(Color StartColor, Color EndColor)
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
