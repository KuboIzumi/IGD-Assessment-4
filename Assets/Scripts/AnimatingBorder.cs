using UnityEngine;

public class AnimatingBorder : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 5.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startTime;
    private float journeyLength;

    void Start()
    {
        startPosition = transform.position;
        endPosition = targetPosition.position;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void Update()
    {
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        if (fractionOfJourney < 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
        }
        else
        {
            Vector3 temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;

            startTime = Time.time;
        }
    }
}


