using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour
{
    [SerializeField] private DialogueContainer dialogue;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button choicePrefab;
    [SerializeField] private Transform buttonContainer;

    private void Start()
    {
        var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
        ProceedToNarrative(narrativeData.TargetNodeGuid);
    }

    private void ProceedToNarrative(string narrativeDataGUID)
    {
        var text = dialogue.DialogueNodeData.Find(x => x.Guid == narrativeDataGUID).DialogueText;
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == narrativeDataGUID);
        dialogueText.text = ProcessProperties(text);
        var buttons = buttonContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        foreach (var choice in choices)
        {
            var button = Instantiate(choicePrefab, buttonContainer);
            button.GetComponentInChildren<TMP_Text>().text = ProcessProperties(choice.PortName);
            button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGuid));
        }
    }

    // TODO: END NODE YAP
    // UI TOOLKIT İLE BAĞLA
    private string ProcessProperties(string text)
    {
        foreach (var exposedProperty in dialogue.ExposedProperties)
        {
            text = text.Replace($"[{exposedProperty.Name}]", exposedProperty.Value);
        }

        return text;
    }
}