using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{    
    private uint     idNum;        //割り振るid
    private string[] words;
    private float    startTime;
    private int      destoroyedEnemyNum = 0;
    [SerializeField] GameObject enemy_prefab;   //敵オブジェクト
    [SerializeField] float      interval;
    [SerializeField] float      enemyTimeLimit;
    [SerializeField] float      enemyHp;
    public int DestroyedEnemyNum
    {
        get { return destoroyedEnemyNum; }
        set { destoroyedEnemyNum++; }
    } 
    public int phase;
    public List<uint> enemyIds;    
    // Start is called before the first frame update
    void Start()
    {        
        enemyIds  = new List<uint>();
        startTime = Time.time;        
        idNum     = 1;
        phase     = 1;
        enemyHp   = 1;
        ReadFile();        
    }

    // Update is called once per frame
    void Update()
    {        
        if(Time.time - startTime > interval)
        {
            GenerateEnemy();            
            startTime = Time.time;
        }
        if(DestroyedEnemyNum > phase * 10)
        {
            phase++;
            enemyHp += phase * 0.1f;
            if(interval > 1.0f) interval -= 0.2f;
        }                
    }

    //敵を生成する
    private void GenerateEnemy()
    {        
        //円周上に敵を生成する
        float radius = 12.0f;
        float degree = Random.Range(0, 360);
        float x = radius * Mathf.Cos(degree * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(degree * Mathf.Deg2Rad);
        float z = 9.0f;        
        GameObject enemy = Instantiate(enemy_prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        string word = GetWord();
        enemy.GetComponent<Enemy>().Initialize(idNum,enemyHp,word,enemyTimeLimit);        
        idNum++;
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
