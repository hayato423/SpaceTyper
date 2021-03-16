using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private bool           isPlayers;
    private bool           isFinishedInit;    
    private GameObject     targetObj;    
    private MeshRenderer   meshRend;
    private string         targetTagName;    
    private float          attackPointOfPlayer = 0;
    [SerializeField] float speed;
    [ColorUsage(false,true)]  public Color enemyColor;
    [ColorUsage(false, true)] public Color playerColor;    
    
    // Start is called before the first frame update
    void Awake()
    {
        isFinishedInit = false;
        meshRend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishedInit == true)
        {            
            try
            {
                float step = speed * Time.deltaTime;
                transform.position =  Vector3.MoveTowards(transform.position, targetObj.transform.position, step);
            }
            catch (MissingReferenceException exception)
            {
                Debug.LogWarning(exception);
                Destroy(this.gameObject);
            }
        }        
    }

    

    public void Initialize(bool isP, string tag, in GameObject target, float attackPoint = 0)
    {
        isPlayers           = isP;
        targetObj           = target;               
        targetTagName       = tag;
        attackPointOfPlayer = attackPoint;
        transform.LookAt(targetObj.transform, new Vector3(0, 1, 0));
        if (isPlayers == true) meshRend.material.SetColor("_EmissionColor", playerColor);        
        else meshRend.material.SetColor("_EmissionColor", enemyColor);        
        isFinishedInit = true;
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTagName)
        {            
            Destroy(this.gameObject);
            if(isPlayers == true) other.gameObject.GetComponent<Enemy>().ReceiveDamage(attackPointOfPlayer);
            else                  other.gameObject.GetComponent<Player>().ReceiveDamage();            
            return;
        }        
    }
}
