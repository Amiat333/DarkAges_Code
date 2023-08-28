using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSword : MonoBehaviour
{


    private bool  m_canTakeDamage = true;
    private float m_damageCooldownDuration = 0.3f;
    private float m_damageCooldownTimer = 0f;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skeleton") && m_canTakeDamage)
        {
            SkeletonEnemyAI scriptEsqueleto = collision.GetComponent<SkeletonEnemyAI>();

            if (scriptEsqueleto != null)
            {
                scriptEsqueleto.TakeHit();
                m_canTakeDamage = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (!m_canTakeDamage)
        {
            m_damageCooldownTimer += Time.deltaTime;

            if (m_damageCooldownTimer >= m_damageCooldownDuration)
            {
                m_canTakeDamage = true;
                m_damageCooldownTimer = 0f;
            }
        }
    }
}

