using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OshijyunScript : MonoBehaviour
{
    Dictionary<uint, OshijyunBase> GetOshijyun = new Dictionary<uint, OshijyunBase>();


    private void Start()
    {
        JyunOshi jyunOshi = new JyunOshi(); // 順押し
        GetOshijyun.Add(0b0100_0010_0000, jyunOshi);
        GetOshijyun.Add(0b0100_0010_0001,jyunOshi);
        JyunHasami jyunHasami = new JyunHasami(); // 順ハサミ
        GetOshijyun.Add(0b0100_0001_0000, jyunHasami);
        GetOshijyun.Add(0b0100_0001_0010, jyunHasami);
        NakaHidari nakaHidari = new NakaHidari(); // 中左
        GetOshijyun.Add(0b0010_0100_0000, nakaHidari);
        GetOshijyun.Add(0b0010_0100_0001, nakaHidari);
        NakaMigi nakaMigi = new NakaMigi(); // 中右
        GetOshijyun.Add(0b0010_0001_0000, nakaMigi);
        GetOshijyun.Add(0b0010_0001_0100, nakaMigi);
        GyakuOshi gyakuOshi = new GyakuOshi(); // 逆押し
        GetOshijyun.Add(0b0001_0010_0000, gyakuOshi);
        GetOshijyun.Add(0b0001_0010_0100, gyakuOshi);
        GyakuHasami gyakuHasami = new GyakuHasami(); // 逆ハサミ
        GetOshijyun.Add(0b0001_0100_0000, gyakuHasami);
        GetOshijyun.Add(0b0001_0100_0010, gyakuHasami);

        //// 呼び出し方　要素１つずつアクセスできる
        //int i = jyunOshi.Get_LineCheck_Num_First()[4];
        //Debug.Log("押し順テスト 要素は" + i);
    }
}


/// <summary>
/// 抽象クラス
/// </summary>
public abstract class OshijyunBase
{
    public virtual string GetOshijyunName()
    {
        return "押し順の名前";
    }
    public virtual int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[] { 1, 2, 0, 0, 0};
        return checkOrder_First;
    }
    public virtual int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 0, 0 };
        return checkOrder_Secound;
    }
    public virtual int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[] { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}


// 順押し
public class JyunOshi : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "順押し";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 2, 0 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 0, 2 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[]   { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}


// 順ハサミ
public class JyunHasami : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "順ハサミ";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 0, 2 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 2, 0 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[]   { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}


// 中左
public class NakaHidari : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "中左";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 1, 1 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 2, 0 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[]   { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}

// 中右
public class NakaMigi : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "中右";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 1, 1 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 0, 2 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[] { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}

// 逆押し
public class GyakuOshi : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "逆押し";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 0, 2 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 1, 1 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[] { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}

// 逆ハサミ
public class GyakuHasami : OshijyunBase
{
    public override string GetOshijyunName()
    {
        return "逆ハサミ";
    }
    public override int[] Get_LineCheck_Num_First()
    {
        int[] checkOrder_First = new int[]   { 1, 2, 0, 0, 2 };
        return checkOrder_First;
    }
    public override int[] Get_LineCheck_Num_Secound()
    {
        int[] checkOrder_Secound = new int[] { 1, 2, 0, 2, 0 };
        return checkOrder_Secound;
    }
    public override int[] Get_LineCheck_Num_Third()
    {
        int[] checkOrder_Third = new int[] { 1, 2, 0, 0, 0 };
        return checkOrder_Third;
    }
}
