using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public uint Id { get; private set; }
    private int hp;     //体力
    private string word;        //表示する単語
    private float timeLimit;    //攻撃までの制限時間
    private int wordIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(uint _id, int _hp, string _word, float _timeLimit)
    {
        Id = _id;
        hp = _hp;
        word = _word;
        timeLimit = _timeLimit;
        wordIndex = 0;
    }

    private void MoveToFrontOfPlayer() { }
    

    public void InputLetter(uint id, char c)
    {
        if(id == Id)
        {
            if(word[wordIndex++] == c)
            {
                //文字を黒くする(見えなくする)
            }
            else
            {
                //文字を赤くする
            }
            if(wordIndex == word.Length - 1)
            {
                //攻撃を受ける
            }
        }
    }

    private void UpdateWord() { }
}
