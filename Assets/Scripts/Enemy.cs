﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ILR;

public class Enemy : MonoBehaviour
{
    public uint         Id { get; private set; }
    private float       maxHp;
    private float       hp;
    private string      displayWord;
    private float       timeLimitUpToAttack;
    private int         wordIndex;
    private float       startTime;
    private bool        didAttack;
    private int[]       charStatus;   //-1:ミス, 1:正解, 0:未決定
    private int         detectedI;
    private int         detectedJ;
    private bool        isActive;
    private Vector3     destinationPosition;
    private Vector3     movingDirectionVector;
    private Text        wordText;
    private Slider      hpSlider;
    private Slider      timeSlider;
    private GameObject  EnemyManager;
    private GameObject  playerObj;
    private GameObject  scoreTextObj;
    private float       animationCriteriaTime;
    private bool        isRise;
    private AudioSource beamSound;    
    [SerializeField] GameObject beam;
    [SerializeField] GameObject explosionEffect;    


    public struct CandidatePosition
    {
        public bool canUse;
        public Vector3 position;

        public CandidatePosition(bool _canUse, Vector3 _position)
        {
            canUse = _canUse;
            position = _position;
        }
    }
    static CandidatePosition[,] candidatePositins = new CandidatePosition[3, 5];
    static bool isFinishedInitCP;

    
    static Enemy()
    {
        //配置される位置の候補を作成
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                candidatePositins[i, j] = new CandidatePosition(true, new Vector3(j * 3 - 6,i * -3 + 3, 9));
            }
        }
    }        

    // Start is called before the first frame update
    void Start()
    {        
        scoreTextObj = GameObject.Find("ScoreText");
        EnemyManager = GameObject.Find("EnemyManager");
        playerObj = GameObject.Find("Player");        
        beamSound = GetComponent<AudioSource>();        
        DetectPosition();
        SetColor();        
    }

    // Update is called once per frame
    void Update()
    {
        //指定された座標まで移動する
        const float moveSpeedms = 12.0f;
        const float toleranceDistanceSquare = 0.05f;
        float SquareOfCurrentPosToDestinationPos = (transform.position - destinationPosition).sqrMagnitude;
        if(isActive == false)
        {
            if (SquareOfCurrentPosToDestinationPos > toleranceDistanceSquare)
            {
                transform.Translate(movingDirectionVector * moveSpeedms * Time.deltaTime);
            }
            else
            {
                startTime = Time.time;
                AddMyIdToList();
                isActive = true;
                isRise = true;
                animationCriteriaTime = Time.time;
            }
        }        

        //HPバー，残り時間バーを更新
        hpSlider.value = hp / maxHp;
        if (isActive == true) timeSlider.value = (timeLimitUpToAttack - (Time.time - startTime)) / timeLimitUpToAttack;
        else timeSlider.value = 1.0f;

        //敵の待機アニメーション
        if (isActive && Time.timeScale > 0)
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0, Space.Self);            
            if (isRise)
            {
                transform.Translate(0, 0.002f, 0);
            }
            else
            {
                transform.Translate(0, -0.002f, 0);
            }
            if (Time.time - animationCriteriaTime > 2.0f)
            {
                isRise = !isRise;
                animationCriteriaTime = Time.time;
            }
        }

        //一定時間経つと攻撃
        if (Time.time - startTime >= timeLimitUpToAttack && didAttack == false && isActive == true)
        {

            Attack();
            didAttack = true;
            // 移動方向ベクトルを反転し，来た道を戻る
            movingDirectionVector = -1 * movingDirectionVector;
        }

        if(didAttack == true)
        {
            //撤退アニメーション
            transform.Translate(movingDirectionVector * moveSpeedms * Time.deltaTime);
            Destroy(this.gameObject, 2f);
        }
    }

    private void OnDestroy()
    {
        Terminate();
    }


    public void Initialize(uint _id, float _hp, string _word, float _timeLimit)
    {
        Id = _id;
        hp = _hp;
        maxHp = _hp;
        displayWord = _word;
        charStatus = new int[displayWord.Length];
        GameObject canvas = transform.Find("Canvas").gameObject;        
        timeSlider = canvas.transform.Find("Time").gameObject.GetComponent<Slider>();
        hpSlider = canvas.transform.Find("HP").gameObject.GetComponent<Slider>();
        InitializeWordPanel(canvas,displayWord);
        timeLimitUpToAttack = _timeLimit;
        wordIndex = 0;
        isActive = false;
        didAttack = false;                
    }


    void InitializeWordPanel(GameObject canvas, string displayWord)
    {
        GameObject wordObj = canvas.transform.Find("WordText").gameObject;
        wordText = wordObj.gameObject.GetComponent<Text>();
        displayWord = displayWord.Replace(" ", string.Empty);
        //文字数が8文字以上の場合、パネルを拡張
        if (displayWord.Length > 7)
        {
            int addWidth = (displayWord.Length - 7) * 20;
            wordObj.GetComponent<RectTransform>().sizeDelta = new Vector2(wordObj.GetComponent<RectTransform>().rect.width + addWidth, wordObj.GetComponent<RectTransform>().rect.height);
            GameObject panel = canvas.transform.Find("Panel").gameObject;
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().rect.width + addWidth, panel.GetComponent<RectTransform>().rect.height);
            GameObject frame = canvas.transform.Find("Frame").gameObject;
            frame.GetComponent<RectTransform>().sizeDelta = new Vector2(frame.GetComponent<RectTransform>().rect.width + addWidth, frame.GetComponent<RectTransform>().rect.height);
        }
        wordText.text = displayWord;
    }

        
    public InputedLetterResult IsInputedLetter(char inputedChar)
    {
        InputedLetterResult result = new InputedLetterResult(false, false);
        // 入力文字数が表示している単語の文字数を超えたらreturn
        if(wordIndex >= displayWord.Length - 1)
        {
            return result;
        }

        // 入力文字の正誤判定
        if(displayWord[wordIndex] == inputedChar)
        {            
            charStatus[wordIndex] = 1;
            result.isCorrect = true;
        }
        else
        {            
            charStatus[wordIndex] = -1;
            // 不正解の場合，入力された文字に入れ替える
            displayWord = displayWord.Remove(wordIndex, 1).Insert(wordIndex, inputedChar.ToString());
            result.isCorrect = false;
        }
        wordText.text = "";
        // 正しい入力の場合は黒色，そうでない場合は赤色にする
        for (int i = 0; i < displayWord.Length; i++)
        {
            if(charStatus[i] == 1)       wordText.text += "<color=#000000>" + displayWord[i] + "</color>";                
            else if(charStatus[i] == -1) wordText.text += "<color=#FF0000>" + displayWord[i] + "</color>";
            else                         wordText.text += displayWord[i];            
        }
        wordIndex++;
        if(wordIndex == displayWord.Length - 1)
        {
            //攻撃を受ける
            result.isAttackValid = true;  
        }
        return result;    
    }
    

    public void ReceiveDamage(float attackPoint)
    {        
        int SuccessCount = charStatus.Count(v => v == 1);      // 正解文字数  
        float attackScore = attackPoint * ((float)SuccessCount / displayWord.Length);        
        hp -= attackScore;
        if (hp <= 0)
        {            
            scoreTextObj.GetComponent<Score>().AddScore(100 + (EnemyManager.GetComponent<EnemyGenerator>().phase-1) * 50);
            EnemyManager.GetComponent<EnemyGenerator>().DestroyedEnemyNum++;
            Terminate();
            Destroy(this.gameObject);
            Explosion();
        }
        else
        {
            UpdateWord();
        }
    }

    void UpdateWord()
    {
        displayWord = EnemyManager.GetComponent<EnemyGenerator>().GetWord();
        wordText.text = displayWord;
        charStatus = new int[displayWord.Length];
        wordIndex = 0;
        GameObject canvas = transform.Find("Canvas").gameObject;
        InitializeWordPanel(canvas, displayWord);
        startTime = Time.time;
    }


    void SetColor()
    {
        int colorNumber = Random.Range(0, 10);
        float hue = colorNumber * 0.1f;
        float saturation = 1.0f;
        float value = 1.0f;
        Color color = Color.HSVToRGB(hue, saturation, value);
        Material mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
        float intensity = 3.2f;
        float factor = Mathf.Pow(2, intensity);
        GetComponent<Renderer>().material.SetColor("_EmissionColor" ,new Color(color.r*factor, color.g*factor, color.b*factor));
    }


    void DetectPosition()
    {
        // 生成された座標の象限の中から，最も遠い，かつ空いているところを配置する位置とする
        Vector3 generatedPosition = this.gameObject.transform.position;
        int beginY = 0, endY = 0, beginX = 0, endX = 0;        
        // 第1象限
        if (generatedPosition.x > 0 && generatedPosition.y > 0)
        {
            beginX = 2; endX = 5;
            beginY = 0; endY = 2;
        }
        // 第2象限
        if (generatedPosition.x > 0 && generatedPosition.y < 0)
        {
            beginX = 2; endX = 5;
            beginY = 1; endY = 3;
        }
        // 第3象限
        if (generatedPosition.x < 0 && generatedPosition.y > 0)
        {
            beginX = 0; endX = 3;
            beginY = 0; endY = 2;
        }
        // 第4象限
        if (generatedPosition.x < 0 && generatedPosition.y < 0)
        {
            beginX = 0; endX = 2;
            beginY = 1; endY = 3;
        }
        
        float maxDistance = -1f;
        for (int i = beginY; i < endY; i++)
        {
            for (int j = beginX; j < endX; j++)
            {
                float distance = (generatedPosition - candidatePositins[i, j].position).sqrMagnitude;
                if (distance > maxDistance && candidatePositins[i, j].canUse == true)
                {
                    maxDistance = distance;
                    detectedI = i;
                    detectedJ = j;
                }
            }
        }
        if (maxDistance == -1) Destroy(this.gameObject); 
        destinationPosition = candidatePositins[detectedI, detectedJ].position;
        candidatePositins[detectedI, detectedJ].canUse = false;
        movingDirectionVector = (destinationPosition - transform.position).normalized;
    }


    void AddMyIdToList()
    {
        EnemyManager.GetComponent<EnemyGenerator>().enemyIds.Add(Id);
    }


    void Attack()
    {
        beamSound.PlayOneShot(beamSound.clip);
        GameObject beamInstance = Instantiate(beam, this.transform.position, Quaternion.identity);             
        beamInstance.GetComponent<Beam>().Initialize(false, "Player", playerObj);
        Terminate();
    }


    private void Explosion()
    {        
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ParticleSystem ps = explosionInstance.GetComponent<ParticleSystem>();        
        Destroy(explosionInstance, ps.main.duration);
    }


    private void Terminate()
    {
        EnemyManager.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
        candidatePositins[detectedI, detectedJ].canUse = true;
    }

}