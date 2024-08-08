using UnityEngine;

public class swingManager : MonoBehaviour
{
    private Quaternion defaultRotation;
    private float beforeTime;
    private bool flagSin;
    private bool flagRotate;

    public static float angleValue = 20f;
    public static float speedValue = 0.2f;  // 周期

    void Start()
    {
        defaultRotation = transform.rotation;
        beforeTime = Time.time;
        flagSin = true;
        flagRotate = true;
    }

    void Update()
    {
        float time = Time.time;

        if (time >= beforeTime + 1.0f / speedValue)
        {
            beforeTime += 1.0f / speedValue;
        }

        float sin = Mathf.Sin(2 * Mathf.PI * speedValue * time);
        if(sin > 0 && flagSin == false)
        {
            flagSin = true;
            if (flagRotate) flagRotate = false;
            else flagRotate = true;
        }
        else if (sin < 0)
        {
            flagSin = false;
        }

        if (flagRotate)
        {
            transform.rotation = Quaternion.AngleAxis(sin * angleValue, Vector3.up) * defaultRotation;
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(sin * angleValue, Vector3.left) * defaultRotation;
        }
    }
}
