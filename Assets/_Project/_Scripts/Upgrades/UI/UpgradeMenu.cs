using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeMenu : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public VisualTreeAsset buttonTemplate;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var upgradeList = root.Q<ScrollView>("UpgradesContainer");

        foreach (var upgrade in upgradeManager.availableUpgrades)
        {
            var button = buttonTemplate.CloneTree();
            button.Q<Button>().text = upgrade.upgradeName;
            button.Q<Button>().clicked += () => upgradeManager.ApplyUpgrade(upgrade);
            upgradeList.Add(button);
        }
    }
}
