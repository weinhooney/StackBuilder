using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] CubeSpawner cubeSpawner;
    [SerializeField] CameraController cameraController;
    [SerializeField] UIController uiController;

    private bool isGameStart = false;
    private int currentScore = 0;

    private IEnumerator Start()
    {
        while(true)
        {
            // Perfect �׽�Ʈ��
            if(Input.GetMouseButtonDown(1))
            {
                if(null != cubeSpawner.CurrentCube)
                {
                    cubeSpawner.CurrentCube.transform.position = cubeSpawner.LastCube.position + Vector3.up * 0.1f;
                    cubeSpawner.CurrentCube.Arrangement();
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }

                cameraController.MoveOneStep();
                cubeSpawner.SpawnCube();
            }

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
                        // GameOverAnimation ����� �Ϸ�� ���Ŀ� OnGameOver() ȣ��
                        cameraController.GameOverAnimation(cubeSpawner.LastCube.position.y, OnGameOver);

                        yield break;
                    }

                    // ���� ���� ���� �� ���� ȭ�鿡 ���� ���� ����
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }

                // ī�޶��� y��ġ�� �̵� ť�� yũ�⸸ŭ �̵�
                cameraController.MoveOneStep();

                // �̵� ť�� ����
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }

    private void OnGameOver()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");

        if(highScore < currentScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            uiController.GameOver(true);
        }
        else
        {
            uiController.GameOver(false);
        }

        StartCoroutine("AfterGameOver");
    }

    private IEnumerator AfterGameOver()
    {
        yield return new WaitForEndOfFrame();

        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
