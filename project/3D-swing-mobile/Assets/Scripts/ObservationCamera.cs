using UnityEngine;

public class ObservationCamera : MonoBehaviour {
    public Camera mainCamera; // 使用するカメラ
    public bool isAutoRotate = true; // 最初に自動で回転させておくかのフラグ
    public float cameraAngleRange = 50.0f;
    public float swipeTurnSpeed = 20.0f; // スワイプで回転するときの回転スピード

    private float autoRotateSpeed = 0.25f; // 自動で回転させるときの回転スピード
    private Vector3 baseMousePos; // 基準となるタップの座標
    private bool isMouseDown = false; // マウスが押されているかのフラグ
    private float minCameraAngle; // カメラの最小角度
    private float maxCameraAngle; // カメラの最大角度
    private float beforeTime;     // スイングを始めた時刻

    private float pinchZoomSpeed = 1f; // ピンチするときのズームスピード
    private bool isPinchStart = true; // ピンチスタートしたかを管理するフラグ
    private float basePinchZoomDistanceX = 0f; // ズームの基準となるピンチの距離 x
    private float basePinchZoomDistanceY = 0f; // ズームの基準となるピンチの距離 y
    private float basePinchDistance = 0f; //  // 基準となるピンチ時の指と指の距離
    private Vector3 baseCameraPos; // 基準となるカメラの座標

    private void Start()
    {
        minCameraAngle = 360.0f - cameraAngleRange;
        maxCameraAngle = cameraAngleRange;
        beforeTime = Time.time;
    }

    void Update () {
        // 自動でスイングする
        if (isAutoRotate)
        {
            float time = Time.time - beforeTime;
            float sin = Mathf.Sin(2 * Mathf.PI * autoRotateSpeed * time);
            float angleY = sin * cameraAngleRange;
            this.transform.eulerAngles = new Vector3(0, angleY, 0);
        }

        // タッチ操作
        else
        {
            // タップの種類の判定 & 対応処理
            if ((Input.touchCount == 1 && !isMouseDown) || Input.GetMouseButtonDown(0))
            {
                baseMousePos = Input.mousePosition;
                isMouseDown = true;
            }
            else if (Input.touchCount == 2)
            {
                // ピンチでズーム用の処理群
                if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended)
                {
                    isPinchStart = true;
                }
                else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
                {
                    if (isPinchStart)
                    {
                        isPinchStart = false;

                        basePinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        baseCameraPos = mainCamera.transform.localPosition;
                    }

                    float currentPinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    float pinchZoomDistance = (basePinchDistance - currentPinchDistance) * pinchZoomSpeed * 0.05f;
                    float cameraPosZ = baseCameraPos.z - pinchZoomDistance;
                    if (cameraPosZ < 0f)
                    {
                        cameraPosZ = 0f;
                    }
                    else if (cameraPosZ > 5f)
                    {
                        cameraPosZ = 5f;
                    }

                    mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, cameraPosZ);
                }

                isMouseDown = false;
                isAutoRotate = false;
            }

            // 指離した時の処理
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }

            // スワイプ回転処理
            if (isMouseDown)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 distanceMousePos = (mousePos - baseMousePos);
                float angleX = this.transform.eulerAngles.x - distanceMousePos.y * swipeTurnSpeed * 0.01f;
                float angleY = this.transform.eulerAngles.y + distanceMousePos.x * swipeTurnSpeed * 0.01f;

                if (!((angleX >= -10f && angleX <= maxCameraAngle) || (angleX >= minCameraAngle && angleX <= 370f)))
                {
                    angleX = this.transform.eulerAngles.x;
                }
                if (!((angleY >= -10f && angleY <= maxCameraAngle) || (angleY >= minCameraAngle && angleY <= 370f)))
                {
                    angleY = this.transform.eulerAngles.y;
                }
                this.transform.eulerAngles = new Vector3(angleX, angleY, 0);

                baseMousePos = mousePos;
            }
        }
	}

    // スイングのボタンが押されたら
    public void OnClickAuto()
    {
        mainCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
        this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        beforeTime = Time.time;
        isAutoRotate = true;
    }

    // タッチ操作のボタンが押されたら
    public void OnClickSwipe()
    {
        isAutoRotate = false;
    }
}
