using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI ringsText;
    [SerializeField] private TextMeshProUGUI healthText;
    

    public void SetRing(int rings)
    {
        ringsText.text = "Rings :" + rings;
    }public void SetHealth(int hp)
    {
        healthText.text = "HP :" + hp;
    }
    
}
