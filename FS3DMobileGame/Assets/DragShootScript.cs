using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using TMPro;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;


public class DragShootScript : MonoBehaviour
{
    public InputActionAsset inputActions;

    //These Variables MUST be public as Private Serialization of the fields breaks code 
    public float power = 10f;
    public float maxDragDistance = 5f;
    public int trajectoryResolution = 30;
    public Color color;
    public int Score;
    public TMP_Text ScoreText;
    




    private bool grounded = true;
    private Rigidbody rb;
    private Camera cam;
    private Vector3 startPoint;
    private LineRenderer lineRenderer;
    private InputAction touchDrag;
    private Vector2 MainTouchVector;
    private Vector2 CurrentDragVector;
    private GameObject LastCollidedObject;



    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        touchDrag = inputActions.FindAction("TouchDrag");
        print("start");
        GetComponent<Rigidbody>().mass = 1;
        color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        GetComponent<MeshRenderer>().material.color = color;
        GetComponent<LineRenderer>().material.color = color + new Color(0.1f,0.1f,0.1f);


        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
            rb.linearVelocity = Vector3.zero;
            if(LastCollidedObject != collision.gameObject)
            {
                Score = Score + 1;
                ScoreText.text = "Score: " + Score;
                LastCollidedObject = collision.gameObject;
                collision.gameObject.GetComponent<MeshRenderer>().materials.ElementAt(0).color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));

            }


        }
        else if(collision.gameObject.tag == "Destroyer")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<Transform>().localScale = collision.gameObject.GetComponent<Transform>().localScale - new Vector3(0.1f, 0f, 0f);
    }


    void Update()
    {
        if (grounded == true)
        {
            // Check if drag released this frame
            Vector2 releaseDirection = GetReleasedDragDirection();
            if (releaseDirection != Vector2.zero)
            {
                lineRenderer.enabled = false;
                rb.AddForce(Vector2.ClampMagnitude(releaseDirection, maxDragDistance), ForceMode.Impulse);
                grounded = false;
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
