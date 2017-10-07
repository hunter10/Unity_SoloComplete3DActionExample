using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

    // 팝업창의 Transform입니다.
    public Transform window;

    public virtual void Awake(){}

    public virtual void Start(){}

    public virtual void Build(DialogData data){}

    public bool Visible
    {
        get
        {
            return window.gameObject.activeSelf;
        }

        private set
        {
            window.gameObject.SetActive(value);
        }
    }

	IEnumerator OnEnter(Action callback)
    {
        Visible = true;

        if(callback != null)
        {
            callback();
        }

        yield break;
    }

    IEnumerator OnExit(Action callback)
    {
        Visible = false;

        if (callback != null)
        {
            callback();
        }

        yield break;
    }

    public void Show(Action callback)
    {
        StartCoroutine(OnEnter(callback));
    }

    public void Close(Action callback)
    {
        StartCoroutine(OnExit(callback));
    }
}
