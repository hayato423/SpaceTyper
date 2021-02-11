﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject enemy_prefab;   //敵オブジェクト
    private uint idNum;        //割り振るid
    private string[] words;
    [SerializeField] float interval;
    private float startTime;
    private float enemyTimeLimit;
    private int enemyHp;
    private int phase;
    public List<uint> enemyIds;
    // Start is called before the first frame update
    void Start()
    {
        enemyIds = new List<uint>();
        startTime = Time.time;
        enemyTimeLimit = 5.0f;
        idNum = 1;
        phase = 1;
        enemyHp = 1;
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {        
        if(Time.time - startTime > interval)
        {
            GenerateEnemy();
            enemyIds.Add(idNum);
            idNum++;
            startTime = Time.time;
        }
        //敵を10体生成する度にフェーズを1上げる
        if(idNum > phase * 10)
        {
            phase++;
            if(interval > 0.3f) interval = interval * (1.0f - ((phase-1) * 0.1f));
            enemyHp += phase;
        }
    }

    //敵を生成する
    private void GenerateEnemy()
    {
        float x = Random.Range(-10f, 10f);
        string word = GetWord();
        GameObject enemy = Instantiate(enemy_prefab, new Vector3(x, 0f, 0f), Quaternion.identity) as GameObject;
        enemy.GetComponent<Enemy>().Initialize(idNum,enemyHp,word,enemyTimeLimit);
    }

    //テキストファイル読み込み
    private void ReadFile()
    {
        TextAsset textAsset = new TextAsset();
        textAsset = Resources.Load("EnglishWords", typeof(TextAsset)) as TextAsset;
        string textLines = textAsset.text;
        words = textLines.Split('\n');
    }

    public string GetWord()
    {
        int wordIndex = Random.Range(0, words.Length);        
        return words[wordIndex];
    }
}
