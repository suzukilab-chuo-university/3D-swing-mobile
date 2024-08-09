using UnityEngine;
using System.Collections;

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

    private void Start()
    {
        minCameraAngle = 360.0f - cameraAngleRange;
        maxCameraAngle = cameraAngleRange;
        beforeTime = Time.time;
    }

    void Update () {
        // 自動で回す
        if (isAutoRotate)
        {
            float time = Time.time - beforeTime;
            float sin = Mathf.Sin(2 * Mathf.PI * autoRotateSpeed * time);
            float angleY = sin * cameraAngleRange;
            this.transform.eulerAngles = new Vector3(0, angleY, 0);
        }
        else
        {
            // タップの種類の判定 & 対応処理
            if ((Input.touchCount == 1 && !isMouseDown) || Input.GetMouseButtonDown(0))
            {
                baseMousePos = Input.mousePosition;
                isMouseDown = true;
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

                // カメラのアングルに制限をかける もっとイカした書き方にしたい
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

    public void OnClickAuto()
    {
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        beforeTime = Time.time;
        isAutoRotate = true;
    }
    public void OnClickSwipe()
    {
        isAutoRotate = false;
    }
}
