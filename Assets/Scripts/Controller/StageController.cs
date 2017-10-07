using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class StageController : MonoBehaviour {

    public static StageController Instance;

    // Stage의 포인트를 관리하는 변수입니다. 몬스터를 하나 잡을때마다 포인트가 증가합니다.
    public int StagePoint = 0;

    // 화면 상단에 표시될 포인트의 Text 오브젝트를 가리키는 변수입니다.
    public Text PointText;

	// Use this for initialization
	void Start () {
        Instance = this;

        DialogDataAlert alert = new DialogDataAlert("START", "Game Start!",
                                                   delegate {
                                                       Debug.Log("OK Pressed");
                                                            }
                                                   );
        DialogManager.Instance.Push(alert);
	}

    public void AddPoint(int Point)
    {
        StagePoint += Point;
        PointText.text = StagePoint.ToString();
    }

    public void FinishGame()
    {
        Application.LoadLevel("Lobby");
    }
}
