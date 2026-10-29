using JetBrains.Annotations;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
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
    public int Score;
    public TMP_Text ScoreText;
    public Canvas MainCanvas;




    private bool shootingDisabled = false;
    private bool grounded = true;
    private Rigidbody rb;
    private Camera cam;
    private Vector3 startPoint;
    private LineRenderer lineRenderer;
    private InputAction touchDrag;
    private Vector2 MainTouchVector;
    private Vector2 CurrentDragVector;
    private GameObject LastCollidedObject;

    public GameObject collidedObject;




    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        touchDrag = inputActions.FindAction("TouchDrag");
        GetComponent<Rigidbody>().mass = 1;
        color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        GetComponent<MeshRenderer>().material.color = color;
        GetComponent<LineRenderer>().material.color = color + new Color(0.1f,0.1f,0.1f, 0.5f);
        

        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            rb.linearVelocity = Vector3.zero;
            if (LastCollidedObject != collision.gameObject && LastCollidedObject != null)
            {
                Score++;
                ScoreText.text = "Score: " + Score;
                LastCollidedObject = collision.gameObject;
                collision.gameObject.GetComponent<MeshRenderer>().materials.ElementAt(0).color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));

            }
            else if (LastCollidedObject == null) 
            {
                LastCollidedObject = collision.gameObject;
                collision.gameObject.GetComponent<MeshRenderer>().materials.ElementAt(0).color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            }


        }
        else if (collision.gameObject.tag == "Destroyer")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AntiShoot")
        {
            shootingDisabled = true;
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        Debug.Log(_collision.gameObject.name);
        collidedObject = _collision.gameObject;

        // Should only continue if leavig ground
        if (_collision.gameObject.tag != "Ground" || _collision.gameObject == gameObject)
            return;

        _collision.gameObject.GetComponent<GroundScript>().StartShrinking();




    }

    void Update()
    {
        if (grounded == true && MainCanvas.GetComponent<PauseMenuScript>().isPaused == false && shootingDisabled == false)
        {
            // Check if drag released this frame
            Vector2 releaseDirection = GetReleasedDragDirection();
            if (releaseDirection != Vector2.zero && releaseDirection.y > 0)
            {
                lineRenderer.enabled = false;
                rb.AddForce(Vector2.ClampMagnitude(releaseDirection, maxDragDistance), ForceMode.Impulse);
                grounded = false;
            }
            if (releaseDirection != Vector2.zero && releaseDirection.y < 0)
            {
                lineRenderer.enabled = false;
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
        Vector3 velocity = initialForce / rb.mass;

        Vector3 startPos = transform.position;

        for (int i = 0; i < points.Length; i++)
        {
            float t = i * Time.fixedDeltaTime;
            Vector3 pos = (startPos + velocity * t + 0.5f * Physics.gravity * t * t);
            /*if (i != 0){
                pos = pos - new Vector3(0, (2 * i) + 0.5f * (-i ^ 2), 0);
            }*/
            pos.z = -1;
            points[(int)i] = pos;
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
