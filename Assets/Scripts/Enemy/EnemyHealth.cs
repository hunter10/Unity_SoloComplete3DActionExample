using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : MonoBehaviour {
        
    public int startingHealth = 100;
    public int currentHealth;

    // 적군이 타격받을 때 캐릭터의 테두리를 빨간색으로 잠시 바꾸고 사라지는 속도를 결정합니다.
    public float flashSpeed = 5f;

    // 적군이 타격받을때 캐릭터의 테두리가 변하는 색상입니다. 기본 빨간색입니다.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    // 적군이 죽으면 땅바닥으로 가라앉는데 가라앉는 속도를 정해주는 변수입니다.
    public float sinkSpeed = 1f;

    bool isDead;
    bool isSinking;
    bool damaged;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        currentHealth = startingHealth;

        isDead = false;
        damaged = false;
        isSinking = false;

        // 몬스터의 Collider를 Trigger가 아니도록 변경시킵니다.
        // Trigger가 true이면 지면이나 플레이어와 충돌하지 않습니다.
        BoxCollider collider = transform.GetComponentInChildren<BoxCollider>();
        collider.isTrigger = false;

        GetComponent<NavMeshAgent>().enabled = true;
    }

    // 데미지를 받았을 때 처리하는 함수입니다.
    public IEnumerator StartDamage(int damage, Vector3 playerPosition, float pushBack, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 공격은 죽지 않았을 때만 받습니다.
        if (!isDead)
        {
            // 가끔 MissingReferenceException 예외가 발생하는데, 예외가 발생해도 스킵하도록 예외 처리합니다.
            try
            {
                // 데미지1 : 데미지를 몬스터의 체력에 반영합니다.
                TakeDamage(damage);
                
                // 데미지2 : 몬스터를 뒤로 밀려나게 합니다. 뭔가 타격받을 때 액션성을 더해줍니다.
                PushBack(playerPosition, pushBack);
            }
            catch(MissingReferenceException e)
            {
                // 이 예외는 나와도 무시
                Debug.Log(e.ToString());
            }
        }
    }

    void PushBack(Vector3 playerPosition, float pushback)
    {
        // 주인공 캐릭터의 위치와 몬스터 위치의 차이를 벡터로 구합니다.
        Vector3 diff = playerPosition - transform.position;

        // 주인공과 몬스터 상이의 차이를 정규화시킵니다.
        diff = diff / diff.sqrMagnitude;

        // 현재 몬스터의 Rigibody에 힘을 가합니다.
        // 플레이어 반대 방향으로 밀려나는데, pushback만큼 비례해서 밀려납니다.
        GetComponent<Rigidbody>().AddForce(diff*-10000f*pushback);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    

	void Update ()
    {
        if(damaged)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", flashColour);
        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_OutlineColor", 
                Color.Lerp(transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_OutlineColor"), 
                           Color.black, 
                           flashSpeed * Time.deltaTime)
            );
        }

        damaged = false;

        if(isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
	}

    void Death()
    {
        isDead = true;

        StageController.Instance.AddPoint(10);

        BoxCollider collider = transform.GetChild(0).GetComponent<BoxCollider>();
        collider.isTrigger = true;

        StartSinking();

        GetComponent<NavMeshAgent>().enabled = false;

        isSinking = true;
    }

    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}
