using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Data;  //DataTableを使う場合は、冒頭にこの1行を追加
using UnityEngine.UI;

public class SuberiInput : MonoBehaviour
{
    public int _currentReelNum;
    public int _currentSuberiCount;
    public string _reelName ;
    [SerializeField]
    string _flagName;

    Text textCurrentReelNum;
    Text textCurrentSuberiCount;
    Text textCurrentElementCount;
    Text textCurrentReelName;
    Text textCurrentFlagName;

    public Transform _ReelTransform;

    List<int> _suberiKomas = new List<int>();  


    void Start()
    {
        textCurrentReelNum = GameObject.Find("Num").GetComponent<Text>();
        textCurrentSuberiCount = GameObject.Find("Count").GetComponent<Text>();
        textCurrentElementCount = GameObject.Find("ElementCount").GetComponent<Text>();
        textCurrentReelName = GameObject.Find("ReelName").GetComponent<Text>();
        textCurrentFlagName = GameObject.Find("FlagName").GetComponent<Text>();
        ReelRotate(-17.1f); // １コマずらすと丁度０コマ目になる
        _currentReelNum = 0;
        _currentSuberiCount = 0;

        // フラグと現在のリールをセット
        _reelName = "Left";
        _flagName = "Replay";

        textCurrentReelName.text = _reelName.ToString();
        textCurrentFlagName.text = _flagName.ToString();

    }





    public void UpButton()
    {
        ReelRotate(17.1f);
        _currentSuberiCount--;
        AddReelNum(-1);
        TextUpdate();
    }

    public void DownButton()
    {
        ReelRotate(-17.1f);
        _currentSuberiCount++;
        AddReelNum(1);
        TextUpdate();
    }


    public void ResetButton()
    {
        _ReelTransform.transform.rotation = Quaternion.identity; // リール回転初期化
        ReelRotate(-17.1f); // １コマずらすと丁度０コマ目になる
        _currentSuberiCount = 0;
        _currentReelNum = 0;
        _suberiKomas.Clear(); // Listの要素を全削除
        TextUpdate();
    }



    /// <summary>
    /// テキスト更新
    /// </summary>
    void TextUpdate()
    {
        textCurrentReelNum.text = _currentReelNum.ToString();
        textCurrentSuberiCount.text = _currentSuberiCount.ToString();
        textCurrentElementCount.text = _suberiKomas.Count.ToString();
        textCurrentFlagName.text = _flagName.ToString();
        Debug.Log(string.Join(",", _suberiKomas));
    }

    /// <summary>
    /// リールを進めたり戻したり
    /// </summary>
    /// <param name="f"></param>
    void ReelRotate(float f)
    {
        _ReelTransform.transform.Rotate(new Vector3(f, 0, 0));
    }

    /// <summary>
    /// 決定ボタンを押してスベリコマを確定（決定後は１コマ滑る）
    /// </summary>
    /// <param name="i"></param>
    public void AddSuberiKoma()
    {
        int a = 0;
        a = _currentSuberiCount;
        for (int i = 0; i <= _currentSuberiCount; i++)
        {
            _suberiKomas.Add(a);
            a--;
        }

        ////a = _currentSuberiCount;
        //for (int i = 0; i <= _currentSuberiCount; i++)
        //{
        //    _suberiKomas.Add(a);
        //    a++;
        //}
        _currentSuberiCount = 0;
        ReelRotate(-17.1f);
        TextUpdate();
    }



    /// <summary>
    /// 現在のリール番号確認用
    /// </summary>
    /// <param name="i"></param>
    void AddReelNum(int i)
    {
        const int MAXREELKOMA = 21;

        _currentReelNum = _currentReelNum + i;

        if(_currentReelNum > MAXREELKOMA)
        {
            _currentReelNum = 0;
        }
        if(_currentReelNum < 0)
        {
            _currentReelNum = MAXREELKOMA;
        }
    }

   

    /// <summary>
    /// スベリコマ数を書き出し CSV?
    /// </summary>
    public void CSVDataExport()
    {
        string todayTime;
        DateTime dateTime = DateTime.Now;
        todayTime = dateTime.ToString();

        string fileName = _reelName + _flagName + todayTime;

        CSV_Export cSV_Export = new CSV_Export();

        cSV_Export.CSVSave(_suberiKomas, _flagName);

        Debug.Log("データを書き出しました");
    }

    

}
