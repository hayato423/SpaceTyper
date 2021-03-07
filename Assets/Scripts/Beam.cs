using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private bool isPlayers;
    private bool isFinishedInit;
    [SerializeField] float speed;
    private GameObject targetObj;
    private Vector3 destination;    
    private MeshRenderer meshRend;
    [ColorUsage(false,true)]  public Color enemyColor;
    [ColorUsage(false, true)] public Color playerColor;    
    private string targetTagName;
    private float attackPointOfPlayer = 0;    
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
        isPlayers            = isP;
        targetObj           = target;
        destination         = targetObj.transform.position;        
        targetTagName       = tag;
        attackPointOfPlayer = attackPoint;
        Rotate();                        
        if(isPlayers == true)
        {
            meshRend.material.SetColor("_EmissionColor", playerColor);
        }
        else
        {
            meshRend.material.SetColor("_EmissionColor", enemyColor);
        }
        isFinishedInit = true;
    }

    private void Rotate()
    {
        //ただLookAtするだけだと，ビームがターゲットに対して縦方向になってしまうため
        //無理やりx軸90°回転させる．
        transform.LookAt(targetObj.transform, new Vector3(0, 1, 0));
        //transform.Rotate(new Vector3(90f, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTagName)
        {            
            Destroy(this.gameObject);
            if(isPlayers == true)
            {
                other.gameObject.GetComponent<Enemy>().ReceiveDamage(attackPointOfPlayer);
            }else
            {
                other.gameObject.GetComponent<Player>().ReceiveDamage();
            }
            return;
        }        
    }
}
