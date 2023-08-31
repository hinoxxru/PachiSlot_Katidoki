using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;


public class Reel_Script : MonoBehaviour
{
    // ------テスト用----------------
    // public Text _text;
    // -----------------


    Rigidbody _ReelRigidbody;
    Transform _ReelTransform;

    float _angleSpeed = 477.45f; // 1分に80回転（つまり 360° * 80回転 / 60秒 = 480°/毎秒 となる）
    Vector3 _axis = Vector3.left; // リール回転の軸方向　Vector3(-1, 0, 0)と同じ意味

    // 回転判定 プロパティ
    private bool _isSpinning { get; set; } // 回っているか？フラグ　tureで回転　falseで停止

    // スベリコマ調整用の変数　ストップボタン押下時の停止コマ数
    int _stopKomaNum;

    int _indicateKomaNum; // _stopKomaNum + 調整したコマ数を足したもの（スベリコマ数とか）

    void Start()
    {
        _ReelRigidbody = GetComponent<Rigidbody>();
        _ReelTransform = this.transform;
        _ReelRigidbody.useGravity = false; // 重力を無効にする
        _isSpinning = false;
    }

    
    void Update()
    {

        if (_isSpinning)
        {
            // １フレームで回転する角度を角速度から計算
            float angle = _angleSpeed * Time.deltaTime;

            // 既存のrotationに軸回転のクォータニオンを掛ける 掛ける順序に注意
            _ReelTransform.rotation = Quaternion.AngleAxis(angle, _axis) * _ReelTransform.rotation;
        }
        
    }




    /// <summary>
    /// 別のスクリプトから呼び出して回転判定boolを取得
    /// </summary>
    /// <returns></returns>
    public bool GetterIsSpinning()
    {
        return _isSpinning;
    }


    /// <summary>
    /// リールを回す（別スクリプトから呼び出せる）
    /// </summary>
    public void CallReelSpin()
    {
        this._isSpinning = true;
        _indicateKomaNum = 0; // 回転時に初期化
    }


    /// <summary>
    /// リールを停止（別スクリプトから呼び出せる）
    /// </summary>
    public void CallReelStop()
    {
        this._isSpinning = false;
        // IsStop_DataSet();
        Set_ReelStop_BitData();
        float stopAngle = convertQuaternionToEuler(); // 停止角度を取得
        // Debug.Log("停止位置は " + stopAngle);
        StopAngleAdjustment(stopAngle); // 停止位置を１コマ画角内に調整
        StopHousikiSelect();
        // Suberi_TableHousiki();
        NaturalStop(); // 停止位置を意図的にちょっとズラす
        IsStop_DataSet();
        // Set_ReelStop_BitData();
        SetDemeData(); // DemeDataクラスに出目をセット
        // Debug.Log("出目情報セットしたよ");
        GetDemeData(); // テスト用
    }



    /// <summary>
    /// リールを滑らせる（別スクリプトから呼び出せる）
    /// </summary>
    /// <param name="addSuberiKoma">滑らせるコマ数</param> 
    public void CallReelSuberi(int addSuberiKoma)
    {
        float reel_1KomaAngle = 17.1f; // １コマあたりの角度
        float addAngle = reel_1KomaAngle * addSuberiKoma;

        // Debug.Log("スベリ制御 受け取ったコマ数は " + addSuberiKoma + " コマ");
        // リールのスベリを実機に近づけるために -> リールは1回転 0.754秒  つまり1コマ0.0359秒　DOTWeenの時間は正確なため Time.deltaTime は多分いらない
        float suberiSec = 0.0359f * addSuberiKoma; // スベリコマ数が増えれば時間もかかる
        _ReelTransform.DOLocalRotate(new Vector3(addAngle * -1, 0f, 0f), suberiSec, RotateMode.LocalAxisAdd);
        // _ReelTransform.Rotate(new Vector3(addAngle * -1, 0f, 0f));
    }


    /// <summary>
    /// よりリアルなリール停止にするため、微妙に停止位置をズラす
    /// </summary>
    void NaturalStop()
    {
        float threshold = 3; // 停止時に微妙にズラすための閾(しきい)値
        float addAngle = UnityEngine.Random.Range(0.0f, threshold);
        _ReelTransform.Rotate(new Vector3(addAngle * -1, 0f, 0f));
    }




    /// <summary>
    /// 停止時に使用するメソッド　クォータニオンをオイラー角に変換
    /// </summary>
    float convertQuaternionToEuler()
    {
        float stopAngle  = 999; // 999が返ってきたら未処理
        Vector3 quaAngle = new Vector3();
        quaAngle = _ReelTransform.forward;

        if(quaAngle.z >= 0) // 正の値の範囲なら
        {
            float f = _ReelTransform.eulerAngles.x; // オイラー角を取得
            if(f >= 270 && f < 360) // 270以上 360未満
            {   // Aの範囲
                stopAngle = 360 - f;
            }
            else if(f >= 0 && f < 90) // 0以上 90未満
            {   // Dの範囲
                stopAngle = 270 + (90 - f);
            }
            else
            {
                Debug.LogError("停止位置判定エラー オイラー角 " + f );
            }
        }
        else if (quaAngle.z < 0) // 負の値の範囲なら
        {
            float f = _ReelTransform.eulerAngles.x; // オイラー角を取得

            if (f >= 270 && f < 360) // 270以上 360未満
            {   // Bの範囲
                stopAngle = 90 + (f - 270);
            }
            else if (f >= 0 && f < 90) // 0以上 90未満
            {   // Dの範囲
                stopAngle = 180 + f;
            }
            else
            {
                Debug.LogError("停止位置判定エラー オイラー角がif範囲外 " + f);
            }
        }
        else
        {
            Debug.LogError("停止位置判定エラー クォータニオンがif範囲外 " + quaAngle);
        }

        return stopAngle;
    }



    /// <summary>
    /// 停止時に使用するメソッド　停止位置を1コマ単位に調整
    /// </summary>
    void StopAngleAdjustment(float stopAngle)
    {
        float reel_1KomaAngle = 17.1f; // １コマあたりの角度
        // Debug.Log("stopAngle（停止）は " + stopAngle);

        // -------ここから↓が停止位置を調整-----------
        float surplusAngle = stopAngle % reel_1KomaAngle;
        // Debug.Log("surplusAngle（余り）は " + surplusAngle);

        float addAngle = 0; // １コマ単位に調整するための変数
        if (surplusAngle != 0) // 停止位置が17.1で割り切れる場合意外は位置調整する
        {
            addAngle = (reel_1KomaAngle - surplusAngle) * -1; // 17.1 から 余りを引く つまり不足分算出
            _ReelTransform.Rotate(new Vector3(addAngle, 0f, 0f));
            // Debug.Log("addAngle（調整）は " + addAngle);
            // Debug.Log(stopAngle + " から停止位置調整 " + addAngle + " 減算しました");
        }

        // -------コマ数算出---------
        _stopKomaNum = 0; // 初期化
        float komaNum = (stopAngle + (addAngle * -1)) / reel_1KomaAngle; // ★★★これが正しいのかも微妙なところ2023/1/23 
        // Debug.Log("停止コマ数（割る前）は " + (stopAngle + (addAngle * -1)));


        // コマ数をリールNo,と合わせる（リールNo,は１スタート、コマ数は０スタート）
        //if (komaNum < 21) // 21より小さい（20以下）
        //{
        //    komaNum++;
        //}
        //else if (komaNum >= 21) // 21以上なら
        //{
        //    komaNum = komaNum - 20;
        //}
        // Debug.Log("停止コマ数（割る前）は " + komaNum);

        // Debug.Log("リールスクリプトより 調整前のkomaNumは " + komaNum);

        //if (komaNum <= 1)
        //{
        //    komaNum = komaNum -1;
        //}

        if(komaNum >= 21) // 21コマ以上なら0コマにする
        {
            komaNum = komaNum - 21;
        }

        // Debug.Log("リールスクリプトより komaNumは " + komaNum);

        _stopKomaNum = (int)Mathf.Round(komaNum); // 偶数丸めでコマ数を決めるのはおかしい（17.1の半分暗いのが変？）
        // Debug.Log("停止コマ数は " + Mathf.Round(komaNum));
        // Debug.Log("停止コマ数（整数）は " + _stopKomaNum);
        // ---------------------------------------------

        _indicateKomaNum += _stopKomaNum; // 停止位置を代入
    }


    /// <summary>
    /// 第１停止はテーブル方式　第２第３はコントロール方式を選択する
    /// </summary>
    void StopHousikiSelect()
    {
        StopReelData stopReelData = StopReelData.GetInstance();
        uint currentStopBit = stopReelData._allReelStop_Bit;
        // Debug.Log("停止時のBitは" + currentStopBit);

        // 第１停止　　停止bitのどこか一つが0の場合は第１停止
        if (currentStopBit == 0b011 || currentStopBit == 0b101 || currentStopBit == 0b110) // 第１停止ステートの場合
        {
            // Debug.Log("第１停止！テーブル方式");
            Suberi_TableHousiki();
        }
        // 第２停止　　bitの0が二つ
        else if (currentStopBit == 0b001 || currentStopBit == 0b010 || currentStopBit == 0b100)// 第２停止なら
        {
            // Debug.Log("第２停止！C方式を呼び出し");
            Suberi_ControlHousiki_Secound();
        }
        // 第３停止　　bitが全て0
        else if (currentStopBit == 0b000)// 第３停止なら
        {
            // Debug.Log("第３停止！");
            Suberi_ControlHousiki_Third();
        }
        else
        {
            Debug.LogError("正常に停止bit情報を取得できませんでした");
        }
    }


    // テーブル方式 ->　別Scriptにして第１停止ステートで呼び出すようにする？

    /// <summary>
    /// テーブルを参照して
    /// </summary>
    void Suberi_TableHousiki()
    {
        TableHousiki_Script tableHousiki_Script = GameObject.Find("ScriptObj").GetComponent<TableHousiki_Script>();
        int adjustmentKoma = tableHousiki_Script.ReturnSuberiKoma(_stopKomaNum) ; // ★_stopKomaNumは要素数１から２１なので１プラスする
        Debug.Log("スベリコマ数は " + adjustmentKoma + " コマ");

        _indicateKomaNum  += adjustmentKoma; // スベリコマ数を加算

        CallReelSuberi(adjustmentKoma); // リールを引数分、滑らせる
    }

    /// <summary>
    /// コントロール方式でスベリコマを取得（第２停止用）
    /// </summary>
    void Suberi_ControlHousiki_Secound()
    {
        ControlHousiki_Secound controlHousiki_Secound = GameObject.Find("ScriptObj").GetComponent<ControlHousiki_Secound>();
        //int adjustmentKoma = controlHousiki_Secound.Control_SecoundStop_Return_SuberiKoma(flagName, _stopKomaNum);
        int adjustmentKoma = controlHousiki_Secound.Control_SecoundStop_Return_SuberiKoma(_stopKomaNum);

        // Debug.Log("リールより 戻り値のスベリコマ数 " + adjustmentKoma);
        _indicateKomaNum += adjustmentKoma; // スベリコマ数を加算
        // int test = 10;
        // CallReelSuberi(test);
        CallReelSuberi(adjustmentKoma); // リールを引数分、滑らせる
    }


    /// <summary>
    /// コントロール方式でスベリコマを取得（第３停止用）
    /// </summary>
    void Suberi_ControlHousiki_Third()
    {
        ControlHousiki_Third controlHousiki_Third = GameObject.Find("ScriptObj").GetComponent<ControlHousiki_Third>();
        // int adjustmentKoma = controlHousiki_Third.Control_ThirdStop_Return_SuberiKoma(flagName, _stopKomaNum);
        int adjustmentKoma = controlHousiki_Third.Control_ThirdStop_Return_SuberiKoma(_stopKomaNum);

        // Debug.Log("リールより 戻り値のスベリコマ数 " + adjustmentKoma);
        _indicateKomaNum += adjustmentKoma; // スベリコマ数を加算
        // int test = 10;
        // CallReelSuberi(test);
        CallReelSuberi(adjustmentKoma); // リールを引数分、滑らせる
    }


    /// <summary>
    /// 出目情報をセット
    /// </summary>
    void SetDemeData()
    {
        DemeData demeData = DemeData.GetInstance();
        demeData.SetDemeData(_indicateKomaNum, this.gameObject.name);
    }


    // テスト用　すでに消してもどこにも影響なし
    void GetDemeData()
    {
        DemeData demeData = DemeData.GetInstance();
        //var tuple = demeData.GetDemeDeta_int_tuple(this.gameObject.name); // int
        var tuple = demeData.GetDemeDeta_string_tuple(this.gameObject.name); // string
        Debug.Log("上段の図柄番号は " + tuple.jyoudann);
        Debug.Log("中段の図柄番号は " + tuple.tyuudann);
        Debug.Log("下段の図柄番号は " + tuple.gedann);
    }


    /// <summary>
    /// 自分が停止した際に、停止した事をDataにセット
    /// </summary>
    void IsStop_DataSet()
    {
        StopReelData stopReelData = StopReelData.GetInstance();
        if(this.gameObject.name == "ReelLeftObj")
        {
            stopReelData._allReelStop_Bit = stopReelData._allReelStop_Bit & 0b011; // 左を0フラグにする（他のリールが既に0でもそこは0になる
        }

        if (this.gameObject.name == "ReelCenterObj")
        {
            stopReelData._allReelStop_Bit = stopReelData._allReelStop_Bit & 0b101; // 左を0フラグにする（他のリールが既に0でもそこは0になる
        }

        if (this.gameObject.name == "ReelRightObj")
        {
            stopReelData._allReelStop_Bit = stopReelData._allReelStop_Bit & 0b110; // 左を0フラグにする（他のリールが既に0でもそこは0になる
        }
    }


    /// <summary>
    /// StopReelDataに押し順情報、どのリールが止まったかの情報をセットする
    /// </summary>
    void Set_ReelStop_BitData()
    {
        StopReelData stopReelData = StopReelData.GetInstance();
        bool b;
        if (this.gameObject.name == "ReelLeftObj")
        {
            b = stopReelData.SetBitInformation(0b011); // 押し順とどのリールが停止したかをセット
        }

        if (this.gameObject.name == "ReelCenterObj")
        {
            b = stopReelData.SetBitInformation(0b101); // 押し順とどのリールが停止したかをセット
        }

        if (this.gameObject.name == "ReelRightObj")
        {
            b = stopReelData.SetBitInformation(0b110); // 押し順とどのリールが停止したかをセット
        }

        //Debug.Log("全リールの停止情報は " + Convert.ToString(stopReelData._allReelStop_Bit, 2));
        //Debug.Log("第１は " + Convert.ToString(stopReelData._firstStop_Bit, 2) +
        //          " 第２は " + Convert.ToString(stopReelData._secoundStop_Bit, 2) +
        //          " 第３は " + Convert.ToString(stopReelData._thirdStop_Bit, 2));
    }
    
}
