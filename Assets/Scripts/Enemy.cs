using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public uint Id { get; private set; }
    private float maxHp;
    private float hp;     //体力
    private string word;        //表示する単語
    private float timeLimit;    //攻撃までの制限時間
    private int wordIndex;
    private float startTime;
    private bool didAttack;
    private int[] charStatus;   //-1:ミス, 1:正解, 0:未決定
    private Text wordText;
    private Slider hpSlider;
    private Slider timeSlider;
    private GameObject EM;
    
    // Start is called before the first frame update
    void Start()
    {                
        EM = GameObject.Find("EnemyManager");
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = hp / maxHp;
        timeSlider.value = (timeLimit - (Time.time - startTime)) / timeLimit;
        if(Time.time - startTime >= timeLimit && didAttack == false)
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
        word = _word;
        charStatus = new int[word.Length];
        GameObject canvas = transform.Find("Canvas").gameObject;
        wordText = canvas.transform.Find("WordText").gameObject.GetComponent<Text>();
        timeSlider = canvas.transform.Find("Time").gameObject.GetComponent<Slider>();
        hpSlider = canvas.transform.Find("HP").gameObject.GetComponent<Slider>();
        wordText.text = word;
        timeLimit = _timeLimit;
        wordIndex = 0;
        didAttack = false;        
        MoveToFrontOfPlayer();
    }

    private void MoveToFrontOfPlayer()
    {
        startTime = Time.time;
    }

    private void Escape()
    {
        EM.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
        Destroy(this.gameObject);
    }
    

    public bool IsInputedLetter(char inputedChar)
    {        
        if(word[wordIndex] == inputedChar)
        {
            //文字を黒くする(見えなくする)
            charStatus[wordIndex] = 1;
        }
        else
        {
            //文字を赤くする
            charStatus[wordIndex] = -1;
            word = word.Remove(wordIndex, 1).Insert(wordIndex, inputedChar.ToString());
        }
        wordText.text = "";
        for(int i = 0; i < word.Length; i++)
        {
            if(charStatus[i] == 1)
            {
                wordText.text += "<color=#000000>" + word[i] + "</color>";
            }else if(charStatus[i] == -1)
            {
                wordText.text += "<color=#FF0000>" + word[i] + "</color>";
            }
            else
            {
                wordText.text += word[i];
            }
        }
        wordIndex++;
        if(wordIndex == word.Length - 1)
        {
            //攻撃を受ける
            return true;
        }
        return false;    
    }
    

    public void ReceiveDamage(float attackPoint)
    {        
        int SuccessCount = charStatus.Count(v => v == 1);        
        float attackScore = attackPoint * ((float)SuccessCount / word.Length);
        //Debug.Log(attackScore);
        hp -= attackScore;
        if (hp <= 0)
        {
            EM.GetComponent<EnemyGenerator>().enemyIds.Remove(Id);
            Destroy(this.gameObject);
        }
        else
        {
            UpdateWord();
        }
    }

    void UpdateWord()
    {
        word = EM.GetComponent<EnemyGenerator>().GetWord();
        wordText.text = word;
        charStatus = new int[word.Length];
        wordIndex = 0;
    }
    
}
