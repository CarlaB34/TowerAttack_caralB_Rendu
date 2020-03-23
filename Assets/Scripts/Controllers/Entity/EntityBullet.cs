using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//+
public class EntityBullet : Entity
{

    public Entity target = null;
    public float speed = 1;
    
    public override void  Update()
    {
        base.Update();
        //vas vers la target
        if(target)
        {
            Vector3 direction = target.gameObject.transform.position - transform.position;
            //direction precise
            direction.Normalize();

            //va vers l'ennemi
            transform.Translate(direction * speed * Time.deltaTime);
        }
        //target n'est pas valide
        //si on est proche on détruit
        if(target && !target.IsValidEntity() && Vector3.Distance(transform.position, target.transform.position) < 0.01f)
        {
            PoolManager.Instance.PoolElement(gameObject);
        }

    }

    //set la target a la bullet
    public void SetTarget(Entity newTarget)
    {
        target = newTarget;

    }

    ///change les dégats
    public void SetDamage(int newDamageAttack)
    {
        damageAttack = newDamageAttack;
    }

    public override void RestartEntity()
    {
        target = null;
    }
    //projectil
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target.gameObject)
        {
            //applique les domage en passant par entity et utilisant la fonction damage entity.
            other.GetComponent<Entity>().DamageEntity(damageAttack);
            //détruit
            PoolManager.Instance.PoolElement(gameObject);
        }
          
    }
    
}
