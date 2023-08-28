using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class HeroKnight : MonoBehaviour
{
    [SerializeField] private float m_blockCooldownDuration = 0.2f;
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] GameObject m_damagePanel;
    [SerializeField] GameObject m_deathPanel;
    [SerializeField] PhysicsMaterial2D m_material2d;
    [SerializeField] public float m_blockPushForce = 1.3f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isAttack = false;
    private bool isDead = false;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_canReceiveDamage = true; // Bandera para permitir o bloquear el daño
    private bool m_invulnerable = false;
    private bool m_isBlocking = false;
    private bool m_isInBlockCooldown = false;
    private bool m_isBlockCooldown = false;
    private bool m_isForce = false;
    private bool isSkeletonColling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    private float m_forceCooldownDuration = 0.2f;
    private float m_forceCooldownTimer = 0f;

    public GameObject sensorEspada;
    public GameObject soundObjectAtaques;
    public GameObject soundObjectSalto;
    public GameObject soundObjectShield;
    public GameObject soundObjectBlock;
    public GameObject soundObjectRoll;
    public GameObject soundObjectHurt;
    public GameObject soundObjectDeath;
    private AudioSource ataqueAudioSource;
    private AudioSource blockAudioSource;
    private AudioSource saltoAudioSource;
    private AudioSource shieldAudioSource;
    private AudioSource rollAudioSource;
    private AudioSource hurtAudio;
    private AudioSource deathAudio;
    public int health = 2;




    public void desactivarEspada()
    {
        if (sensorEspada)
        {
            sensorEspada.SetActive(false);
        }
    }

    public void CambioEscenaIndex(int index)
    {
        SceneManager.LoadScene(index);
    }



    // Use this for initialization
    void Start()
    {
        if (m_damagePanel != null)
        {
            m_damagePanel.SetActive(false);
        }
        if (m_deathPanel != null)
        {
            m_deathPanel.SetActive(false);
        }
        ataqueAudioSource = soundObjectAtaques.GetComponent<AudioSource>();
        saltoAudioSource = soundObjectSalto.GetComponent<AudioSource>();
        shieldAudioSource = soundObjectShield.GetComponent<AudioSource>();
        blockAudioSource = soundObjectBlock.GetComponent<AudioSource>();
        rollAudioSource = soundObjectRoll.GetComponent<AudioSource>();
        hurtAudio = soundObjectHurt.GetComponent<AudioSource>();
        deathAudio = soundObjectDeath.GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    private IEnumerator ActivarDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Permitir recibir daño nuevamente
        m_canReceiveDamage = true;
    }

    private IEnumerator OcultarDamagePanel(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (m_damagePanel != null)
        {
            m_damagePanel.SetActive(false);
            health = health + 1;
            Debug.Log(health);
        }
    }

    private IEnumerator ActivarInvulnerabilidad(float duration)
    {
        m_rolling = true;
        m_invulnerable = true; // Activar estado invulnerable
        yield return new WaitForSeconds(duration);
        m_invulnerable = false; // Desactivar estado invulnerable
        m_rolling = false;
    }

    private IEnumerator DisableBlockCooldown()
    {
        yield return new WaitForSeconds(m_blockCooldownDuration);
        m_isInBlockCooldown = false;
    }

    private IEnumerator StopBlocking(float delay)
    {
        yield return new WaitForSeconds(delay);

        m_isBlocking = false;
        m_animator.SetBool("IdleBlock", false);

        // Puedes reiniciar aquí cualquier otra lógica relacionada con el bloqueo
    }

    private IEnumerator StartBlockCooldown(float duration)
    {
        m_isBlockCooldown = true; // Bloquear el uso del bloqueo
        yield return new WaitForSeconds(duration);
        m_isBlockCooldown = false; // Permitir el uso del bloqueo nuevamente
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
        {
            // Si el personaje está muerto, no ejecutar el código de movimiento y animaciones.
            return;
        }

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            this.GetComponent<Rigidbody2D>().sharedMaterial = null;
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            this.GetComponent<Rigidbody2D>().sharedMaterial = m_material2d;
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");


        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
            if (m_facingDirection == 1)
            {
                sensorEspada.transform.localPosition = new Vector3(Mathf.Abs(sensorEspada.transform.localPosition.x),
                                                                   sensorEspada.transform.localPosition.y,
                                                                   sensorEspada.transform.localPosition.z);
            }
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
            if (m_facingDirection == -1)
            {
                sensorEspada.transform.localPosition = new Vector3(Mathf.Abs(sensorEspada.transform.localPosition.x) * -1,
                                                                   sensorEspada.transform.localPosition.y,
                                                                   sensorEspada.transform.localPosition.z);
            }
        }

        // Move
        if (!m_rolling && !m_isBlocking && !m_isAttack)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (!isSkeletonColling) && (!isDead) && (m_wallSensorR1.State() && m_wallSensorR2.State()) || (!isSkeletonColling) && (!isDead) &&(m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("p") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }

        //Hurt
        else if (Input.GetKeyDown("o") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && m_grounded && !m_isBlocking)
        {
            if (sensorEspada)
            {
                sensorEspada.SetActive(true);
                Invoke("desactivarEspada", 0.26f);
            }

            m_isAttack = true;
            m_currentAttack++;

            if (m_isAttack)
            {
                m_body2d.velocity = new Vector2();
            }

            

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);
            // Reproducir el sonido de ataque
            ataqueAudioSource.PlayOneShot(ataqueAudioSource.clip);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }


        // Block
        else if (Input.GetMouseButtonDown(1) && !isSkeletonColling && !m_rolling && m_grounded && !m_isBlockCooldown)
        {
            m_animator.SetTrigger("Block");
            m_isBlocking = true;
            OnAttackAnimationEnd();
            m_animator.SetBool("IdleBlock", true);
            shieldAudioSource.PlayOneShot(shieldAudioSource.clip);
            // Dejar de bloquear después de 0.7 segundos
            StartCoroutine(StopBlocking(0.55f));
            // Iniciar el tiempo de enfriamiento del bloqueo
            StartCoroutine(StartBlockCooldown(1.0f));
        }


        // Block movement while blocking
        if (m_isBlocking)
        {
            inputX = 0;
            m_body2d.velocity = new Vector2();
        }


        // Force to block timer
        if (m_isForce)
        {
            m_forceCooldownTimer += Time.deltaTime;

            if (m_forceCooldownTimer >= m_forceCooldownDuration)
            {
                m_isForce = false;
                m_forceCooldownTimer = 0f;
            }
        }


        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isAttack && m_grounded)
        {
            m_invulnerable = true; // Activa la inmunidad antes de rodar
            StartCoroutine(ActivarInvulnerabilidad(m_rollDuration)); // Activa la invulnerabilidad durante la animación de rodar
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            rollAudioSource.PlayOneShot(rollAudioSource.clip);
        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling && !m_isAttack)
        {
            m_animator.SetTrigger("Jump");
            this.GetComponent<Rigidbody2D>().sharedMaterial = m_material2d;
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            saltoAudioSource.PlayOneShot(saltoAudioSource.clip);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Skeleton"))
        {
            isSkeletonColling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Skeleton"))
        {
            isSkeletonColling = false;
        }
    }

    public void OnAttackAnimationEnd()
    {
        m_isAttack = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skeleton"))
        {
            if (sensorEspada && !sensorEspada.activeSelf)
                // Realizar acción al entrar en contacto con el esqueleto
                // Por ejemplo, recibir daño
                recibirDano(1);
        }

        if (m_isBlocking)
        {
            Rigidbody2D skeletonRigidbody = collision.GetComponent<Rigidbody2D>();

            if (skeletonRigidbody)
            {
                if (!m_isForce)
                {
                    // Calcular dirección desde el esqueleto hacia el jugador
                    Vector2 forceDirection = skeletonRigidbody.position - m_body2d.position;

                    // Establecer el componente Y del vector a 0
                    forceDirection.y = 0;

                    forceDirection.Normalize();

                    // Aplicar fuerza hacia atrás al esqueleto (solo en el eje X)
                    skeletonRigidbody.AddForce(new Vector2(forceDirection.x, 0) * m_blockPushForce, ForceMode2D.Impulse);
                    blockAudioSource.PlayOneShot(blockAudioSource.clip);

                    m_isForce = true;
                }                
            }
        }
    }

    public void recibirDano(int damage)
    {
        if (isDead)
        {
            return; // No recibir daño si el personaje ya está muerto.
        }

        if (m_invulnerable || m_isBlocking || m_isInBlockCooldown)
            return;

        if (!m_canReceiveDamage)
            return;

        // Bloquear el daño por un tiempo específico
        m_canReceiveDamage = false;

        health = health - damage;

        m_animator.SetTrigger("Hurt");

        OnAttackAnimationEnd();

        hurtAudio.PlayOneShot(hurtAudio.clip);

        Debug.Log(health);

        if (m_damagePanel != null)
        {
            m_damagePanel.SetActive(true);
        }
        if (health <= 0)
        {
            isDead = true;

            m_animator.SetTrigger("Death");

            deathAudio.PlayOneShot(deathAudio.clip);

            this.GetComponent<Rigidbody2D>().sharedMaterial = null;

            if (m_deathPanel != null)
            {
                m_deathPanel.SetActive(true);
            }
            // Aquí puedes realizar las acciones adicionales que desees al morir el personaje, como mostrar un panel de UI, pausar el juego, etc.
            // También puedes deshabilitar los controles del personaje.
            // Ejemplo: GetComponent<HeroKnightController>().enabled = false;
        }
        // Llamada al método para activar el collider después de 2 segundos
        StartCoroutine(ActivarDamage(2f));
        StartCoroutine(OcultarDamagePanel(20f)); // Cambia el valor de 20f según la duración que desees mostrar el panel.
    }

    //public void RespawnAtCheckpoint()
    //{
    //    Vector3 respawnPos = GameManager.Instance.GetRespawnPosition();
    //    transform.position = respawnPos;
    //}
}