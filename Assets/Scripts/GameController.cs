using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] CubeSpawner cubeSpawner;
    [SerializeField] CameraController cameraController;
    [SerializeField] UIController uiController;

    private bool isGameStart = false;

    private IEnumerator Start()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // 게임 시작 후 처음 마우스 왼쪽 버튼을 눌렀을 때 1회만 호출
                if(false == isGameStart)
                {
                    isGameStart = true;
                    uiController.GameStart();
                }

                // CurrentCube가 null이 아니면 큐브 이동 중지
                if(null != cubeSpawner.CurrentCube)
                {
                    bool isGameOver = cubeSpawner.CurrentCube.Arrangement();
                    if(isGameOver)
                    {
                        Debug.Log("GameOver");
                    }
                }

                // 카메라의 y위치를 이동 큐브 y크기만큼 이동
                cameraController.MoveOneStep();

                // 이동 큐브 생성
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
