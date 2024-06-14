using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAxis { x = 0, z }

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

    private MoveAxis moveAxis = MoveAxis.x; // 현재 이동 축

    public void SpawnCube()
    {
        // 이동 큐브 생성
        Transform clone = Instantiate(movingCubePrefab);

        // 방금 생성한 이동 큐브의 위치
        // LastCube가 "StartCubeTop"인 경우는 게임을 시작하고 첫 이동 큐브를 생성할 때임
        if(null == LastCube || LastCube.name.Equals("StartCubeTop"))
        {
            // cubeSpawnPoints의 위치를 그대로 사용
            clone.position = cubeSpawnPoints[(int)moveAxis].position;
        }
        else
        {
            // 위치 설정
            float x = cubeSpawnPoints[(int)moveAxis].position.x;
            float z = cubeSpawnPoints[(int)moveAxis].position.z;

            // y축의 경우 LastCube 위치 + 프리팹의 y크기로 설정해 마지막으로 생성한 큐브보다 프리팹의 y크기만큼 더 높게 설정
            float y = LastCube.position.y + movingCubePrefab.localScale.y;

            clone.position = new Vector3(x, y, z);
        }


        // 방금 생성한 이동 큐브의 색상
        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        // cubeSpawnPoints 배열의 인덱스 변경
        moveAxis = (MoveAxis)(((int)moveAxis + 1) % cubeSpawnPoints.Length);

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
