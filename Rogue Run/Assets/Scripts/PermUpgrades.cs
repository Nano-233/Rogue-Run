using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PermUpgrades : MonoBehaviour
{
    //player
    private GameObject _player;
    private PlayerController _playerController;

    //levels of upgrades
    private int[] _stats = new int[6];
    private int[] _levels = new int[6];
    
    //list of upgrades
    private Upgrade[] _upgrades = new Upgrade[]
    {
        new Upgrade { Name = "Dasher", Description = "Decrease the delay between dashes by X%. \n Currently V%"},
        new Upgrade { Name = "Assassin", Description = "Increases damage to enemies from behind by X%. \n Currently V%"},
        new Upgrade { Name = "Vanguard", Description = "Decreases the first instance of damage taken per room by X%. \n Currently V%"},
        new Upgrade { Name = "Undead", Description = "Heal for XHP after each room.  \n Currently VHP"},
        new Upgrade { Name = "Gambler", Description = "You earn X% more darkness, but take twice the damage.  \n Currently V%"},
        new Upgrade { Name = "Slayer", Description = "You have a X% chance to gain 5HP after each enemy killed.  \n Currently V%"},
    };

    private void Awake()
    {
        //gets the player component
        _player = GameObject.FindGameObjectWithTag("Player");
        //controller
        _playerController = _player.GetComponent<PlayerController>();
        ButtonsSet();
    }


    [SerializeField] private Button Upgrade_button1;
    [SerializeField] private Button Upgrade_button2;
    [SerializeField] private Button Upgrade_button3;
    [SerializeField] private Button Upgrade_button4;
    [SerializeField] private Button Upgrade_button5;
    [SerializeField] private Button Upgrade_button6;

    [SerializeField] private TMP_Text Upgrade_DescriptionText1;
    [SerializeField] private TMP_Text Upgrade_DescriptionText2;
    [SerializeField] private TMP_Text Upgrade_DescriptionText3;
    [SerializeField] private TMP_Text Upgrade_DescriptionText4;
    [SerializeField] private TMP_Text Upgrade_DescriptionText5;
    [SerializeField] private TMP_Text Upgrade_DescriptionText6;



    public void ButtonsSet()
    {
        
        //gets the current levels
        for (int i = 0; i < 6; i++)
        {
            _stats[i] = _playerController.GetPermUpgrade(i);
            
            //gets the flat levels
            switch (i)
            {
                case 0: case 4:
                    _levels[i] = _stats[i] / 10;
                    break;
                case 5:
                    _levels[i] = _stats[i] / 2;
                    break;
                default:
                    _levels[i] = _stats[i] / 5;
                    break;
            }
        }

        // Setting text
        Upgrade_button1.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[0].Name;
        Upgrade_button2.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[1].Name;
        Upgrade_button3.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[2].Name;
        Upgrade_button4.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[3].Name;
        Upgrade_button5.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[4].Name;
        Upgrade_button6.transform.GetChild(0).GetComponent<TMP_Text>().text = _upgrades[5].Name;

        // Replacing the X with increase value, and V with original value
        Upgrade_DescriptionText1.text = _upgrades[0].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText1.text = Upgrade_DescriptionText1.text.Replace("V", _stats[0].ToString());
        Upgrade_DescriptionText2.text = _upgrades[1].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText2.text = Upgrade_DescriptionText2.text.Replace("V", _stats[0].ToString());
        Upgrade_DescriptionText3.text = _upgrades[2].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText3.text = Upgrade_DescriptionText3.text.Replace("V", _stats[0].ToString());
        Upgrade_DescriptionText4.text = _upgrades[3].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText4.text = Upgrade_DescriptionText4.text.Replace("V", _stats[0].ToString());
        Upgrade_DescriptionText5.text = _upgrades[4].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText5.text = Upgrade_DescriptionText5.text.Replace("V", _stats[0].ToString());
        Upgrade_DescriptionText6.text = _upgrades[5].Description.Replace("X", (_stats[0] + 10).ToString());
        Upgrade_DescriptionText6.text = Upgrade_DescriptionText6.text.Replace("V", _stats[0].ToString());
        
        
        // Setting color of the buttons
        Dictionary<int, Color> rarityColors = new Dictionary<int, Color>();
        rarityColors.Add(0, new Color(1, 1, 1, 1));
        rarityColors.Add(1, new Color(0.5f, 1f, 0.5f, 1));
        rarityColors.Add(2, new Color(0f, 0f, 1f, 1));
        rarityColors.Add(3, new Color(1f, 1f, 0f, 1));
        rarityColors.Add(4, new Color(0.5f, 0f, 0.5f, 1));
        rarityColors.Add(5, new Color(1f, 0.5f, 0f, 1));


        Upgrade_button1.GetComponent<Image>().color = rarityColors[_levels[0]];
        Upgrade_button2.GetComponent<Image>().color = rarityColors[_levels[1]];
        Upgrade_button3.GetComponent<Image>().color = rarityColors[_levels[2]];
        Upgrade_button4.GetComponent<Image>().color = rarityColors[_levels[3]];
        Upgrade_button4.GetComponent<Image>().color = rarityColors[_levels[4]];
        Upgrade_button4.GetComponent<Image>().color = rarityColors[_levels[5]];
    }

    // UPGRADES
    public void UpgradeChosen(int upgrade)
    {
        _playerController.PermUpgrade(upgrade);
    }
    

    public class Upgrade
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] Level { get; set; }
    }
}


