using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI ringsText;
   // [SerializeField] private GameObject infoPanel;
    private int _rings;
    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseRings()
    {
        _rings++;
        ringsText.text = "Rings: " + _rings;
    }

    public void damagePlayer()
    {
        Debug.Log("Dead");
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
