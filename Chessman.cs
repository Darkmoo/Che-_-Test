using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chessman : MonoBehaviour 
{
	public int CurrentX{ set;get;}
	public int CurrentY{ set;get;}
	public bool isWhite;
	public bool deathMark;

    [SerializeField] private GameObject _modelContainer;
    [SerializeField] private GameObject _modelDeathMark;

    public int speed = 15;

    private Vector3 targetPos;
    private Chessman enemyChessman;

    //визуальные эффекты
    [SerializeField] private GameObject _weapon;

    private bool isMoves = false;
    private bool isAttacks = false;


    void Awake()
    {
        targetPos = transform.position;
    }

    private void Update()
    {
        //Debug.Log("Name - > "+ transform.name + " X "+CurrentX +" Y " + CurrentY );
        if(_modelDeathMark != null)
            _modelDeathMark.GetComponent<MeshRenderer>().enabled = deathMark;
        if (deathMark && _modelDeathMark != null)
        {
            _modelDeathMark.GetComponent<Animation>().Play();
        }

        if (!OnTarget())
        {
            isMoves = true;
            Move();
        }
        else
        {
            isMoves = false;
            isAttacks = false;
            if (enemyChessman != null)
            {
                enemyChessman.Die();
                enemyChessman = null;
                if (_weapon != null)
                    _weapon.GetComponentInChildren<ParticleSystem>().enableEmission = false;
            }
        }
        Animate();

    }

    private bool OnTarget()
    {
        bool checkX = Mathf.Round(targetPos.x) == Mathf.Round(transform.position.x);
        bool checkZ = Mathf.Round(targetPos.z) == Mathf.Round(transform.position.z);
        return checkX && checkZ;
    }
    
	public void SetPosition(int x, int y, Chessman enemy = null)
	{
		CurrentX = x;
		CurrentY = y;
        targetPos = BoardControl.Instance.GetTileCenter(x, y);
        targetPos.y = 1.2f;
        if (enemy != null)
        {
            enemyChessman = enemy;
            isMoves = false;
            isAttacks = true;
            if(_weapon !=null)
                _weapon.GetComponentInChildren<ParticleSystem>().enableEmission = true;
        }
        //transform.position = newPos;
    }

    void Move()
    {
        Vector3 dir = targetPos - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (isAttacks)
            speed = 10;
        else
            speed = 15;
    }

    public void Die()
    {
        Destroy(transform.gameObject);
    }

    private void Animate()
    {
        //анимируем оружие
        if (_weapon != null)
        {
            Animator melee_weapon_anim = _weapon.GetComponent<Animator>();

            melee_weapon_anim.SetBool("isMoves", isMoves);
            melee_weapon_anim.SetBool("isAttack", isAttacks);
        }
    }

    public virtual bool[,] PossibleMove()
	{
		return new bool[8,8];
	}

}
