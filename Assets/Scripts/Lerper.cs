using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float speed = 1f;

    private float t = 0f;  // Parameter for lerp calculation
    private bool movingToEnd = true;  // Flag to indicate the direction of movement

    private void Start()
    {
        // Set the initial position of the object to the start position
        transform.position = startPos.position;
    }

    private void Update()
    {
        // Calculate the new position using lerp
        if (movingToEnd)
        {
            t += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
        }
        else
        {
            t -= speed * Time.deltaTime;
            transform.position = Vector3.Lerp(endPos.position, startPos.position, t);
        }

        // Check if the object has reached the end position or the start position
        if (t >= 1f)
        {
            movingToEnd = false;
        }
        else if (t <= 0f)
        {
            movingToEnd = true;
        }
    }
}
