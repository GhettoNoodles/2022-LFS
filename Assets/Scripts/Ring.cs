using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    private Vector3 _position;

    public Vector3 GetPos()
    {
        return _position;
    }

    public void SetPos(Vector3 pos)
    {
        _position = pos;
    }
    public void Use()
    {
        //VFX, AFX 
        AudioManager.Instance.Ring();
        Instantiate(vfx,gameObject.transform.position,Quaternion.identity);
        gameObject.SetActive(false);
    }
}
