using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    private Vector3 moveDirection;

    private CubeSpawner cubeSpawner;
    private MoveAxis moveAxis;
    private PerfectController perfectController;

    public void Setup(CubeSpawner cubeSpawner, PerfectController perfectController, MoveAxis moveAxis)
    {
        this.cubeSpawner = cubeSpawner;
        this.perfectController = perfectController;
        this.moveAxis = moveAxis;

        if(MoveAxis.x == moveAxis)
        {
            moveDirection = Vector3.left;
        }
        else if(MoveAxis.z == moveAxis)
        {
            moveDirection = Vector3.back;
        }
    }

    private void Update()
    {
        // 이동 축을 기준으로 -1.5 ~ 1.5를 왕복 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if(MoveAxis.x == moveAxis)
        {
            if(transform.position.x <= -1.5f)
            {
                moveDirection = Vector3.right;
            }
            else if(1.5f <= transform.position.x)
            {
                moveDirection = Vector3.left;
            }
        }
        else if(MoveAxis.z == moveAxis)
        {
            if(transform.position.z <= -1.5f)
            {
                moveDirection = Vector3.forward;
            }
            else if(1.5f <= transform.position.z)
            {
                moveDirection = Vector3.back;
            }
        }
    }

    public bool Arrangement()
    {
        moveSpeed = 0;

        float hangOver = GetHangOver();

        if(IsGameOver(hangOver))
        {
            return true;
        }

        // 퍼펙트 여부 검사
        bool isPerfect = perfectController.IsPerfect(hangOver);

        // isPerfect가 false일 때만 조각 큐브 생성
        if(false == isPerfect)
        {
            float direction = 0 <= hangOver ? 1 : -1;

            if (MoveAxis.x == moveAxis)
            {
                SplitCubeOnX(hangOver, direction);
            }
            else if (MoveAxis.z == moveAxis)
            {
                SplitCubeOnZ(hangOver, direction);
            }
        }

        // 현재 이동중인 큐브를 정지해서 배치했기 때문에 배치되어 있는 큐브 중
        // 가장 상단에 있는 큐브를 LastCube 프로퍼티에 저장
        cubeSpawner.LastCube = this.transform;

        return false;
    }

    private float GetHangOver()
    {
        float amount = 0;

        // x축으로 이동 중일 때는 x축에 겹치지 않는 부분이 발생하고,
        // z축으로 이동 중일 때는 z축에 겹치지 않는 부분이 발생
        if(MoveAxis.x == moveAxis)
        {
            amount = transform.position.x - cubeSpawner.LastCube.transform.position.x;
        }
        else if(MoveAxis.z == moveAxis)
        {
            amount = transform.position.z - cubeSpawner.LastCube.transform.position.z;
        }

        return amount;
    }

    private void SplitCubeOnX(float hangOver, float direction)
    {
        // 이동 큐브의 새로운 위치, 크기 연산
        float newXPosition = transform.position.x - (hangOver / 2);
        float newXSize = transform.localScale.x - Mathf.Abs(hangOver);

        // 이동 큐브의 위치, 크기 설정
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);

        // 조각 큐브의 위치, 크기 연산
        float cubeEdge = transform.position.x + (transform.localScale.x / 2 * direction); // 이동 큐브와 조각 큐브의 경계 위치
        float fallingBlockSize = Mathf.Abs(hangOver); // 조각 큐브의 크기
        float fallingBlockPosition = cubeEdge + fallingBlockSize / 2 * direction; // 조각 큐브의 위치

        // 조각 큐브 생성
        SpawnDropCube(fallingBlockPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangOver, float direction)
    {
        // 이동 큐브의 새로운 위치, 크기 연산
        float newZPosition = transform.position.z - (hangOver / 2);
        float newZSize = transform.localScale.z - Mathf.Abs(hangOver);

        // 이동 큐브의 위치, 크기 설정
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);

        // 조각 큐브의 위치, 크기 연산
        float cubeEdge = transform.position.z + (transform.localScale.z / 2 * direction);
        float fallingBlockSize = Mathf.Abs(hangOver);
        float fallingBlockPosition = cubeEdge + fallingBlockSize / 2 * direction;

        // 조각 큐브 생성
        SpawnDropCube(fallingBlockPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        GameObject clone = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 방금 생성한 조각 큐브의 위치, 크기 설정
        if(MoveAxis.x == moveAxis)
        {
            clone.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
            clone.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
        }
        else if(MoveAxis.z == moveAxis)
        {
            clone.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
            clone.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        }

        // 방금 생성한 조각 큐브의 색상 설정
        clone.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;

        // 방금 생성한 조각 큐브가 중력을 받아 아래로 떨어지도록 설정
        clone.AddComponent<Rigidbody>();

        // 2초 뒤에 삭제
        Destroy(clone, 2);
    }

    private bool IsGameOver(float hangOver)
    {
        // 이동 큐브와 LastCube의 겹치지 않는 부분(hangOver)값이 LastCube의 크기보다 큰 경우는
        // 아예 겹치지 않는다는 의미이기 때문에 게임 오버로 처리
        float max = MoveAxis.x == moveAxis ? cubeSpawner.LastCube.transform.localScale.x : cubeSpawner.LastCube.transform.localScale.z;

        if(max < Mathf.Abs(hangOver))
        {
            return true;
        }

        return false;
    }

    public void RecoveryCube()
    {
        float recoverySize = 0.1f;

        if(MoveAxis.x == moveAxis)
        {
            float newXSize = transform.localScale.x + recoverySize;
            float newXPosition = transform.position.x + recoverySize * 0.5f;

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            float newZSize = transform.localScale.z + recoverySize;
            float newZPosition = transform.position.z + recoverySize * 0.5f;

            transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        }
    }
}
