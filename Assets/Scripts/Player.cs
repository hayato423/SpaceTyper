using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ILR;

public class Player : MonoBehaviour
{
    private float attackPoint;
    private bool isInputValid;    
    [SerializeField] GameObject sightObj;
    private GameObject targetedEnemy;
    private GameObject[] enemys;
    private AudioSource beamSound;
    private AudioSource damagedSound;
    private AudioSource powerUpSound;
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
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanael;
    [SerializeField] Slider ComboBar;
    [SerializeField] int  maxValueOfComboBar = 10;
    private int ComboValue;
    // Start is called before the first frame update
    void Start()
    {
        isInputValid = true;               
        attackPoint = 1;
        ComboValue = 0;
        LineUpLifeImg();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        beamSound = audioSources[0];
        damagedSound = audioSources[1];
        powerUpSound = audioSources[2];
        gameOverPanel.SetActive(false);
        pausePanael.SetActive(false);
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

        ComboBar.value = (float)ComboValue / (float)maxValueOfComboBar;
        if(ComboValue >= maxValueOfComboBar)
        {
            powerUpSound.PlayOneShot(powerUpSound.clip);
            attackPoint *= 1.05f;
            ComboValue = 0;
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;        
        if(e.type == EventType.KeyDown && e.type != EventType.KeyUp && e.keyCode != KeyCode.None)
        {           
            if(e.keyCode == KeyCode.Escape)
            {
                isInputValid = !isInputValid;
                if (isInputValid == true)
                {
                    Time.timeScale = 1.0f;
                    pausePanael.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0.0f;
                    pausePanael.SetActive(true);
                }
            }


            if (isInputValid == true)
            {
                if (e.keyCode == KeyCode.Tab)
                {
                    targetedEnemy = sightObj.GetComponent<Target>().ChangeRockOnEnemy(enemyIdList, enemys);
                }
                else
                {
                    if (targetedEnemy != null && keycodeToChar.ContainsKey(e.keyCode))
                    {
                        InputedLetterResult IsAttackValid = targetedEnemy.GetComponent<Enemy>().IsInputedLetter(keycodeToChar[e.keyCode]);
                        if(IsAttackValid.isCorrect == true)
                        {
                            ComboValue++;
                        }
                        else
                        {
                            ComboValue = 0;
                        }

                        if (IsAttackValid.isAttackValid == true)
                        {
                            Attack();
                        }
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
            isInputValid = false;
            int score = GameObject.Find("ScoreText").GetComponent<Score>().GetScore();            
            Text scoreText = gameOverPanel.transform.Find("ScoreText").GetComponent<Text>();
            scoreText.text = "SCORE:" + score;
            gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
