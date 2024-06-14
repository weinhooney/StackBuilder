using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAxis { x = 0, z }

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

    private MoveAxis moveAxis = MoveAxis.x; // ���� �̵� ��

    public void SpawnCube()
    {
        // �̵� ť�� ����
        Transform clone = Instantiate(movingCubePrefab);

        // ��� ������ �̵� ť���� ��ġ
        // LastCube�� "StartCubeTop"�� ���� ������ �����ϰ� ù �̵� ť�긦 ������ ����
        if(null == LastCube || LastCube.name.Equals("StartCubeTop"))
        {
            // cubeSpawnPoints�� ��ġ�� �״�� ���
            clone.position = cubeSpawnPoints[(int)moveAxis].position;
        }
        else
        {
            // ��ġ ����
            float x = cubeSpawnPoints[(int)moveAxis].position.x;
            float z = cubeSpawnPoints[(int)moveAxis].position.z;

            // y���� ��� LastCube ��ġ + �������� yũ��� ������ ���������� ������ ť�꺸�� �������� yũ�⸸ŭ �� ���� ����
            float y = LastCube.position.y + movingCubePrefab.localScale.y;

            clone.position = new Vector3(x, y, z);
        }


        // ��� ������ �̵� ť���� ����
        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        // cubeSpawnPoints �迭�� �ε��� ����
        moveAxis = (MoveAxis)(((int)moveAxis + 1) % cubeSpawnPoints.Length);

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
