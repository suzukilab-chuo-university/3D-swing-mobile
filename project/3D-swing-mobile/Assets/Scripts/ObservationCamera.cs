using UnityEngine;
using System.Collections;

public class ObservationCamera : MonoBehaviour {
    public Camera mainCamera; // 使用するカメラ
    public bool isAutoRotate = false; // 最初に自動で回転させておくかのフラグ
    public float cameraAngleRange = 40.0f;
    public float swipeTurnSpeed = 20.0f; // スワイプで回転するときの回転スピード
    public float pinchZoomSpeed = 100.0f; // ピンチするときのズームスピード
    public float autoRotateSpeed = 20.0f; // 自動で回転させるときの回転スピード

    private Vector3 baseMousePos; // 基準となるタップの座標
    private Vector3 baseCameraPos; // 基準となるカメラの座標
    private float basePinchDistance = 0f; //  // 基準となるピンチ時の指と指の距離
    private bool isMouseDown = false; // マウスが押されているかのフラグ
    private bool isPinchStart = true; // ピンチスタートしたかを管理するフラグ
    private float minCameraAngle; // カメラの最小角度
    private float maxCameraAngle; // カメラの最大角度

    private void Start()
    {
        minCameraAngle = 360.0f - cameraAngleRange;
        maxCameraAngle = cameraAngleRange;
    }

    void Update () {
		// 自動で回す
		if (isAutoRotate) {
            float angleY = this.transform.eulerAngles.y - Time.deltaTime * autoRotateSpeed;
			this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, angleY, 0);
		}

		// タップの種類の判定 & 対応処理
		if ((Input.touchCount == 1 && !isMouseDown)|| Input.GetMouseButtonDown(0)) {
			baseMousePos = Input.mousePosition;
			isMouseDown = true;
            isAutoRotate = false;
		} 
		else if (Input.touchCount == 2) {
			// ピンチでズーム用の処理群
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended)
            {
                isPinchStart = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved) {
                if (isPinchStart) {
                    isPinchStart = false;

                    basePinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    baseCameraPos = mainCamera.transform.localPosition;
                }

                float currentPinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                float pinchZoomDistance = (basePinchDistance - currentPinchDistance) * pinchZoomSpeed * 0.05f;
                float cameraPosZ = baseCameraPos.z - pinchZoomDistance;

                mainCamera.transform.localPosition = new Vector3 (mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, cameraPosZ);
            }
				
			isMouseDown = false;
            isAutoRotate = false;
		}

        // 指離した時の処理
		if (Input.GetMouseButtonUp(0)) {
			isMouseDown = false;
		}

		// スワイプ回転処理
		if (isMouseDown) {
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
