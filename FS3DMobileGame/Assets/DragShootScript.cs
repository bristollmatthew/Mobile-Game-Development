using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;



public class DragShootScript : MonoBehaviour
{
    public InputActionAsset inputActions;

    //These Variables MUST be public as Private Serialization of the fields breaks code 
    public float power = 10f;
    public float maxDragDistance = 5f;
    public int trajectoryResolution = 30;
    public Color color;
    
    





    private Rigidbody rb;
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
        GetComponent<Rigidbody>().mass = 1;
        color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        GetComponent<MeshRenderer>().material.color = color;
        GetComponent<LineRenderer>().material.color = color;


        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }



    void Update()
    {
        // Check if drag released this frame
        Vector2 releaseDirection = GetReleasedDragDirection();
        if (releaseDirection != Vector2.zero)
        {
            lineRenderer.enabled = false;
            rb.AddForce(Vector2.ClampMagnitude(releaseDirection, maxDragDistance), ForceMode.Impulse);
        }
        else 
        {
            // If Not Released Do This
            Vector2 lineDirection = GetCurrentDragDirection();
            if (lineDirection != Vector2.zero) 
            {
                lineRenderer.enabled = true;
                ShowTrajectory(lineDirection * power);
            }
        }




    }
    void ShowTrajectory(Vector2 initialForce)
    {
        lineRenderer.positionCount = trajectoryResolution;

        Vector3[] points = new Vector3[trajectoryResolution];

        //Because I am changing the mass of the rigid body this could break things:: \_(0_0)_/
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

    Vector2 GetCurrentDragDirection()
    {
        foreach (Touch CurrentTouch in Touch.activeTouches)
        {
            if (CurrentTouch.isInProgress)
            {
                return CurrentTouch.screenPosition - CurrentTouch.startScreenPosition;
            }
        }
        return Vector2.zero;
    }

    Vector2 GetReleasedDragDirection()
    {
        foreach (Touch CurrentTouch in Touch.activeTouches)
        {
            if (CurrentTouch.phase == TouchPhase.Ended)
            {
                return CurrentTouch.screenPosition - CurrentTouch.startScreenPosition;
            }
        }
        return Vector2.zero;
    }



}
