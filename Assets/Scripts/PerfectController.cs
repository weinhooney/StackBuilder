using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;
    [SerializeField] private Transform perfectEffect;

    private AudioSource audioSource;

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
        // �⺻ ����Ʈ ����Ʈ ����
        OnPerfectEffect();
    }

    private void OnPerfectEffect()
    {
        // ����Ʈ ���� ��ġ
        Vector3 position = cubeSpawner.LastCube.position;
        position.y = cubeSpawner.CurrentCube.transform.position.y - cubeSpawner.CurrentCube.transform.localScale.y * 0.5f;

        // ����Ʈ ũ��
        Vector3 scale = cubeSpawner.CurrentCube.transform.localScale;
        scale = new Vector3(scale.x + addedSize, perfectEffect.localScale.y, scale.z + addedSize);

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
}
