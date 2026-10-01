using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragShootScript : MonoBehaviour
{
    public float power = 10f;
    public float maxDragDistance = 5f;
    public int trajectoryResolution = 30;
    public Color color;
    public float scale;
    public GameObject gravitySlider;


    private float currentTime = 0;
    private bool buttonHeld = false;

    private Rigidbody2D rb;
    private Camera cam;
    private Vector3 startPoint;
    private LineRenderer lineRenderer;
    




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        print("start");
        GetComponent<Rigidbody2D>().gravityScale = gravitySlider.GetComponent<Slider>().value;
        scale = Random.Range(0.01f, 0.6f);
        color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f));
        this.transform.localScale += new Vector3(scale, scale, scale);
        GetComponent<Renderer>().material.color = color;
        GetComponent<LineRenderer>().startColor = color;
        GetComponent<LineRenderer>().endColor = color;


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


        GetComponent<Rigidbody2D>().gravityScale = gravitySlider.GetComponent<Slider>().value;
        if (Input.GetMouseButtonDown(0)) 
        {
            if (buttonHeld == false)
            {
                currentTime = Time.time;
            }
            buttonHeld = true; 

            startPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 0;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0;

            Vector2 dragVector = startPoint - currentPoint;
            dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

            if (Time.time - currentTime > 0.15f)
            {
                ShowTrajectory(dragVector * power);
            }


        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - currentTime > 0.12f)
            {
                Vector3 endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 0;

                Vector2 force = (startPoint - endPoint) * power;
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            lineRenderer.enabled = false;

            buttonHeld = false;
        }

        void ShowTrajectory(Vector2 initialForce)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = trajectoryResolution;

            Vector3[] points = new Vector3[trajectoryResolution];
            Vector2 velocity = initialForce / rb.mass;
            Vector2 startPos = transform.position;

            for (int i = 0; i < points.Length; i++)
            {
                float t = i * Time.fixedDeltaTime;
                Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
                points[i] = pos;


                lineRenderer.SetPositions(points);
            }

        }

    }
}
