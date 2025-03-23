using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TaskStep
{
    public GameObject displayObject;
    public GameObject triggerObject;
}

[System.Serializable]
public class TaskSequence
{
    public List<TaskStep> steps = new List<TaskStep>();
}

public class TaskManager : MonoBehaviour
{
    public List<TaskSequence> tasks = new List<TaskSequence>();
    public GameObject uiPanel;
    public Transform player;
    public JoystickButtonSelector uiSelector;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private int currentTaskIndex = -1;
    private int currentStepIndex = 0;

    private void Start()
    {
        initialPosition = player.position;
        initialRotation = player.rotation;

        HideAllSteps();
        if (uiPanel != null) uiPanel.SetActive(true);
        if (uiSelector != null) uiSelector.ShowUI();
    }

    public void StartTask(int taskIndex)
    {
        currentTaskIndex = taskIndex;
        currentStepIndex = 0;

        HideAllSteps();
        SetStepActive(taskIndex, 0, true);

        if (uiPanel != null) uiPanel.SetActive(false);
    }

    public void CompleteTaskStep(GameObject triggeredObject)
    {
        if (currentTaskIndex < 0 || currentTaskIndex >= tasks.Count) return;

        var sequence = tasks[currentTaskIndex];
        if (currentStepIndex >= sequence.steps.Count) return;

        var step = sequence.steps[currentStepIndex];
        if (step.triggerObject != triggeredObject) return;

        // 结束当前 step
        if (step.displayObject != null) step.displayObject.SetActive(false);
        if (step.triggerObject != null) step.triggerObject.tag = "Untagged";

        currentStepIndex++;

        // 是否完成任务
        if (currentStepIndex >= sequence.steps.Count)
        {
            player.position = initialPosition;
            player.rotation = initialRotation;

            if (uiPanel != null) uiPanel.SetActive(true);
            if (uiSelector != null)
            {
                uiSelector.StopTaskTimer(currentTaskIndex);
                uiSelector.ShowUI();
            }

            Debug.Log("任务完成");
        }
        else
        {
            // 激活下一个 step
            SetStepActive(currentTaskIndex, currentStepIndex, true);
        }
    }

    private void HideAllSteps()
    {
        foreach (var task in tasks)
        {
            foreach (var step in task.steps)
            {
                if (step.displayObject != null) step.displayObject.SetActive(false);
                if (step.triggerObject != null) step.triggerObject.tag = "Untagged";
            }
        }
    }

    private void SetStepActive(int taskIndex, int stepIndex, bool active)
    {
        var step = tasks[taskIndex].steps[stepIndex];

        if (step.displayObject != null) step.displayObject.SetActive(active);
        if (step.triggerObject != null)
        {
            step.triggerObject.tag = active ? "Interactable" : "Untagged";
        }

        Debug.Log($"激活任务 {taskIndex} 的阶段 {stepIndex}");
    }
}
