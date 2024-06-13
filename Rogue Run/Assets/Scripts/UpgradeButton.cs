using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_button : MonoBehaviour
{
    [SerializeField] private PermUpgrades permUpgradesscript;

    //upgrade
    public void Upgrade()
    {
        int upgradeChosen = Int32.Parse(gameObject.transform.GetChild(0).name);
        permUpgradesscript.UpgradeChosen(upgradeChosen);
        permUpgradesscript.ButtonsSet();
    }
}