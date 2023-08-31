using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class CSV_Export 
{
    // ファイルがない場合は新規作成
    // すでにファイルが存在する場合は上書きされてしまう？
    string _filePath = "/Users/satounaoyuki/Documents/KatidokiReelCSV/CSVExportFile.csv";

    int _reelKoma = 21;

    public void CSVSave(List<int> x, string flagName)
    {
        StreamWriter sw; // これがキモらしい
        FileInfo fi;

        fi = new FileInfo(_filePath);
        sw = fi.AppendText(); // 既存データに追記

        // Listの要素数を21に整理
        int[] b = ListNo21_overDelete(x);

        string exportCSV = flagName;
        string listJoin = string.Join(",", b);

        // string newLine = "LF";  // 改行コード必要な場合はこれを追加
        // exportCSV = exportCSV + "," + listJoin + "," + newLine;// 改行コード必要な場合はこれを追加

        exportCSV = exportCSV + "," + listJoin + "," ;
        sw.WriteLine(exportCSV);


        sw.Flush();
        sw.Close();
    }


    /// <summary>
    /// Listの要素数を21に切り捨て
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    int[] ListNo21_overDelete(List<int> l)
    {


        int[] c = new int[_reelKoma]; // ListからListは参照渡しになってしまう
        l.CopyTo(0, c, 0, _reelKoma);

        Array.Reverse(c);
        // l.Reverse(); // リストの中身反転

        return c;
    }
}


// CSVメモ
// 改行コードはLFの２文字（CRでも良いがLinux対応する場合はLF）
