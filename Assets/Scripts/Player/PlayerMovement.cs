using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

    protected Animator avatar;
    protected PlayerAttack playerAttack;

    float lastAttackTime; // 마지막으로 공격을 누른 시점
    float lastSkillTime; // 마지막으로 대시 공격을 누른 시점
    float lastDashTime; // 마지막으로 스킬 공격을 누른 시점
    public bool attacking = false;
    public bool dashing = false;

    void Start () {
        avatar = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
	}

    float h, v;

    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

	// Update is called once per frame
	void Update () {
        if(avatar)
        {
            avatar.SetFloat("Speed", (h * h + v * v));

            Rigidbody rigibody = GetComponent<Rigidbody>();

            if(rigibody)
            {
                if (h != 0f && v != 0f){
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }
        }
	}

    
    public void OnAttackDown()
    {
        attacking = true;
        avatar.SetBool("Combo", true);
        StartCoroutine(StartAttack());
    }

    public void OnAttackUp()
    {
        avatar.SetBool("Combo", false);
        attacking = false;
    }

    IEnumerator StartAttack()
    {
        if (Time.time - lastAttackTime > 1f){
            lastAttackTime = Time.time;
            while(attacking){
                avatar.SetTrigger("AttackStart");
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void OnSkillDown()
    {
        if(Time.time - lastSkillTime > 1f)
        {
            avatar.SetBool("Skill", true);
            lastSkillTime = Time.time;
            //playerAttack.SkillAttack();
        }
    }

    public void OnSkillUp()
    {
        avatar.SetBool("Skill", false);
    }
    

    public void OnDashDown()
    {
        if (Time.time - lastDashTime > 1f)
        {
            lastDashTime = Time.time;
            dashing = true;
            avatar.SetTrigger("Dash");
            //playerAttack.DashAttack();
        }
    }

    public void OnDashUp()
    {
        dashing = false;
    }
}
