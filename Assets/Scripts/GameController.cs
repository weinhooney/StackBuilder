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
                // ���� ���� �� ó�� ���콺 ���� ��ư�� ������ �� 1ȸ�� ȣ��
                if(false == isGameStart)
                {
                    isGameStart = true;
                    uiController.GameStart();
                }

                // CurrentCube�� null�� �ƴϸ� ť�� �̵� ����
                if(null != cubeSpawner.CurrentCube)
                {
                    bool isGameOver = cubeSpawner.CurrentCube.Arrangement();
                    if(isGameOver)
                    {
                        Debug.Log("GameOver");
                    }
                }

                // ī�޶��� y��ġ�� �̵� ť�� yũ�⸸ŭ �̵�
                cameraController.MoveOneStep();

                // �̵� ť�� ����
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
