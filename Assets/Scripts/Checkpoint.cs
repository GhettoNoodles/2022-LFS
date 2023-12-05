using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Checkpoint nextCP;
    [SerializeField] private Checkpoint prevCP;
    [SerializeField] private GameObject vfxIdle;
    [SerializeField] private GameObject vfxSpawn;
    [SerializeField] private AudioSource cpAudioSrc;
    public int cpNum;
    private GameObject particlesIdle;
    private Vector3 position;
    private bool isActive;

    private void Start()
    {
        position = gameObject.transform.position;
        
        //Setup Starting point as Checkpoint
        if (prevCP == this)
        {
            AudioManager.Instance.CpActivate(cpAudioSrc);
            GameManager.Instance.SetActiveCP(this);
            particlesIdle= Instantiate(vfxIdle,position,Quaternion.identity);
            particlesIdle= Instantiate(vfxSpawn,position,Quaternion.identity);
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
        // Particle VFX for player respawn
        Instantiate(vfxSpawn,position,Quaternion.identity);
    }
    public void Activate()
    {
        //Activate this checkpoint and Deactivate previous Checkpoint
        if (!nextCP.GetIsActive()||nextCP == this)
        {
            AudioManager.Instance.CpActivate(cpAudioSrc);
            GameManager.Instance.SetActiveCP(this);
            isActive = true;
            prevCP.Deactivate();
            particlesIdle= Instantiate(vfxIdle,position,Quaternion.identity);
            particlesIdle= Instantiate(vfxSpawn,position,Quaternion.identity);
        }
    }

    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            Destroy(particlesIdle); //cleans up particles
        }
        
    }
    

}
