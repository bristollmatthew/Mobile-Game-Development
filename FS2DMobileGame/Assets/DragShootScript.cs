using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;



public class DragShootScript : MonoBehaviour
{
    public InputActionAsset inputActions;

    //These Variables MUST be public as Private Serialization of the fields breaks code 
    public float power = 10f;
    public float maxDragDistance = 5f;
    public int trajectoryResolution = 30;
    public Color color;
    public float scale;
    public GameObject gravitySlider;
    





    private Rigidbody2D rb;
    private Camera cam;
    private Vector3 startPoint;
    private LineRenderer lineRenderer;
    private InputAction touchDrag;
    private Vector2 MainTouchVector;
    private Vector2 CurrentDragVector;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnhancedTouchSupport.Enable();
        touchDrag = inputActions.FindAction("TouchDrag");
        print("start");
        GetComponent<Rigidbody2D>().gravityScale = gravitySlider.GetComponent<Slider>().value;
        scale = Random.Range(0.4f, 0.6f);
        color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        this.transform.localScale += new Vector3(scale, scale, scale);
        GetComponent<Renderer>().material.color = color;
        GetComponent<LineRenderer>().material.color = color;


        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }

    // Update is called once per frame
    /*private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }*/



    /*   private void OnCollisionEnter2D(Collision2D collision)
       {
           if (!collision.gameObject.GetComponent<Rigidbody2D>()) return;
           print("collided");
           if (collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude < this.gameObject.GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude && collision.gameObject.transform.localScale.x <= this.gameObject.transform.localScale.x) 
           {

               if (this.transform.localScale.x < 15f)
               {
                   this.transform.localScale += new Vector3(1f, 1f, 1f);
                   Destroy(collision.gameObject);
               }
               print("grow");
           }
       }
    */

    void Update()
    {
        MainTouchVector = CurrentTouch();
        GetComponent<Rigidbody2D>().gravityScale = gravitySlider.GetComponent<Slider>().value;


        if (MainTouchVector == new Vector2(-1, -1))
        {
            lineRenderer.enabled = false;
        }
        else if(MainTouchVector == new Vector2(-2, -2))
        {
            CurrentDragVector = Vector2.ClampMagnitude(CurrentDragVector, maxDragDistance);
            Vector2 force = CurrentDragVector * power;
            rb.AddForce(force, ForceMode2D.Impulse);


        }
        else
        {
            MainTouchVector = Vector2.ClampMagnitude(MainTouchVector, maxDragDistance);
            lineRenderer.enabled = true;
            ShowTrajectory(MainTouchVector * power);
            CurrentDragVector = MainTouchVector;
        }

        /*GetComponent<Rigidbody2D>().gravityScale = gravitySlider.GetComponent<Slider>().value;
        if () 
        {
            lineRenderer.enabled = true;

            startPoint = cam.ScreenToViewportPoint();
            startPoint.z = 0;
            buttonHeld = true;
        }
        if (touchDrag.ReadValue<float>() > 0.5f )
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0;

            Vector2 dragVector = startPoint - currentPoint;
            dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

                ShowTrajectory(dragVector * power);



        }
        if (touchDrag.ReadValue<float>() < 0.5f && buttonHeld == true)
        {
            buttonHeld = false;
                Vector3 endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 0;

                Vector2 force = (startPoint - endPoint) * power;
                rb.AddForce(force, ForceMode2D.Impulse);

            lineRenderer.enabled = false;


        }*/

        void ShowTrajectory(Vector2 initialForce)
            {
                lineRenderer.positionCount = trajectoryResolution;

                Vector3[] points = new Vector3[trajectoryResolution];
                Vector2 velocity = initialForce / rb.mass;
                Vector2 startPos = transform.position;

                for (int i = 0; i < points.Length; i++)
                {
                    float t = i * Time.fixedDeltaTime;
                    Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
                    points[i] = pos;
                }
                lineRenderer.SetPositions(points);
            }






    }

    UnityEngine.Vector2 CurrentTouch()
    {

        foreach (Touch CurrentTouch in Touch.activeTouches)
        {
            if (CurrentTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                return new Vector2 (-2, -2);
            }

                print(CurrentTouch.screenPosition - CurrentTouch.startScreenPosition);
                return (CurrentTouch.screenPosition - CurrentTouch.startScreenPosition);
            



        }
        return new Vector2(-1, -1);
    }



}
//Touch.screenpostion - //Touch.StartPostion = swipe direction
//Touch.screenpostion - //Touch.StartPostion = swipe direction
