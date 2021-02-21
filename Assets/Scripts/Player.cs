using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int attackPoint;
    private bool isInputValid;    
    [SerializeField] GameObject sightObj;
    private GameObject targetedEnemy;
    private GameObject[] enemys;
    private AudioSource beamSound;
    private AudioSource damagedSound;
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
    [SerializeField] GameObject beam;
    [SerializeField] int life;
    [SerializeField] Canvas canvas;
    [SerializeField] Image lifeImg;    
    // Start is called before the first frame update
    void Start()
    {
        isInputValid = true;               
        attackPoint = 1;
        LineUpLifeImg();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        beamSound = audioSources[0];
        damagedSound = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyIdList = GameObject.Find("EnemyManager").GetComponent<EnemyGenerator>().enemyIds;
        if (enemyIdList.Contains(sightObj.GetComponent<Target>().targetedEnemyId) == false)
        {
            targetedEnemy = sightObj.GetComponent<Target>().ChangeRockOnEnemy(enemyIdList, enemys);
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
                targetedEnemy = sightObj.GetComponent<Target>().ChangeRockOnEnemy(enemyIdList, enemys);
            }
            else
            {                
                if(targetedEnemy != null && keycodeToChar.ContainsKey(e.keyCode))
                {                    
                    bool IsAttackValid = targetedEnemy.GetComponent<Enemy>().IsInputedLetter(keycodeToChar[e.keyCode]);
                    if (IsAttackValid == true)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    void Attack()
    {
        beamSound.PlayOneShot(beamSound.clip);
        GameObject beamInstance = Instantiate(beam, transform.position, Quaternion.identity);
        beamInstance.GetComponent<Beam>().Initialize(true, "Enemy", targetedEnemy, attackPoint);
        //enemy.GetComponent<Enemy>().ReceiveDamage(attackPoint);
    }    


    private void LineUpLifeImg()
    {
        for(int i = 1; i <= life; ++i)
        {
            Image instance =  Instantiate(original: lifeImg, parent: canvas.transform);
            Vector2 lifeImgPos = lifeImg.rectTransform.anchoredPosition;
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector3(lifeImgPos.x + (40 * (i-1)), lifeImgPos.y,0);                        
            instance.name = "Life" + i;
        }
    }

    public void ReceiveDamage()
    {
        damagedSound.PlayOneShot(damagedSound.clip);
        Destroy(GameObject.Find("Life" + life));
        life--;
        if( life < 0)
        {
            Debug.Log("gameover");
        }
    }
}
