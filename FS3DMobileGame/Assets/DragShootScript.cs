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
    public GameObject Particle;
    public int stepmultiplier;

    public GameObject Platform1;
    public GameObject Platform2;




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

    public GameObject SaveGame;
    private GameSaveScript SavegamePointer;


    public GameObject collidedObject;
    const int RGBMax = 255;


    



    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*        if (SavegamePointer == null)
                {
                    throw new InvalidDataSourceException("Error Getting SaveGame, No Save Game Found, Failed To Create New Save Game");
                }*/

        EnhancedTouchSupport.Enable();
        Application.targetFrameRate = 60;
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

        SavegamePointer = GameSaveScript.First;


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            rb.linearVelocity = Vector3.zero;
            if (LastCollidedObject != collision.gameObject && LastCollidedObject != null)
            {
                int HiScore = SavegamePointer.currentData.highscore;
                //In-game Stuff
                Score++;
                LastCollidedObject = collision.gameObject;
                collision.gameObject.GetComponent<MeshRenderer>().materials.ElementAt(0).color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                Instantiate(Particle, this.transform.position - new Vector3(0f,0.5f,4f), new Quaternion(0,-180,0,0));
                var main = Particle.gameObject.GetComponent<ParticleSystem>().main;
                main.startColor = collision.gameObject.GetComponent<MeshRenderer>().materials.ElementAt(0).color;





                //Save Data Stuff
                if (SavegamePointer.currentData.highscore <= Score)
                {

                    SavegamePointer.currentData.highscore = Score;
                }
                SavegamePointer.SaveGame(SavegamePointer.currentData);

                //Output


                //HiScore displays as 1 less than score when a new save is created, and score both Score and HiScore = 1
                //This is a temporary Fix & Unoptimal Do Not Allow in Final Build
                //If This Makes It To Final Build Richard Please Don't Let Me Hear The End Of It

                if (HiScore == 0 & Score == 1)
                {
                    int DisplayHiScore = 1;
                    ScoreText.text = "Score: " + Score + " Hi: " + DisplayHiScore;
                }
                else
                {
                    int DisplayHiScore = HiScore + 1;
                    ScoreText.text = "Score: " + Score + " Hi: " + DisplayHiScore;
                }

                //Update Color

                Color Platform1Color = Platform1.GetComponent<MeshRenderer>().material.color;
                Color Platform2Color = Platform2.GetComponent<MeshRenderer>().material.color;

                Color NEWcolor = new Color(((Platform1Color.r + Platform2Color.r) / 2f), ((Platform1Color.g + Platform2Color.g) / 2f), ((Platform1Color.b + Platform2Color.b) / 2f), 1f);

                GetComponent<MeshRenderer>().material.color = NEWcolor;
                GetComponent<LineRenderer>().material.color = NEWcolor + new Color(0.1f, 0.1f, 0.1f, 0.5f);



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
            lineRenderer.enabled = false;
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        collidedObject = _collision.gameObject;

        // Should only continue if leaving ground
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
        Vector3 velocity = initialForce / rb.mass * Time.fixedDeltaTime;

        Vector3 startPos = transform.position;

        float midpointTime = -velocity.y / Physics.gravity.y;
        float scalar = midpointTime / (stepmultiplier * points.Length);

        for (int i = 0; i < points.Length; i++)
        {
            float t = i * scalar;
            float step = t * stepmultiplier;
            Vector3 pos = (startPos + velocity * step + 0.5f * Physics.gravity * step * step);

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

    Color InvertColor( Color color)
    {

        return new Color(RGBMax - color.r, RGBMax - color.g, RGBMax - color.b, 100);
    }


}
