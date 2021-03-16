using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{    
    private GameObject    targetedEnemy;
    private RectTransform myRectTransform;
    private Camera        cam;
    public uint targetedEnemyId { get; private set; }
    private int enemyIdsIndex;
    // Start is called before the first frame update
    void Start()
    {
        enemyIdsIndex = 0;
        targetedEnemyId = 0;
        myRectTransform = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // ターゲットしている敵に追従
        if (targetedEnemy != null && targetedEnemy.GetComponent<Renderer>().isVisible)
        {
            GetComponent<Image>().enabled = true;                     
            Vector2 screenPos = cam.WorldToScreenPoint(targetedEnemy.transform.position);
            myRectTransform.position = screenPos;
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
    }

    public GameObject ChangeRockOnEnemy(in  List<uint> enemyIds, in GameObject[] enemys)
    {        
        if(enemyIds.Count > 0)
        {
            if(enemyIdsIndex+1 > enemyIds.Count - 1) enemyIdsIndex = 0;            
            else                 enemyIdsIndex++;            
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
