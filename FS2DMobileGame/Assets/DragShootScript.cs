using UnityEngine;

public class DragShootScript : MonoBehaviour
{
    public float power = 10f;
    public float maxDragDistance = 5f;
    public int trajectoryResolution = 30;

    private Rigidbody2D rb;
    private Camera cam;
    private Vector3 startPoint;
    private LineRenderer lineRenderer;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            startPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 0;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0;

            Vector2 dragVector = startPoint - currentPoint;
            dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

            ShowTrajectory(dragVector * power);


        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 0;

            Vector2 force = (startPoint - endPoint) * power;
            rb.AddForce(force, ForceMode2D.Impulse);

            lineRenderer.enabled = false;
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
