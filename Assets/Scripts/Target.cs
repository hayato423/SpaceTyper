using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameObject targetedEnemy;
    public uint targetedEnemyId { get; private set; }
    private int enemyIdsIndex;
    // Start is called before the first frame update
    void Start()
    {
        enemyIdsIndex = 0;
        targetedEnemyId = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy != null && targetedEnemy.GetComponent<Renderer>().isVisible)
        {
            this.transform.position = targetedEnemy.transform.position + new Vector3(0f, 0f, -0.78f);
        }
        else
        {
            this.transform.position = new Vector3(0f, 0f, 6.31f);
        }        
    }

    public GameObject ChangeRockOnEnemy(in  List<uint> enemyIds, in GameObject[] enemys)
    {        
        if(enemyIds.Count > 0)
        {
            if(enemyIdsIndex+1 > enemyIds.Count - 1)
            {
                enemyIdsIndex = 0;
            }
            else
            {
                enemyIdsIndex++;
            }
            targetedEnemyId = enemyIds[enemyIdsIndex];
        }                
        foreach(GameObject enemy in enemys)
        {
            if(targetedEnemyId == enemy.GetComponent<Enemy>().Id)
            {
                targetedEnemy = enemy;
                return enemy;
            }
        }
        return null;
    }
}
