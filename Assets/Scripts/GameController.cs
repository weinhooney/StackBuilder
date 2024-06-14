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
                // ī�޶��� y��ġ�� �̵� ť�� yũ�⸸ŭ �̵�
                cameraController.MoveOneStep();

                // �̵� ť�� ����
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
