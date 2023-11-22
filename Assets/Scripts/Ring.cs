using System.Collections;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] GameObject vfx;
    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void Use()
    {
        //VFX, AFX 
        AudioManager.Instance.Ring();
        Instantiate(vfx,gameObject.transform.position,Quaternion.identity);
        //StartCoroutine(DestroyDelay());
        Destroy(gameObject);
    }
}
