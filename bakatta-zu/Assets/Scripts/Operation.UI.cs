using UnityEngine;

public class OperationUI : MonoBehaviour
{
    public GameObject operationPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            operationPanel.SetActive(!operationPanel.activeSelf);
        }
    }
}
