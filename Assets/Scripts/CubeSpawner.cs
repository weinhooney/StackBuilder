using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] cubeSpawnPoints; // ť�� ���� ��ġ(x, z)
    [SerializeField] private Transform movingCubePrefab; // �̵� ť�� ������

    // ���ο� ť�� ������ �ʿ��� ��ġ/ũ�� ����, ���� ť�� ����, ���ӿ��� ���� � ���
    [field: SerializeField] public Transform LastCube { get; set; } // �������� ������ ť�� ����

    [SerializeField] private float colorWeight = 15.0f; // ������ ����� ����(���� �������� �� ����� ����)

    // ������ ���ο� �������� �����ϱ� ���� ���� Ƚ��, �ִ� Ƚ��
    private int currentColorNumberOfTime = 5;
    private int maxColorNumberOfTIme = 5;

    public void SpawnCube()
    {
        // �̵� ť�� ����
        Transform clone = Instantiate(movingCubePrefab);

        // ��� ������ �̵� ť���� ����
        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        // ��� ������ �̵� ť���� ������ LastCube�� ����
        LastCube = clone;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < cubeSpawnPoints.Length; ++i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cubeSpawnPoints[i].transform.position, movingCubePrefab.localScale);
        }
    }

    private Color GetRandomColor()
    {
        // maxNumberOfTime�� ������ 5ȸ ������ ������ ���������� ��ȭ�ϰ�,
        // �� 5ȸ���� �� ���� ������ ���ο� �������� ����
        Color color = Color.white;

        // ���� ���󿡼� rgb���� ���� �ٲ� ����� ����
        if(0 < currentColorNumberOfTime)
        {
            float colorAmount = (1.0f / 255.0f) * colorWeight;
            color = LastCube.GetComponent<MeshRenderer>().material.color;
            color = new Color(color.r - colorAmount, color.g - colorAmount, color.b - colorAmount);

            currentColorNumberOfTime--;
        }
        else // ������ �ٸ� ����
        {
            color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

            currentColorNumberOfTime = maxColorNumberOfTIme;
        }

        return color;
    }
}
