using UnityEngine;

public class swingManager : MonoBehaviour
{
    private Quaternion defaultRotation;
    private float beforeTime;
    public static float angleValue = 20f;
    public static float speedValue = 0.2f;  // 周期

    void Start()
    {
        defaultRotation = transform.rotation;
        beforeTime = Time.time;
    }

    void Update()
    {
        float time = Time.time;

        if (time >= beforeTime + 1.0f / speedValue)
        {
            beforeTime += 1.0f / speedValue;
        }

        float sin = Mathf.Sin(2 * Mathf.PI * speedValue * time);
        transform.rotation = Quaternion.AngleAxis(sin * angleValue, Vector3.up) * defaultRotation;
    }
}
