using UnityEngine;

public class SlowHover : MonoBehaviour
{
    public float hoverRange = 0.5f; // Range of vertical movement
    public float hoverSpeed = 1.0f; // Speed of vertical movement

    private Vector3 startPosition;
    private float currentHoverOffset = 0.0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new vertical position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverRange;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

