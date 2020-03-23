using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    //+
    public GameObject prefabToInstantiateEnemy;

    public GameObject globalTargetPlayer;
    //+
    public GameObject globalTargetEnemy;


    private Camera m_CurrentCamera;

    //spawn enemy proprieties
    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;
    private void Awake()
    {
        m_CurrentCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        InstantiateEnemy();
    }

    private void InstantiateEnemy()
    {
        // Creation d'un Ray à partir de la camera
        Ray ray = m_CurrentCamera.ScreenPointToRay(Input.mousePosition);
        float mult = 1000;
        Debug.DrawRay(ray.origin, ray.direction * mult, Color.green);

        // Recuperation du bouton droit de la souris. pour player
        if (Input.GetMouseButtonDown(0))
        {
            // 
            if (Physics.Raycast(ray, out RaycastHit hit, mult, LayerMask.GetMask("Default")))
            {
                // On recupère un élement depuis le poolmanager
                GameObject instantiated = PoolManager.Instance.GetElement(prefabToInstantiate);
                instantiated.transform.position = hit.point;
                instantiated.SetActive(true);

                Entity entity = instantiated.GetComponent<Entity>();
                if (entity)
                {
                    if (entity is EntityMoveable moveable)
                    {
                        moveable.SetGlobalTarget(globalTargetPlayer);
                    }
                    entity.RestartEntity();
                }

            }
        }

            //spawn enemy //+
            elapsedTime += Time.deltaTime;

            if (elapsedTime > secondsBetweenSpawn)
            {
                elapsedTime = 0;
                Debug.Log(true);

                Vector3 spawnPosition = new Vector3(13f, 0.5f, 10.5f);
                GameObject newEnemy = (GameObject)Instantiate(prefabToInstantiateEnemy, spawnPosition, Quaternion.Euler(0, 0, 0));
            //
                GameObject instantiated = PoolManager.Instance.GetElement(prefabToInstantiateEnemy); 
                instantiated.SetActive(true);
                //+ //fait avancer l'enemy
                Entity entityEnemy = instantiated.GetComponent<Entity>();
                if (entityEnemy is EntityMoveable moveable)
                {
                    moveable.SetGlobalTarget(globalTargetEnemy);
                }
                entityEnemy.RestartEntity();
            }
        
    }
        
}


