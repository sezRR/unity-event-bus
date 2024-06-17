using System;
using UnityEngine;

public interface IUpgradeable
{
    void ApplyUpgrade(GameObject target);
    void RaiseUpgradeEvent();
}
