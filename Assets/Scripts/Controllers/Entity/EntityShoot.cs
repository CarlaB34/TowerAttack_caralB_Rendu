using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityShoot : Entity
{
    public GameObject prefabBullet;
    public Transform pointSpawn;

    private void Shoot(Entity target)
    {
        //stocker la balle créer
        GameObject bullet = Instantiate(prefabBullet, pointSpawn.position, pointSpawn.rotation);
       
        //récupère le component
        EntityBullet entity = bullet.GetComponent<EntityBullet>();
        entity.SetDamage(damageAttack);
        entity.SetTarget(target);
    }


    protected override bool DoAttack(Entity targetEntity)
    {

        if (targetEntity.IsValidEntity() && m_CanAttack == true)
        {
            //faire spawn bullet
            Shoot(targetEntity);
            // On set les variables pour l'attente de l'attaque
            m_CanAttack = false;
            m_CurrentTimeBeforeNextAttack = 0;

            SoundManager.Instance.PlayOneShotGlobalSound();
            return true;
        }
        return false;
    
    }
}
