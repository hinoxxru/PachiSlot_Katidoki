using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHousiki_Secound : MonoBehaviour
{
    

    // uint _all_StopReel_Bit; // 全リール判定
    uint _firstStop_ReelBit;   // 第１停止はどのリールか
    uint _secoundStop_ReelBit; // 第２停止はどのリールか
    uint _thirdStop_ReelBit;   // 第３停止はどのリールか

    // 第１停止の情報を入れる用
    int[] _firstStop_Deme_ints = new int[3]; // 2 上段
                                             // 1 中段
                                             // 0 下段

    // 第２停止の情報を入れる用
    int[] _secoundStop_ReelHairetu_ints = new int[7]; // 2 上段     6 ４コマスベリの範囲
                                                      // 1 中段     ↑
                                                      // 0 下段     3

    int[] tenpaiCheck_Order_first = new int[5];
    int[] tenpaiCheck_Order_secound = new int[5];

    uint   _currentFlag_LeftBit;   // フラグのBitを分割　左8bit
    uint   _currentFlag_CenterBit; // フラグのBitを分割　中8bit
    uint   _currentFlag_RightBit;  // フラグのBitを分割　右8bit

    int _suberiKoma; // テンパイ判定のスコープに入れようと思ったが、bool値を返したかったのでフィールド変数にした

    // モノビヘイビア承継してるけど、入賞チェッククラスに合わせて下手打ちする
    // 役名称から入賞判定用Bitを検索する辞書　キーが役名称　　取得方法は　zugaraNames[32] とか
    Dictionary<string, uint> _nyuusyouBit = new Dictionary<string, uint>()
        {
            { "Hazure",  0b_0000_0000_0000_0000_0000_0000},
            { "Replay" , 0b_0000_0001_0000_0001_0000_0001},
            { "Bell" ,   0b_0000_0010_0000_0010_0000_0010},
            { "Suika" ,  0b_0000_0100_0000_0100_0000_0100},
            { "Cherry" , 0b_0000_1000_1111_1111_1111_1111},
            { "Reg" ,    0b_0001_0000_0001_0000_0001_0000},
            { "Big" ,    0b_0010_0000_0010_0000_0010_0000}
            // { "Ichimaiyaku_A" }
        };


    // 蹴飛ばし制御で停止してはいけないコマ数をあらかじめ決めておく
    List<int> _dontStopKoma_Left = new List<int>   { 0, 1, 10, 11, 12, 14, 17, 20 }; // チェリー含む
    List<int> _dontStopKoma_Center = new List<int> { 0, 1, 13, 17 };
    List<int> _dontStopKoma_Right = new List<int>  { 10, 14, 18 };


    // Start is called before the first frame update
    void Start()
    {

    }


    /// <summary>
    /// 外部から呼ぶ用　引き込み制御と蹴飛ばし制御を行いスベリコマ数を返す（コントロール方式本体）
    /// </summary>
    /// <param name="stopKoma"></param>
    /// <returns></returns>
    public int Control_SecoundStop_Return_SuberiKoma(int stopKoma)
    {
        // ▼▼▼------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▼▼▼
        // Castインターフェイス方式に切替え後
        FlagData flagData = FlagData.GetInstance();
        FlagBit_8bitSplit(flagData._currentCast.GetCastBit()); // CastインターフェイスのGetCastBitメソッドでBitを取得
        // ▲▲▲------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▲▲▲

        //// ▼▼▼------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▼▼▼
        //// Debug.Log("C方式 引数でもらったフラグは " + flagNmae);
        //FlagBit_8bitSplit(_nyuusyouBit[flagNmae]); // 成立フラグのBitを分解して、左中右に分ける
        //// ▲▲▲------------- 引数　現在の成立フラグ -> 入賞判定用のBitを分解 ----------▲▲▲



        //////// ここ専用のメソッドにした方が良い
        // ▼▼▼------------ 今第何停止か？という情報と、押し順情報を取得 ------------▼▼▼
        StopReelData stopReelData = StopReelData.GetInstance();
        // _all_StopReel_Bit = stopReelData._allReelStop_Bit; // ここはいらないかもな〜
        this._firstStop_ReelBit = stopReelData._firstStop_Bit; // 第１停止はどのリールか
        // this._firstStop_ReelBit = this._firstStop_ReelBit & 0x0f; // 下位4bitだけ取得できるか？
        // Debug.Log("第１停止リールの停止ビットは" + Convert.ToString(_firstStop_ReelBit, 2));
        this._secoundStop_ReelBit = stopReelData._secoundStop_Bit; // 第２停止はどのリールか
        // Debug.Log("第２停止リールの停止ビットは" + Convert.ToString(_secoundStop_ReelBit, 2));
        // ▲▲▲------------ 今第何停止か？という情報と、押し順情報を取得 ------------▲▲▲


        // ▼▼▼------------ 第１停止の出目を取得 ------------▼▼▼
        string firstStop_ReelName = stopReelBit_ToReelName(_firstStop_ReelBit); // 対応リール名に変換
        DemeData demeData = DemeData.GetInstance();
        var firstDemeTuple = demeData.GetDemeDeta_int_tuple(firstStop_ReelName); // 出目情報をタプルで取得
        _firstStop_Deme_ints[2] = firstDemeTuple.jyoudann;
        _firstStop_Deme_ints[1] = firstDemeTuple.tyuudann;
        _firstStop_Deme_ints[0] = firstDemeTuple.gedann;
        // ▲▲▲------------ 第１停止の出目を取得 ------------▲▲▲


        // ▼▼▼----------- 第２停止のリール配列を取得 ---------▼▼▼
        string secoundStop_ReelName = stopReelBit_ToReelName(_secoundStop_ReelBit);
        // Debug.Log("判定に使うBitを試しに表示 " + Convert.ToString(0b100, 2));
        // Debug.Log("string変換用に渡したreelBit " + Convert.ToString(_secoundStop_ReelBit, 2));
        // Debug.Log("多分stringがnull " + secoundStop_ReelName);
        ReelHairetuData reelHairetuData = ReelHairetuData.GetInstance();
        // _secoundStop_ReelHairetu_ints = reelHairetuData.Get_ZugaraNum_7Koma_Tuple(stopKoma, secoundStop_ReelName);
        var tuple_7koma = reelHairetuData.Get_ZugaraNum_7koma_Tuple(stopKoma, secoundStop_ReelName);
        _secoundStop_ReelHairetu_ints[0] = tuple_7koma.gedann;
        _secoundStop_ReelHairetu_ints[1] = tuple_7koma.tyuudann;
        _secoundStop_ReelHairetu_ints[2] = tuple_7koma.jyoudann;
        _secoundStop_ReelHairetu_ints[3] = tuple_7koma.ue4Koma;
        _secoundStop_ReelHairetu_ints[4] = tuple_7koma.ue5Koma;
        _secoundStop_ReelHairetu_ints[5] = tuple_7koma.ue6Koma;
        _secoundStop_ReelHairetu_ints[6] = tuple_7koma.Ue7Koma;

        // この下はチェック用
        //for (int i = 0; i < 7; i++)
        //{
        //    Debug.Log(_secoundStop_ReelHairetu_ints[i] + "   戻り値で返ってきた第２停止のリール配列");
        //}
        // ▲▲▲----------- 第２停止のリール配列を取得 ---------▲▲▲


        _suberiKoma = 0; // スベリコマ数を初期化


        // ▼▼▼----------- 引き込み制御（テンパイ判定） ------------▼▼▼
        if (HikikomiSeigyo_Check()) // 引き込み可能でtrue
        {
            // Debug.Log("C方式 返すスベリコマ数は " + _suberiKoma);
            return _suberiKoma;
        }
        // ▲▲▲----------- 引き込み制御（テンパイ判定） ------------▲▲▲


        // ここDontStop検証用
        Debug.Log("引数の停止コマは " + stopKoma + " コマ目");
        // Debug.Log("第２停止のリール名は " + secoundStop_ReelName);

        // ▼▼▼----------- 蹴飛ばし制御 ------------▼▼▼
        for (int i = 0; i < 5; i++) // 最大４コマ(i<5で5回チェック)
        {
            // Debug.Log("蹴飛ばし制御（非停止コマ） " + i + "回目のループ");
            
            // 止まってはいけないコマに止まってないかチェック
            if (KetobasiSeigyo_DontStopKoma_Check(stopKoma, secoundStop_ReelName))
            {
                // Debug.Log("ドントストップで蹴飛ばし");
                continue; // if分がtrueだったら1コマ滑らせるから、次のループに戻り最初からチェックする
            }

            if (KetobasiSeigyo_HeikouTenpai_Check())
            {
                // Debug.Log("平行テンパイで蹴飛ばし");
                continue; // if分がtrueだったら1コマ滑らせるから、次のループに戻り最初からチェックする
            }

            if (KetobasiSeigyo_DabutenDame_Check())
            {
                // Debug.Log("ダブテンあったので蹴飛ばし");
                continue; // if分がtrueだったら1コマ滑らせるから、次のループに戻り最初からチェックする
            }
            return _suberiKoma;
        }
        // ▲▲▲----------- 蹴飛ばし制御 ------------▲▲▲


        return 0;
    }


    /// <summary>
    /// 止まってはいけないコマに止まってないかチェック
    /// </summary>
    /// <param name="stopKoma"></param>
    /// <param name="secoundStopReel"></param>
    /// <returns></returns>
    bool KetobasiSeigyo_DontStopKoma_Check(int stopKoma, string secoundStopReel)
    {
        int checkKoma = stopKoma + _suberiKoma;
        int convertCheckKoma = Over21KomaConverter(checkKoma);
        // Debug.Log("最終的なチェックコマは " + convertCheckKoma);
        if (secoundStopReel == "ReelLeftObj")
        {
            if (_dontStopKoma_Left.Contains(convertCheckKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma ++;
                return true;
            }
        }

        if (secoundStopReel == "ReelCenterObj")
        {
            if (_dontStopKoma_Center.Contains(convertCheckKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma++;
                return true;
            }
        }

        if (secoundStopReel == "ReelRightObj")
        {
            if (_dontStopKoma_Right.Contains(convertCheckKoma)) // 停止してはいけないコマ数だった場合 スベる
            {
                _suberiKoma++;
                return true;
            }
        }

        return false; // ここまで来たらスベリ無し
    }


    /// <summary>
    /// 中段（１ライン）が平行テンパイしないように蹴飛ばす
    /// </summary>
    /// <param name="stopKoma"></param>
    /// <returns></returns>
    bool KetobasiSeigyo_HeikouTenpai_Check()
    {
        if(_firstStop_Deme_ints[1] == _secoundStop_ReelHairetu_ints[1 + _suberiKoma]) // 中段ラインが同じ図柄だった場合スベらせる
        {
            _suberiKoma++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// ダブテンしないように蹴飛ばす制御
    /// </summary>
    /// <returns></returns>
    bool KetobasiSeigyo_DabutenDame_Check()
    {
        //var tenpai_Tuple = Order_HikikomiSeigyo_TenpaiCheck(_firstStop_ReelBit, _secoundStop_ReelBit);
        //tenpaiCheck_Order_first = tenpai_Tuple.first;
        //tenpaiCheck_Order_secound = tenpai_Tuple.secound;

        int tenpaiCount = 0;

        for (int indexCount = 0; indexCount < 5; indexCount++) // indexは0スタートなので4で5ライン全てチェック
        {
            // Debug.Log("indexCountは " + indexCount + "回目");
            // Debug.Log("_suberiKomaは " + _suberiKoma + "コマ");
            // Debug.Log("チェック箇所検証 第１は" + tenpaiCheck_Order_first[indexCount] + "番" + "　出目は " + _firstStop_Deme_ints[tenpaiCheck_Order_first[indexCount]]);
            // Debug.Log("チェック箇所検証 第２は" + (tenpaiCheck_Order_secound[indexCount] + _suberiKoma) + "番" + "　出目は " + _secoundStop_ReelHairetu_ints[tenpaiCheck_Order_secound[indexCount] + _suberiKoma]);
            // 第１出目と第２配列が同じか、１コマごとにチェック（全コマ）
            if (_firstStop_Deme_ints[tenpaiCheck_Order_first[indexCount]] == 
                _secoundStop_ReelHairetu_ints[tenpaiCheck_Order_secound[indexCount] + _suberiKoma]) // 第２配列はスベリコマも足す
            {
                tenpaiCount++; ; // これがReelScriptに返すスベリコマになる
                // Debug.Log("テンパイ加算 " + tenpaiCount + "個目");
            }

            if(tenpaiCount >= 2)
            {
                _suberiKoma++; // 滑らせる
                // Debug.Log("ダブテンありなので抜ける スベリコマは現在 " + _suberiKoma + " コマ");
                return true; 　// ダブテンあり 
            }
            // Debug.Log("まだテンパイ無し indexcout " + indexCount + " 回目");
            // Debug.Log("スベリカウント " + suberiCount + "回目");
        }
        // Debug.Log("ダブテン無しなので 戻る");
        return false; // ここまで来るとテンパイ不可という事で falseを返す
    }




    /// <summary>
    /// 引き込み制御の本体　引き込み可能範囲に小役があるか（テンパイするか？）
    /// </summary>
    /// <returns></returns>
    bool HikikomiSeigyo_Check()
    {
        var tenpai_Tuple = Order_HikikomiSeigyo_TenpaiCheck(_firstStop_ReelBit, _secoundStop_ReelBit);
        tenpaiCheck_Order_first = tenpai_Tuple.first;
        tenpaiCheck_Order_secound = tenpai_Tuple.secound;

        uint firstStop_FlagBit = stopReelBit_TocurrentFlagBit(_firstStop_ReelBit);
        uint secoundStop_FlagBit = stopReelBit_TocurrentFlagBit(_secoundStop_ReelBit);



        for(int suberiCount = 0; suberiCount < 4; suberiCount++) // スベリカウントが４コマを超えるまで（スベリコマで返す変数はフィールド_suberiKoma
        {
            for (int indexCount = 0; indexCount < 5; indexCount++) // indexは0スタートなので5になったら0に戻す
            {

                // 第１出目と成立フラグのチェック（出目情報の配列は3つの要素しか無いが、チェックする順番は5つの要素がある
                if (_firstStop_Deme_ints[ tenpaiCheck_Order_first [indexCount] ] == firstStop_FlagBit)
                {
                    // 第２停止の配列をチェック（第１出目と成立フラグが正しい場合）
                    if (_secoundStop_ReelHairetu_ints[tenpaiCheck_Order_secound[ indexCount ] + suberiCount] == secoundStop_FlagBit)
                    {                                           // ↑5ラインチェックしてテンパイが無い場合は、第２リールを１コマずらす
                        // Debug.Log("テンパイ!! 第２停止の引き込み制御");
                        _suberiKoma = suberiCount; // これがReelScriptに返すスベリコマになる
                        return true; // テンパイあり
                    }
                }

                // Debug.Log("まだテンパイ無し indexcout " + indexCount + " 回目");
                // Debug.Log("スベリカウント " + suberiCount + "回目");
            }
        }
        // Debug.Log("第２停止 引き込み範囲でテンパイ無し");
        return false; // ここまで来るとテンパイ不可という事で falseを返す
    }



    /// <summary>
    /// 押し順を元に、テンパイチェックのindexを返す
    /// </summary>
    /// <param name="F_bit"></param>
    /// <param name="S_Bit"></param>
    /// <returns></returns>
    public (int[] first, int[] secound)Order_HikikomiSeigyo_TenpaiCheck(uint F_bit, uint S_Bit)
    {
        int[] first = new int[5]   { 1, 2, 0, 0, 0}; // 上中段は120で固定 クロスは下で書き換え
        int[] secound = new int[5] { 1, 2, 0, 0, 0}; // 上中段は120で固定 クロスは下で書き換え

        if (F_bit == 0b1010) // 中押しなら（中右も中左も同じ）
        {
            if (S_Bit == 0b1001) // 中右なら
            {
                first[3] = 1;
                first[4] = 1;
                secound[3] = 0;
                secound[4] = 2;
                // Debug.Log("C方式より　中右");
                return (first, secound);
            }
            if (S_Bit == 0b1100) // 中左なら
            {
                first[3] = 1;
                first[4] = 1;
                secound[3] = 2;
                secound[4] = 0;
                // Debug.Log("C方式より　中押し");
                return (first, secound);
            }
        }

        if(F_bit == 0b1100) // 左ファーストなら
        {
            if(S_Bit == 0b1010) // 順押しなら
            {
                first[3] = 2;
                first[4] = 0;
                secound[3] = 1;
                secound[4] = 1;
                // Debug.Log("C方式より　順押し");
                return (first, secound);
            }
            if(S_Bit == 0b1001) // 順ハサミなら
            {
                first[3] = 2;
                first[4] = 0;
                secound[3] = 0;
                secound[4] = 2;
                // Debug.Log("C方式より　順ハサミ");
                return (first, secound);
            }
        }

        if(F_bit == 0b1001) // 右ファーストなら
        {
            if (S_Bit == 0b1010) // 逆押しなら
            {
                first[3] = 0;
                first[4] = 2;
                secound[3] = 1;
                secound[4] = 1;
                // Debug.Log("C方式より　逆押し");
                return (first, secound);
            }
            if (S_Bit == 0b1100) // 逆ハサミなら
            {
                first[3] = 0;
                first[4] = 2;
                secound[3] = 2;
                secound[4] = 0;
                // Debug.Log("C方式より　逆ハサミ");
                return (first, secound);
            }
        }

        // Debug.LogError("おそらく押し順判定でエラーです");
        return (first, secound);
    }



    /// <summary>
    /// 変換系　フラグのBitを分解　　右シフトして左、中、右を抜き取る
    /// </summary>
    /// <param name="flagBit"></param>
    void FlagBit_8bitSplit(uint flagBit)
    {
        _currentFlag_LeftBit = flagBit >> 16; // 左の8bitだけ残る
        // Debug.Log("左の8bitは " + _currentFlag_LeftBit);

        _currentFlag_CenterBit = flagBit >> 8;
        _currentFlag_CenterBit = _currentFlag_CenterBit & 0b_0000_0000_1111_1111; // 1と1意外は0になる
        // Debug.Log("中の8bitは " + _currentFlag_CenterBit);

        _currentFlag_RightBit = flagBit & 0b_0000_0000_0000_0000_1111_1111; // 右の8bitだけ残る　1と1意外は0になる
        // Debug.Log("右の8bitは " + _currentFlag_RightBit);
    }



    /// <summary>
    /// 変換系　Bitを元に対応するリール名を返す
    /// </summary>
    /// <param name="argsBit"></param>
    /// <returns></returns>
    string stopReelBit_ToReelName(uint argsBit)
    {
        if(argsBit == 0b1100) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelLeftObj";
        }
        else if (argsBit == 0b1010) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelCenterObj";
        }
        else if(argsBit == 0b1001) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return "ReelRightObj";
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 変換系　Bitを元に対応するcurrentBitを返す
    /// </summary>
    /// <param name="argsBit"></param>
    /// <returns></returns>
    uint stopReelBit_TocurrentFlagBit(uint argsBit)
    {
        if (argsBit == 0b1100) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_LeftBit;
        }
        else if (argsBit == 0b1010) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_CenterBit;
        }
        else if (argsBit == 0b1001) // リール判定は3bitでできるが、最小単位を4bitにしたので頭に1が付く
        {
            return _currentFlag_RightBit;
        }
        else
        {
            Debug.LogError("C方式 第２より 該当しません");
            return 0b0000;
        }
    }

    // ２１コマを超える場合は修正
    int Over21KomaConverter(int argsKoma)
    {
        if (argsKoma >= 21)
        {  // 21を超える場合は1からに変換
            argsKoma -= 21;
        }
        return argsKoma;
    }
}
