using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public uint Id { get; private set; }
    private float maxHp;
    private float hp;
    private string displayWord;
    private float timeLimitUpToAttack;
    private int wordIndex;
    private float startTime;
    private bool didAttack;
    private int[] charStatus;   //-1:ミス, 1:正解, 0:未決定
    private int detectedI = 0;
    private int detectedJ = 0;
    public bool isActive { get; private set; }
    private Vector3 destinationPosition;
    private Vector3 movingDirectionVector;
    private Text wordText;
    private Slider hpSlider;
    private Slider timeSlider;
    private GameObject EnemyManager;
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
    static CandidatePosition[,] candidatePositins = new CandidatePosition[3,5];

    static Enemy()
    {
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
        EnemyManager = GameObject.Find("EnemyManager");

        DetectPosition();
        SetColor();        
    }

    // Update is called once per frame
    void Update()
    {
        //指定された座標まで移動する
        const float moveSpeedms = 12.0f;
        const float toleranceDistanceSquare = 0.01f;
        float SquareOfCurrentPosToDestinationPos = (transform.position - destinationPosition).sqrMagnitude;
        if (SquareOfCurrentPosToDestinationPos > toleranceDistanceSquare)
        {
            transform.Translate(movingDirectionVector * moveSpeedms * Time.deltaTime);
        } 
        else if(isActive == false)
        {
            startTime = Time.time;
            AddMyIdToList();
            isActive = true;
        }

        //HPバー，残り時間バーを更新
        hpSlider.value = hp / maxHp;
        if (isActive == true) timeSlider.value = (timeLimitUpToAttack - (Time.time - startTime)) / timeLimitUpToAttack;
        else timeSlider.value = 1.0f;

        if (Time.time - startTime >= timeLimitUpToAttack && didAttack == false && isActive == true)
        {
            Debug.Log("攻撃");
            didAttack = true;
            Escape();
        }
    }
    public void Initialize(uint _id, float _hp, string _word, float _timeLimit)
    {
        Id = _id;
        hp = _hp;
        maxHp = _hp;
        displayWord = _word;
        charStatus = new int[displayWord.Length];
        GameObject canvas = transform.Find("Canvas").gameObject;
        GameObject wordObj = canvas.transform.Find("WordText").gameObject;
        wordText = wordObj.gameObject.GetComponent<Text>();
        timeSlider = canvas.transform.Find("Time").gameObject.GetComponent<Slider>();
        hpSlider = canvas.transform.Find("HP").gameObject.GetComponent<Slider>();
        wordText.text = displayWord;
        timeLimitUpToAttack = _timeLimit;
        wordIndex = 0;
        isActive = false;
        didAttack = false;        
        //文字数が8文字以上の場合、パネルを拡張
        if(wordText.text.Length > 8)
        {
            int addWidth = (wordText.text.Length - 8) * 20;
            wordObj.GetComponent<RectTransform>().sizeDelta = new Vector2(wordObj.GetComponent<RectTransform>().rect.width+addWidth, wordObj.GetComponent<RectTransform>().rect.height);
            GameObject panel = canvas.transform.Find("Panel").gameObject;
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().rect.width + addWidth, panel.GetComponent<RectTransform>().rect.height);
            GameObject frame = canvas.transform.Find("Frame").gameObject;
            frame.GetComponent<RectTransform>().sizeDelta = new Vector2(frame.GetComponent<RectTransform>().rect.width + addWidth, frame.GetComponent<RectTransform>().rect.height);
        }        
    }


    private void OnDestroy()
    {
        EnemyManager.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
        candidatePositins[detectedI, detectedJ].canUse = true;
    }

    private void Escape()
    {
        //EnemyManager.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
        //candidatePositins[detectedI, detectedJ].canUse = true;
        Destroy(this.gameObject);
    }

    

    public bool IsInputedLetter(char inputedChar)
    {        
        if(displayWord[wordIndex] == inputedChar)
        {
            //文字を黒くする(見えなくする)
            charStatus[wordIndex] = 1;
        }
        else
        {
            //文字を赤くする
            charStatus[wordIndex] = -1;
            displayWord = displayWord.Remove(wordIndex, 1).Insert(wordIndex, inputedChar.ToString());
        }
        wordText.text = "";
        for(int i = 0; i < displayWord.Length; i++)
        {
            if(charStatus[i] == 1)
            {
                wordText.text += "<color=#000000>" + displayWord[i] + "</color>";
            }else if(charStatus[i] == -1)
            {
                wordText.text += "<color=#FF0000>" + displayWord[i] + "</color>";
            }
            else
            {
                wordText.text += displayWord[i];
            }
        }
        wordIndex++;
        if(wordIndex == displayWord.Length - 1)
        {
            //攻撃を受ける
            return true;
        }
        return false;    
    }
    

    public void ReceiveDamage(float attackPoint)
    {        
        int SuccessCount = charStatus.Count(v => v == 1);        
        float attackScore = attackPoint * ((float)SuccessCount / displayWord.Length);
        //Debug.Log(attackScore);
        hp -= attackScore;
        if (hp <= 0)
        {
            //EnemyManager.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
            //candidatePositins[detectedI, detectedJ].canUse = true;
            Destroy(this.gameObject);
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
        float intensity = 4f;
        float factor = Mathf.Pow(2, intensity);
        GetComponent<Renderer>().material.SetColor("_EmissionColor" ,new Color(color.r*factor, color.g*factor, color.b*factor));
    }


    void DetectPosition()
    {
        //生成された座標の象限の中から，最も遠い，かつ空いているところを配置する位置に決定する
        Vector3 generatedPosition = this.gameObject.transform.position;
        int beginY = 0, endY = 0, beginX = 0, endX = 0;
        if (generatedPosition.x < 0 && generatedPosition.y > 0)
        {
            beginX = 0; endX = 3;
            beginY = 0; endY = 2;
        }
        if (generatedPosition.x > 0 && generatedPosition.y > 0)
        {
            beginX = 2; endX = 5;
            beginY = 0; endY = 2;
        }
        if (generatedPosition.x < 0 && generatedPosition.y < 0)
        {
            beginX = 0; endX = 2;
            beginY = 1; endY = 3;
        }
        if (generatedPosition.x > 0 && generatedPosition.y < 0)
        {
            beginX = 2; endX = 5;
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
    
}
