using UnityEngine;

public class PlayerPositionChecker : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation; // ��¼��ʼ����

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // ������ɺ�ֱ�Ӵ�����һس�ʼλ��
    public void ResetPlayerPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        Debug.Log("����ѱ����ͻس�ʼλ��");
    }
}
