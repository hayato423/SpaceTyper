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
        targetListIndex = 0;
        targetEnemyId = 1;
        attackPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if(isInputValid && e.type == EventType.KeyDown && e.type != EventType.KeyUp && e.keyCode != KeyCode.None
           && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)  && !Input.GetMouseButton(2))
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            if (e.keyCode == KeyCode.Tab)
            {                
                List<uint> enemyIdList = GameObject.Find("EnemyManager").GetComponent<EnemyGenerator>().enemyIds;                
                if (enemyIdList.Count > 0)
                {
                    if (targetListIndex == enemyIdList.Count - 1)
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
                            targetObj.transform.position = enemy.transform.position;
                        }
                    }
                }                
            }
            else
            {
                foreach (GameObject enemy in enemys)
                {
                    if (targetEnemyId == enemy.GetComponent<Enemy>().Id)
                    {
                        bool IsAttackValid = enemy.GetComponent<Enemy>().IsInputedLetter(keycodeToChar[e.keyCode]);
                        if(IsAttackValid == true)
                        {
                            Attack(enemy);
                        }
                    }
                }
            }
        }
    }

    void Attack(GameObject e)
    {        
        e.GetComponent<Enemy>().ReceiveDamage(attackPoint);
    }
}
