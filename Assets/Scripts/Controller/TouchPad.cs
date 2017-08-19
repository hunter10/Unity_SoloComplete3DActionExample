using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour {

    private RectTransform _touchPad;

    // 터치 입력 중에 방향 컨트롤러의 영역 안에 있는 입력을 구분하기 위한 아이디어입니다. 
    private int _touchId = -1;

    // 입력이 시작되는 좌표입니다.
    private Vector3 _startPos = Vector3.zero;

    //방향 컨트롤러가 원으로 움직이는 반지름입니다.
    public float _dragRadius = 60f;

    // 플레이어의 움직임을 관리하는 PlayerMovement 스크립트와 연결합니다.
    // 방향키가 변경되면 캐릭터에게 신호를 보내야 하기 때문입니다.
    public PlayerMovement _player;

    // 버튼이 눌렸는지 체크하는 bool 변수입니다.
    private bool _buttonPressed = false;


	// Use this for initialization
	void Start () {
        _touchPad = GetComponent<RectTransform>();
        _startPos = _touchPad.position;
	}

    public void ButtonDown()
    {
        _buttonPressed = true;
    }

    public void ButtonUp()
    {
        _buttonPressed = false;
        HandleInput(_startPos);
    }

    private void FixedUpdate()
    {
        // 모바일에서는 터치패드 방식으로 여러 터치 입력을 받아 처리합니다.
        HandleTouchInput();

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        HandleInput(Input.mousePosition);
#endif
    }

    void HandleTouchInput()
    {

    }

    void HandleInput(Vector3 input)
    {
        // 버튼이 눌러진 상황이라면
        if(_buttonPressed)
        {
            // 방향 컨트롤러의 기준 좌표로부터 입력받은 좌표가 얼마나 떨어져 있는지 구합니다.
            Vector3 diffVector = (input - _startPos);

            // 입력 지점과 기준 좌표의 거리를 비교합니다. 만약 최대치보다 크다면,
            if(diffVector.sqrMagnitude > _dragRadius *_dragRadius)
            {
                // 방향벡터의 거리를 1로 만듭니다.
                diffVector.Normalize();

                // 그리고 방향 컨트롤러는 최대치 만큼만 움직이게 합니다.
                _touchPad.position = _startPos + diffVector * _dragRadius;
            }
            else // 입력 지점과 기준좌표가 최대치보다 크지 않다면
            {
                // 현재 입력 좌표에 방향키를 이동시킵니다.
                _touchPad.position = input;

            }
        }
        else
        {
            // 버튼에서 손이 떼어지면, 방향키를 원래 위치로 되돌려 놓습니다.
            _touchPad.position = _startPos;
        }

        // 방향키와 기준 지점의 차이를 구합니다.
        Vector3 diff = _touchPad.position - _startPos;

        // 방향키의 방향을 유지한채로, 거리를 나누어 방향만 구합니다.
        Vector2 normDiff = new Vector3(diff.x / _dragRadius, diff.y / _dragRadius);

        if(_player != null)
        {
            // 플레이어에게 변경된 좌표를 전달해줍니다.
            _player.OnStickChanged(normDiff);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
