using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

    protected Animator avatar;
    //protected PlayerAttack palyerAttack;

    float lastAttackTime, lastSkillTime, lastDashTime;
    public bool attacking = false;
    public bool dashing = false;

    void Start () {
        avatar = GetComponent<Animator>();	
	}

    float h, v;

    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
        // Debug.Log("h:" + h + ", v:" + v);
    }

	// Update is called once per frame
	void Update () {
        if(avatar)
        {
            float back = 1f;

            if (v < 0f) back = -1f;

            //if ((h * h + v * v) > 0.1f)
            {
                //Debug.Log("h^2:" + h * h + "+ v^2:" + v * v + " = " + (h * h + v * v));
                avatar.SetFloat("Speed", (h * h + v * v));
            }

            Rigidbody rigibody = GetComponent<Rigidbody>();

            if(rigibody)
            {
                Vector3 speed = rigibody.velocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rigibody.velocity = speed;
                //Debug.Log(speed);
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
}
