using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogType
{
    Alert,
    Confirm,
    Ranking
}

public class DialogManager : MonoBehaviour {

    // 유저에게 보여줄 팝업창들을 저장해 놓는 리스트입니다. 리스트에 들어온 순서대로 꺼내서 하나씩 유저에게 보여줍니다.
    List<DialogData> _dialogQueue;

    // 다이얼로그 타입에 따른 컨트롤러를 매핑한 Dictoinary변수입니다.
    // DialogType.Alert 유형은 DialogControllerAlert
    Dictionary<DialogType, DialogController> _dialogMap;

	// 현재 화면에 떠있는 다이얼로그를 지정합니다.
	DialogController _currentDialog;

    // 싱글톤 패턴으로 하나의 인스턴스를 전역적으로 공유하기 위해 instance를 여기에 생성합니다.
    private static DialogManager instance = new DialogManager();
    public static DialogManager Instance
    {
        get{
            return instance;
        }
    }

    private DialogManager()
    {
        _dialogQueue = new List<DialogData>();
        _dialogMap = new Dictionary<DialogType, DialogController>();
    }

    public void Regist(DialogType type, DialogController controller)
    {
        _dialogMap[type] = controller;
    }

    public void Push(DialogData data)
    {
        _dialogQueue.Add(data);

        if(_currentDialog == null)
        {
            ShowNext();
        }
    }

    public void Pop()
    {
        if(_currentDialog != null){
            _currentDialog.Close(
                delegate
                {
                    _currentDialog = null;
                    if (_dialogQueue.Count > 0)
                    {
                        ShowNext();
                    }
                }
            );
        }
    }

    private void ShowNext()
    {
		// 다이얼로그 리스트에서 첫 번째 멤버를 가져옵니다.
		DialogData next = _dialogQueue[0];

        // 가져온 멤버의 다이얼로그 유형을 확인합니다.
        // 그래서 그 다이얼로그 유형에 맞는 다이얼로그 컨트롤러(DialogController)를 조회합니다.
        DialogController controller = _dialogMap[next.Type].GetComponent<DialogController>();

		// 조회한 다이얼로그 컨트롤러를 현재 열린 팝업의 다이얼로그 컨트롤러로 지정합니다.
		_currentDialog = controller;

		// 현재 열린 다이얼로그 데이터를 화면에 표시합니다.
		_currentDialog.Build(next);

		// 다이얼로그를 화면에 보여주는 애니메이션을 시작합니다.
		_currentDialog.Show(delegate {});

        _dialogQueue.RemoveAt(0);
	}

    public bool IsShowing()
    {
        return _currentDialog != null;
    }
}
