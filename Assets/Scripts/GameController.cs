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
                        OnGameOver();

                        yield break;
                    }

                    // 현재 점수 증가 및 게임 화면에 점수 정보 갱신
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }

                // 카메라의 y위치를 이동 큐브 y크기만큼 이동
                cameraController.MoveOneStep();

                // 이동 큐브 생성
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
