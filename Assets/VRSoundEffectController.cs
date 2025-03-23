using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedContinuousMoveProvider))]
public class VRSoundEffectController : MonoBehaviour
{
    public AudioSource audioSource;  // 提前放好，避免动态生成
    public AudioClip[] footstepClips;  // 不同材质音效
    public float stepInterval = 0.5f;

    private ActionBasedContinuousMoveProvider moveProvider;
    private float stepTimer;
    private Vector2 lastMoveInput;

    void Start()
    {
        moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 预加载
        if (footstepClips.Length > 0)
        {
            audioSource.clip = footstepClips[0];  // 默认选一个，后续按材质切换
        }
    }

    void Update()
    {
        Vector2 moveInput = moveProvider.leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        moveInput += moveProvider.rightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

        bool isMoving = moveInput.magnitude > 0.1f;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        lastMoveInput = moveInput;
    }

    void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);  // 快速播放
        }
    }
}
