using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControllerConfirm : DialogController {

    public Text LabelTitle;
    public Text LabelMessage;

    DialogDataConfirm Data
    {
        get;
        set;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        // DialogManager에 현재 이 다이얼로그 컨트롤러 클래스가 확인창을 다룬다는 사실을 등록합니다.
        DialogManager.Instance.Regist(DialogType.Confirm, this);
    }

    public override void Build(DialogData data)
    {
        base.Build(data);

        if(!(data is DialogDataConfirm))
        {
            Debug.LogError("Invalid dialog data!");
            return;
        }

        Data = data as DialogDataConfirm;
        LabelTitle.text = Data.Title;
        LabelMessage.text = Data.Message;
    }

    public void OnClickOK()
    {
        if (Data.Callback != null)
            Data.Callback(true);

        Debug.Log("OnClickOK");

        DialogManager.Instance.Pop();
    }

    public void OnClickCancel()
    {
        if (Data.Callback != null)
            Data.Callback(false);

        DialogManager.Instance.Pop();
    }
}
