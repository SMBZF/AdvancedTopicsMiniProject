using UnityEngine;

public class PlayerPositionChecker : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation; // 记录初始朝向

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // 任务完成后，直接传送玩家回初始位置
    public void ResetPlayerPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        Debug.Log("玩家已被传送回初始位置");
    }
}
