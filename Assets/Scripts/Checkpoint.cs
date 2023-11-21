using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Checkpoint nextCP;
    [SerializeField] private Checkpoint prevCP;
    [SerializeField] private GameObject vfxIdle;
    [SerializeField] private GameObject vfxSpawn;
    private GameObject particlesIdle;
    private Vector3 position;
    private bool isActive;

    private void Start()
    {
        position = gameObject.transform.position;
        if (prevCP == this)
        {
            GameManager.Instance.SetActiveCP(this);
            particlesIdle= Instantiate(vfxIdle,position,Quaternion.identity);
            isActive = true;
        }
    }

    public Vector3 GetPosition()
    {
        return position;
    } 
    public bool GetIsActive()
    {
        return isActive;
    }

    public void Spawn()
    {
        Instantiate(vfxSpawn,position,Quaternion.identity);
    }
    public void Activate()
    {
        if (!nextCP.GetIsActive()||nextCP == this)
        {
            Debug.Log("Activating " + gameObject.name);
            GameManager.Instance.SetActiveCP(this);
            isActive = true;
            prevCP.Deactivate();
            particlesIdle= Instantiate(vfxIdle,position,Quaternion.identity);
        }
    }

    public void Deactivate()
    {
        isActive = false;
        Debug.Log("Deactivating " + gameObject.name);
        Destroy(particlesIdle);
    }
    

}
