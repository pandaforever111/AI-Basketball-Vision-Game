using UnityEngine;
using UnityEngine.UI; // If you need to display the score using a UI Text

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    private Camera _mainCamera;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _dragStart;
    private bool _isDragging;

    public LineRenderer trajectoryLine;
    public Text scoreText; // Assign a UI Text component in the inspector to display the score

    [Header("Settings")]
    public float forceMultiplier = 10f; // Exposed in Inspector for fine-tuning the launch force

    private int _score;

    void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _score = 0;
        UpdateScoreText();

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    void Update()
    {
        HandleDrag();
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                _isDragging = true;
                _dragStart = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));
                _dragStart.z = 0;

                _rigidbody2D.velocity = Vector2.zero; // Stop any existing momentum
                _rigidbody2D.isKinematic = true; // Stop the ball from falling due to gravity

                if (trajectoryLine != null)
                {
                    trajectoryLine.enabled = true;
                }
            }
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            Vector3 currentDragPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));
            currentDragPosition.z = 0;

            // Update trajectory visualization
            if (trajectoryLine != null)
            {
                Vector3 direction = _dragStart - currentDragPosition;
                Vector3 launchForce = direction * forceMultiplier;

                Vector3[] points = CalculateTrajectory(transform.position, launchForce, _rigidbody2D.gravityScale);
                trajectoryLine.positionCount = points.Length;
                trajectoryLine.SetPositions(points);
            }
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            Vector3 dragEnd = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));
            dragEnd.z = 0;

            Vector3 force = (_dragStart - dragEnd) * forceMultiplier;
            _rigidbody2D.isKinematic = false; // Re-enable physics
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;
            }
        }
    }

    private Vector3[] CalculateTrajectory(Vector3 startPosition, Vector3 initialVelocity, float gravityScale)
    {
        int resolution = 30; // Number of points in the trajectory
        float timeStep = 0.1f; // Time step between points
        Vector3[] trajectoryPoints = new Vector3[resolution];

        float gravity = Physics2D.gravity.y * gravityScale;
        for (int i = 0; i < resolution; i++)
        {
            float t = i * timeStep;
            float x = startPosition.x + initialVelocity.x * t;
            float y = startPosition.y + initialVelocity.y * t + 0.5f * gravity * t * t;
            trajectoryPoints[i] = new Vector3(x, y, 0);
        }

        return trajectoryPoints;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hoop"))
        {
            _score++;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + _score;
        }
    }
}
