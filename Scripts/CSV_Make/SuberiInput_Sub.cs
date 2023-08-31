using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuberiInput_Sub : MonoBehaviour
{
   
    public Transform _ReelTransform;

    void Start()
    {
        ReelRotate(-17.1f);
    }





    public void UpButton()
    {
        ReelRotate(17.1f);
    }



    public void DownButton()
    {
        ReelRotate(-17.1f);

    }



    public void ResetButton()
    {
        _ReelTransform.transform.rotation = Quaternion.identity; // リール回転初期化
        ReelRotate(-17.1f);
    }



    /// <summary>
    /// リールを進めたり戻したり
    /// </summary>
    /// <param name="f"></param>
    void ReelRotate(float f)
    {
        _ReelTransform.transform.Rotate(new Vector3(f, 0, 0));
    }

}