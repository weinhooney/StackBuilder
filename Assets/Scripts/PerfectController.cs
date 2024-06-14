using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;
    [SerializeField] private Transform perfectEffect;

    private AudioSource audioSource;

    private float perfectCorrection = 0.01f; // Perfect로 인정하는 보정값
    private float addedSize = 0.1f; // 기존 큐브 크기에 더해지는 값
    private int perfectCombo = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsPerfect(float hangOver)
    {
        // Perfect로 콤보가 중첩될 때
        if(Mathf.Abs(hangOver) <= perfectCorrection)
        {
            EffectProcess(); // 콤보에 따라 이펙트 재생
            SFXProcess(); // 콤보에 따라 사운드 재생

            perfectCombo++;

            return true;
        }
        else // 콤보 초기화
        {
            perfectCombo = 0;

            return false;
        }
    }

    private void EffectProcess()
    {
        // 기본 퍼펙트 이펙트 생성
        OnPerfectEffect();
    }

    private void OnPerfectEffect()
    {
        // 이펙트 생성 위치
        Vector3 position = cubeSpawner.LastCube.position;
        position.y = cubeSpawner.CurrentCube.transform.position.y - cubeSpawner.CurrentCube.transform.localScale.y * 0.5f;

        // 이펙트 크기
        Vector3 scale = cubeSpawner.CurrentCube.transform.localScale;
        scale = new Vector3(scale.x + addedSize, perfectEffect.localScale.y, scale.z + addedSize);

        // 이펙트 생성
        Transform effect = Instantiate(perfectEffect);
        effect.position = position;
        effect.localScale = scale;
    }

    private void SFXProcess()
    {
        int maxCombo = 5;
        float volumeMin = 0.3f;
        float volumeAdditive = 0.15f;
        float pitchMin = 0.7f;
        float pitchAdditive = 0.15f;

        // maxCombo인 5가 될 때까지 volume과 pitch를 서서히 증가
        if(perfectCombo < maxCombo)
        {
            audioSource.volume = volumeMin + perfectCombo * volumeAdditive;
            audioSource.pitch = pitchMin + perfectCombo * pitchAdditive;
        }

        audioSource.Play();
    }
}
