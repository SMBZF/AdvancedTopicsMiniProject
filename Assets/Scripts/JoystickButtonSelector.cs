using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class JoystickButtonSelector : MonoBehaviour
{
    public GameObject uiPanel;
    public Button[] buttons;
    public Text[] timerTexts; // ��Ӧÿ����ť��� Text
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;
    public InputActionReference joystickInput;
    public InputActionReference triggerInput;
    public TaskManager taskManager;

    public InputActionReference leftStickAction;  // ����ƶ�����
    public InputActionReference rightStickAction; // ����ӽǿ���
    public Transform player; // ��� transform ���ڲ��

    private int currentIndex = 0;
    private bool canMove = true;
    private float[] taskStartTimes; // ��¼ÿ������Ŀ�ʼʱ��
    private bool[] taskCompleted;   // ����Ƿ����
    private float timerStart;       // ��ǰ��ʱ���

    private float currentTaskDistance = 0f;
    private Vector3 lastPlayerPosition;
    private bool isTrackingDistance = false;

    private void Start()
    {
        UpdateButtonSelection();
        taskStartTimes = new float[buttons.Length];
        taskCompleted = new bool[buttons.Length];

        ShowUI();
    }

    private void Update()
    {
        Vector2 joystickValue = joystickInput.action.ReadValue<Vector2>();

        if (canMove)
        {
            if (joystickValue.y > 0.5f) MoveSelection(-1);
            else if (joystickValue.y < -0.5f) MoveSelection(1);
        }

        if (triggerInput.action.WasPressedThisFrame())
        {
            StartSelectedTask();
        }

        if (isTrackingDistance)
        {
            float delta = Vector3.Distance(player.position, lastPlayerPosition);
            currentTaskDistance += delta;
            lastPlayerPosition = player.position;
        }
    }

    private void MoveSelection(int direction)
    {
        canMove = false;
        int nextIndex = currentIndex;

        do
        {
            nextIndex += direction;
            if (nextIndex < 0) nextIndex = buttons.Length - 1;
            if (nextIndex >= buttons.Length) nextIndex = 0;
        } while (!buttons[nextIndex].gameObject.activeSelf);

        currentIndex = nextIndex;
        UpdateButtonSelection();
        Invoke(nameof(ResetMoveCooldown), 0.2f);
    }

    private void ResetMoveCooldown() => canMove = true;

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colorBlock = buttons[i].colors;
            if (i == currentIndex)
            {
                colorBlock.normalColor = selectedColor;
                colorBlock.highlightedColor = selectedColor;
                colorBlock.pressedColor = selectedColor;
            }
            else
            {
                colorBlock.normalColor = normalColor;
                colorBlock.highlightedColor = normalColor;
                colorBlock.pressedColor = normalColor;
            }
            buttons[i].colors = colorBlock;
        }

        buttons[currentIndex].Select();
    }

    private void StartSelectedTask()
    {
        if (taskCompleted[currentIndex]) return;

        timerStart = Time.time;
        taskStartTimes[currentIndex] = timerStart;

        currentTaskDistance = 0f;
        lastPlayerPosition = player.position;
        isTrackingDistance = true;

        taskManager.StartTask(currentIndex);
        HideUI();
    }

    public void StopTaskTimer(int index)
    {
        if (taskCompleted[index]) return;

        isTrackingDistance = false;

        float elapsedTime = Time.time - taskStartTimes[index];
        float distance = currentTaskDistance;

        timerTexts[index].text = $"{ elapsedTime:F2} s | {distance:F2} m";
        taskCompleted[index] = true;
    }

    private void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            //Debug.Log("UI ������");

            if (leftStickAction != null) leftStickAction.action.Enable();
            if (rightStickAction != null) rightStickAction.action.Enable();
        }
    }

    public void ShowUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            //Debug.Log("UI ��ʾ");

            if (leftStickAction != null)
            {
                leftStickAction.action.Disable();
                //Debug.Log("��ҡ���ѽ���");
            }
            if (rightStickAction != null)
            {
                rightStickAction.action.Disable();
                //Debug.Log("��ҡ���ѽ���");
            }
        }
    }
}
