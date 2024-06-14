using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] cubeSpawnPoints; // 큐브 생성 위치(x, z)
    [SerializeField] private Transform movingCubePrefab; // 이동 큐브 프리팹

    // 새로운 큐브 생성에 필요한 위치/크기 정보, 조각 큐브 제작, 게임오버 감사 등에 사용
    [field: SerializeField] public Transform LastCube { get; set; } // 마지막에 생성한 큐브 정보

    [SerializeField] private float colorWeight = 15.0f; // 색상의 비슷한 정도(값이 작을수록 더 비슷한 색상)

    // 완전히 새로운 색상으로 변경하기 위한 현재 횟수, 최대 횟수
    private int currentColorNumberOfTime = 5;
    private int maxColorNumberOfTIme = 5;

    public void SpawnCube()
    {
        // 이동 큐브 생성
        Transform clone = Instantiate(movingCubePrefab);

        // 방금 생성한 이동 큐브의 색상
        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        // 방금 생성한 이동 큐브의 정보를 LastCube에 저장
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
        // maxNumberOfTime에 설정된 5회 동안은 색상이 점진적으로 변화하고,
        // 매 5회마다 한 번씩 완전히 새로운 색상으로 설정
        Color color = Color.white;

        // 현재 색상에서 rgb값이 조금 바뀐 비슷한 색상
        if(0 < currentColorNumberOfTime)
        {
            float colorAmount = (1.0f / 255.0f) * colorWeight;
            color = LastCube.GetComponent<MeshRenderer>().material.color;
            color = new Color(color.r - colorAmount, color.g - colorAmount, color.b - colorAmount);

            currentColorNumberOfTime--;
        }
        else // 완전히 다른 색상
        {
            color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

            currentColorNumberOfTime = maxColorNumberOfTIme;
        }

        return color;
    }
}
