using UnityEngine;

public class GroundScript : MonoBehaviour
{
    float lastShrinkStartTime;
    bool isShrinking;
    float shrinkDuration = 2;
    float startWidth = 0;
    float widthDecrease = 0.3f;
    public void StartShrinking()
    {
        if (isShrinking)
            return;

        lastShrinkStartTime = Time.time;
        isShrinking = true;
        startWidth = gameObject.transform.localScale.x;
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
                float Value = startWidth - (widthDecrease * ((Time.time - lastShrinkStartTime) / shrinkDuration));
                gameObject.transform.localScale = new Vector3(Value, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

            }
        }
    }

}
