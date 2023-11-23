using System.Collections;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] GameObject vfx;
    

    public void Use()
    {
        //VFX, AFX 
        AudioManager.Instance.Ring();
        Instantiate(vfx,gameObject.transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
