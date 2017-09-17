using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    // 주인공의 시작체력
    public int startingHealth = 100;

    // 주인공의 현재 체력입니다.
    public int currentHealth;

    // 체력 게이지 UI와 연결된 변수입니다.
    public Slider healthSlider;

    // 주인공이 데미지를 입을때 화면을 빨갛게 만들기 위한 투명한 이미지입니다.
    public Image damageImage;

    // 주인공이 데미지를 입었을 때 재생할 오디오입니다.
    public AudioClip deathClip;

    // 화면이 빨갛게 변하고나서 다시 투명한 상태로 돌아가는 속도입니다.
    public float flashSpeed = 5f;

	// 주인공이 데미지를 입었을때 화면이 변하게되는 색상입니다.
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

	// 애니메이터 컨트롤러에 매개변수를 전달하기 위해 연결한 Animator 컴포넌트
	Animator anim;

    // 플레이어 게임 오브젝트에 붙어있는 오디오 소스 컴포넌트
    // 효과을을 재생할 때 필요합니다.
    AudioSource playerAudio;

    // 플레이어의 움직임을 관리하는 PlayerMovement 
    PlayerMovement playerMovement;

    // 플레이어가 죽었는지 저장하는 플래그
    bool isDead;

    // 플레이어가 데미지를 입었는지 저장하는 플래그
    bool damaged;


	// Use this for initialization
	void Start () {
        // Player 게임 오브젝트에 붙어있는 Animator 컴포넌트를 찾아서 변수에 넣습니다.
        anim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();

        currentHealth = startingHealth;
	}

    void Update()
    {
        if(damaged)
        {
            damageImage.color = flashColor;
        }
        else{
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    // 플레이어가 공격받았을때 호출되는 함수입니다.
    public void TakeDamage(int amount)
    {
        damaged = true;

        // 공격을 받으면 amount만큼 체력을 감소시킵니다.
        currentHealth -= amount;

        // 체력게이지에 변경된 체력값을 표시합니다.
        healthSlider.value = currentHealth;

        // 만약현재 체력이 0이하가 된다면 죽었다는 함수를 호출합니다.
        if(currentHealth <= 0 && !isDead)
        {
            // 플레이어가 죽었을 때 수행할 명령이 정의된 Death()함수를 호출 합니다.
            Death();
        }
        else
        {
            // 죽은게 아니라면, 데미지를 입었다는 Trigger를 발동시킵니다.
            anim.SetTrigger("Damage");
        }
    }
	
	void Death()
    {
        // 캐릭터가 죽었다면 isDead플래그를 true로 설정합니다.
        isDead = true;

        // 애니메이션에서 Die라는 트리거를 발동시킵니다.
        anim.SetTrigger("Die");

        // 플레이어의 움직임을 관리하는 PlayerMoveMent
        playerMovement.enabled = false;
    }


}
