using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int attackPoint;
    private bool isInputValid;
    private int targetListIndex;
    private uint targetEnemyId;
    [SerializeField] GameObject targetObj;
    private GameObject targetedEnemy;
    private GameObject[] enemys;
    List<uint> enemyIdList;    
    Dictionary<KeyCode, char> keycodeToChar = new Dictionary<KeyCode, char>()
    {
        {KeyCode.A, 'a' },
        {KeyCode.B, 'b' },
        {KeyCode.C, 'c' },
        {KeyCode.D, 'd' },
        {KeyCode.E, 'e' },
        {KeyCode.F, 'f' },
        {KeyCode.G, 'g' },
        {KeyCode.H, 'h' },
        {KeyCode.I, 'i' },
        {KeyCode.J, 'j' },
        {KeyCode.K, 'k' },
        {KeyCode.L, 'l' },
        {KeyCode.M, 'm' },
        {KeyCode.N, 'n' },
        {KeyCode.O, 'o' },
        {KeyCode.P, 'p' },
        {KeyCode.Q, 'q' },
        {KeyCode.R, 'r' },
        {KeyCode.S, 's' },
        {KeyCode.T, 't' },
        {KeyCode.U, 'u' },
        {KeyCode.V, 'v' },
        {KeyCode.W, 'w' },
        {KeyCode.X, 'x' },
        {KeyCode.Y, 'y' },
        {KeyCode.Z, 'z' }
    };
    // Start is called before the first frame update
    void Start()
    {
        isInputValid = true;
        targetListIndex = -1;
        targetEnemyId = 0;
        attackPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyIdList = GameObject.Find("EnemyManager").GetComponent<EnemyGenerator>().enemyIds;
        if (enemyIdList.Contains(targetEnemyId) == false)
        {
            targetedEnemy = ChangeRockonTarget();
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;        
        if(isInputValid && e.type == EventType.KeyDown && e.type != EventType.KeyUp && e.keyCode != KeyCode.None
           && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)  && !Input.GetMouseButton(2))
        {            
            if (e.keyCode == KeyCode.Tab)
            {
                targetedEnemy = ChangeRockonTarget();            
            }
            else
            {                
                if(targetedEnemy != null)
                {
                    bool IsAttackValid = targetedEnemy.GetComponent<Enemy>().IsInputedLetter(keycodeToChar[e.keyCode]);
                    if (IsAttackValid == true)
                    {
                        Attack(targetedEnemy);
                    }
                }
            }
        }
    }

    void Attack(GameObject e)
    {        
        e.GetComponent<Enemy>().ReceiveDamage(attackPoint);
    }


    GameObject ChangeRockonTarget()
    {        
        if (enemyIdList.Count > 0)
        {
            if (targetListIndex+1 > enemyIdList.Count - 1)
            {
                targetListIndex = 0;
            }
            else
            {
                targetListIndex++;
            }
            targetEnemyId = enemyIdList[targetListIndex];
            foreach (GameObject enemy in enemys)
            {
                if (targetEnemyId == enemy.GetComponent<Enemy>().Id)
                {
                    targetObj.transform.position = enemy.transform.position + new Vector3(0f,0f,-0.78f);
                    return enemy;
                }
            }
        }
        return null;
    }
}
