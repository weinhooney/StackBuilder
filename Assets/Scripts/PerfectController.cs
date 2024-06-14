using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;
    [SerializeField] private Transform perfectEffect;
    [SerializeField] private Transform perfectComboEffect;
    [SerializeField] private Transform perfectRecoveryEffect;

    private AudioSource audioSource;

    [SerializeField] private int recoveryCombo = 5; // ť���� ũ�Ⱑ �����ϴ� �ּ� �޺�

    private float perfectCorrection = 0.01f; // Perfect�� �����ϴ� ������
    private float addedSize = 0.1f; // ���� ť�� ũ�⿡ �������� ��
    private int perfectCombo = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsPerfect(float hangOver)
    {
        // Perfect�� �޺��� ��ø�� ��
        if(Mathf.Abs(hangOver) <= perfectCorrection)
        {
            EffectProcess(); // �޺��� ���� ����Ʈ ���
            SFXProcess(); // �޺��� ���� ���� ���

            perfectCombo++;

            return true;
        }
        else // �޺� �ʱ�ȭ
        {
            perfectCombo = 0;

            return false;
        }
    }

    private void EffectProcess()
    {
        // ����Ʈ ���� ��ġ
        Vector3 position = cubeSpawner.LastCube.position;
        position.y = cubeSpawner.CurrentCube.transform.position.y - cubeSpawner.CurrentCube.transform.localScale.y * 0.5f;

        // ����Ʈ ũ��
        Vector3 scale = cubeSpawner.CurrentCube.transform.localScale;
        scale = new Vector3(scale.x + addedSize, perfectEffect.localScale.y, scale.z + addedSize);

        // �⺻ ����Ʈ ����Ʈ ����
        OnPerfectEffect(position, scale);

        if(0 < perfectCombo && perfectCombo < recoveryCombo)
        {
            // �޺� ����Ʈ ����Ʈ ����
            StartCoroutine(OnPerfectComboEffect(position, scale));
        }
        else if(recoveryCombo <= perfectCombo)
        {
            OnPerfectRecoveryEffect();
        }
    }

    private void OnPerfectEffect(Vector3 position, Vector3 scale)
    {
        // ����Ʈ ����
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

        // maxCombo�� 5�� �� ������ volume�� pitch�� ������ ����
        if(perfectCombo < maxCombo)
        {
            audioSource.volume = volumeMin + perfectCombo * volumeAdditive;
            audioSource.pitch = pitchMin + perfectCombo * pitchAdditive;
        }

        audioSource.Play();
    }

    private IEnumerator OnPerfectComboEffect(Vector3 position, Vector3 scale)
    {
        // ����Ʈ ���
        // �޺��� ��ø�� ������ ���� �߰�
        int currentCombo = 0;
        float beginTime = Time.time;
        float duration = 0.15f;

        while(currentCombo < perfectCombo)
        {
            float t = (Time.time - beginTime) / duration;
            if(1 <= t)
            {
                // ����Ʈ ����
                Transform effect = Instantiate(perfectComboEffect);
                effect.position = position;
                effect.localScale = scale;

                beginTime = Time.time;

                currentCombo++;
            }

            yield return null;
        }
    }

    public void OnPerfectRecoveryEffect()
    {
        // ����Ʈ ����
        Transform effect = Instantiate(perfectRecoveryEffect);

        // ����Ʈ ���� ��ġ
        effect.position = cubeSpawner.CurrentCube.transform.position;

        // ����Ʈ�� ���� �ݰ� ����(�������� �β�)
        var shape = effect.GetComponent<ParticleSystem>().shape;

        Transform currentCube = cubeSpawner.CurrentCube.transform;
        float radius = currentCube.transform.localScale.z < currentCube.localScale.x ? currentCube.localScale.x : currentCube.localScale.z;
        shape.radius = radius;
        shape.radiusThickness = radius * 0.5f;

        // �̵� ť���� �Ϻκ��� ȸ����Ŵ
        cubeSpawner.CurrentCube.RecoveryCube();
    }
}
