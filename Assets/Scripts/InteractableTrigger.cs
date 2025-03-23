using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private TaskManager taskManager;
    private bool taskCompleted = false;

    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!taskCompleted &&
            (other.CompareTag("Left Hand") || other.CompareTag("Right Hand")) &&
            CompareTag("Interactable"))
        {
            taskManager.CompleteTaskStep(gameObject);
            taskCompleted = true;
        }
    }
}
