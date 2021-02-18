using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private bool isPlayer;
    private bool isFinishedInit;
    [SerializeField] float speed;
    private GameObject targetObj;
    private Material mat;
    private Color enemyColor;
    private Color playerColor;    
    private string targetTagName;
    private int attackPointOfPlayer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        isFinishedInit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishedInit == true)
        {            
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, step);
        }        
    }

    

    public void Initialize(bool isP, string tag, in GameObject target, int attackPoint = 0)
    {
        isPlayer            = isP;
        targetObj           = target;
        targetTagName       = tag;
        attackPointOfPlayer = attackPoint;
        Rotate();                
        isFinishedInit = true;
    }

    private void Rotate()
    {
        //ただLookAtするだけだと，ビームがターゲットに対して縦方向になってしまうため
        //無理やりx軸90°回転させる．
        transform.LookAt(targetObj.transform, new Vector3(0, 1, 0));
        transform.Rotate(new Vector3(90f, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTagName)
        {            
            Destroy(this.gameObject);
            if(targetTagName == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().ReceiveDamage(attackPointOfPlayer);
            }
            return;
        }        
    }
}
