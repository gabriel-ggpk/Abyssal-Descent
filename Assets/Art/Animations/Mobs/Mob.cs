using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster: MonoBehaviour 
{
    [SerializeField] float life;
    public virtual void takeDamage()
    {

    }
    public virtual void die()
    {

    }


}

