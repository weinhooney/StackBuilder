using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] CubeSpawner cubeSpawner;
    [SerializeField] CameraController cameraController;

    private IEnumerator Start()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // 카메라의 y위치를 이동 큐브 y크기만큼 이동
                cameraController.MoveOneStep();

                // 이동 큐브 생성
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
