using UnityEngine;

public class TitleFishMovement1 : MonoBehaviour
{
    public GameObject startLocation;
    public GameObject endLocation;
    public float moveSpeed = 2f;
    public float scaleSpeed = 1f;
    public float circleRadius = 1.5f;
    public float circleSpeed = 1f;
    public Vector3 startingScale = new Vector3(1f, 1f, 1f);
    public Vector3 endingScale = new Vector3(2f, 2f, 2f);

    private Vector3 targetScale;
    private Vector3 circleCenter;
    private float angle = 0f;
    private bool isMovingTowardsEnd = true;

    private void Awake()
    {
        startLocation = GameObject.FindGameObjectWithTag("startPos2");
        endLocation = GameObject.FindGameObjectWithTag("endPos2");
        transform.position = Vector3.zero;
        transform.localScale = startingScale;
        targetScale = endingScale;
        circleCenter = endLocation.transform.position;
    }

    private void Start()
    {
        transform.position = startLocation.transform.position;
    }

    private void Update()
    {
        if (isMovingTowardsEnd)
        {
            MoveTowardsEnd();
            ScaleUp();

            if (transform.position == endLocation.transform.position)
            {
                isMovingTowardsEnd = false;
                angle = 0f;
            }
        }
        else
        {
            CircleAround();
        }
    }

    private void MoveTowardsEnd()
    {
        transform.position = Vector3.MoveTowards(transform.position, endLocation.transform.position, moveSpeed * Time.deltaTime);
    }

    private void ScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
        {
            transform.localScale += (targetScale - startingScale) * Time.deltaTime * (scaleSpeed / 10);
        }
    }

    private void CircleAround()
    {
        angle += circleSpeed * Time.deltaTime;
        float x = circleCenter.x + Mathf.Cos(angle) * (circleRadius / 100);
        float y = circleCenter.y + Mathf.Sin(angle) * (circleRadius / 100);
        Vector3 circlePosition = new Vector3(x, y, transform.position.z);
        transform.position = circlePosition;
    }
}
