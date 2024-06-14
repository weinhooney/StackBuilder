using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [Header("One Step Move Parameters")]
    [SerializeField] private float moveDistance = 0.1f;
    [SerializeField] private float oneStepMoveTime = 0.25f;

    [Header("Game Over Paramters")]
    [SerializeField] private float gameOverAnimationTime = 1.5f;
    [SerializeField] private float limitMinY = 4; // �ִϸ��̼� ����� ���� LastCube�� �ּ� y��ġ

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void MoveOneStep()
    {
        // ���� ��ġ���� �̵� ť���� yũ���� 0.1��ŭ ���� �̵�
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.up * moveDistance;

        StartCoroutine(OnMoveTo(start, end, oneStepMoveTime));
    }

    private IEnumerator OnMoveTo(Vector3 start, Vector3 end, float time)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    public void GameOverAnimation(float lastCubeY, UnityAction action = null)
    {
        // �������� ��ġ�� LastCube�� y��ġ�� limitMinY���� ������ �ִϸ��̼� ��� ����
        if(lastCubeY < limitMinY)
        {
            if (null != action)
            {
                action.Invoke();
            }

            return;
        }

        // ī�޶� y ��ġ ����
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, lastCubeY + 1, transform.position.z);

        // ī�޶� �̵� �ִϸ��̼�
        StartCoroutine(OnMoveTo(startPosition, endPosition, gameOverAnimationTime));

        // ī�޶� View ũ�� ����
        float startSize = mainCamera.orthographicSize;
        float endSize = lastCubeY - 1;

        // ī�޶� View ũ�� ���� �ִϸ��̼�
        StartCoroutine(OnOrthographicSizeTo(startSize, endSize, gameOverAnimationTime, action));
    }

    private IEnumerator OnOrthographicSizeTo(float start, float end, float time, UnityAction action)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.orthographicSize = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        if(null != action)
        {
            action.Invoke();
        }
    }
}
