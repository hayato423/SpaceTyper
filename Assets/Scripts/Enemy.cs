using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public uint Id { get; private set; }
    private int hp;     //体力
    private string word;        //表示する単語
    private float timeLimit;    //攻撃までの制限時間
    private int wordIndex;
    private float startTime;
    private bool didAttack;
    private Text wordText;
    private Slider hpSlider;
    private Slider timeSlider;
    
    // Start is called before the first frame update
    void Start()
    {                
    }

    // Update is called once per frame
    void Update()
    {
        timeSlider.value = (timeLimit - (Time.time - startTime)) / timeLimit;
        if(Time.time - startTime >= timeLimit && didAttack == false)
        {
            Debug.Log("攻撃");
            didAttack = true;
            Escape();
        }
    }
    public void Initialize(uint _id, int _hp, string _word, float _timeLimit)
    {
        Id = _id;
        hp = _hp;
        word = _word;
        GameObject canvas = transform.Find("Canvas").gameObject;
        wordText = canvas.transform.Find("WordText").gameObject.GetComponent<Text>();
        timeSlider = canvas.transform.Find("Time").gameObject.GetComponent<Slider>();
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
        Destroy(this.gameObject);
    }
    

    public bool InputLetter(char c)
    {        
        if(word[wordIndex] == c)
        {
            //文字を黒くする(見えなくする)
        }
        else
        {
            //文字を赤くする
        }
        wordIndex++;
        if(wordIndex == word.Length - 1)
        {
            //攻撃を受ける
            return true;
        }
        return false;    
    }
    
    
}
