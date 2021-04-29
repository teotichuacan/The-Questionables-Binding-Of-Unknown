using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    GameManager gamemanager; // Verkn�pfung mit Gamemanager

    [Header("Georges attributes:")]
    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 5f;
    public float detonationTimer = 0f;
    public int healthRecover = 10;
    public int useHealpotions = 1;
    public int maxHealpotionsSlots = 3; //******************************** Noch einbauen im UI und Player

    [Header("Arrows:")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5.0f;
    public float fireDelay = 0.5f;
    private float lastFire;

    [Header("References:")]
    //public Animator playerAnimator;
    private Rigidbody2D rb;
    public Animator animator;
    public HealthBar healthBar;
    public GameObject Explosion;

    [Header("Statistics:")]
    public int healpotionsInInventory;
    private Vector2 movement; // zwischenspeicherung von bewegungswerten

    void Start()
    {
        // Updaten der Stats im Gamemanager
        gamemanager = FindObjectOfType<GameManager>();
        // drehen des Projektile des Charakters in Blickrichtung 
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * moveSpeed * 10;
        transform.LookAt(transform.position, this.rb.velocity);

        currentHealth = maxHealth;          // ver�ndet Wert der Healtbar
        healthBar.SetMaxHealth(maxHealth);
        // Update UI CurrentHealth, MaxHealth
        gamemanager.UpdateMaxHealthText(maxHealth);
        gamemanager.UpdateCurrentHealthText(currentHealth);
        gamemanager.UpdateMaxHealpotionsSlots(maxHealpotionsSlots);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Char Movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        /////////*********************************************** Test Rotation
        /*
        Vector3 newPosition = new Vector3(movement.x, 0.0f, movement.y);
        transform.LookAt(newPosition + transform.position);
        transform.Translate(newPosition * moveSpeed * Time.deltaTime, Space.World);
        */

        // animator einf�gen
        // animator.SetFloat("Horizontal", movement.x);
        //  animator.SetFloat("Vertical", movement.y);
        // animator.SetFloat("Speed", movement.sqrMagnitude);

        float shootHor = Input.GetAxis("ShootHorizontal");
        // transform.forward = new Vector3(shootHor, 0, 0);
        float shootVert = Input.GetAxis("ShootVertical");
        if ((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.Q) && gamemanager.healpotions > 0 && currentHealth != maxHealth) // Gamemanager fragen wie viele heiltr�nke wir haben, wemm �ber 1 = true, wenn Leben Voll = false
        {
            HealDamage(healthRecover);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        // Updaten des Healthbartextes im UI
        gamemanager.UpdateCurrentHealthText(currentHealth);
        if (currentHealth <= 0)
        {

            // Spiele Sound ab passiert in der explosion

            // Spiele Effect ab
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, Quaternion.identity);
            }

            // Zerst�re Spieler, Teleportiere ihn zur�ck zur Stadt
            // Destroy(gameObject, detonationTimer);
            Debug.Log("Du bist gestorben");
        }
    }

    public void HealDamage(int healthRecover)
    {
        currentHealth += healthRecover;

        if (currentHealth > maxHealth) // falls man mit einen Heiltrank mehr heilen sollte als man hat wird der Wert auf den Max wert gesetzt
        {
            currentHealth = maxHealth;
        }

        healthBar.SetHealth(currentHealth);
        // Updaten des Healthbartextes im UI
        gamemanager.UpdateCurrentHealthText(currentHealth);
        // Updaten der HeiltrankAnzahl im UI
        gamemanager.UseHealpotions();
        // Updaten der HeiltrankSlotsAnzahl im UI
        gamemanager.UpdateMaxHealpotionsSlots(maxHealpotionsSlots);

    }

    void Shoot (float x, float y) // Berechnen der Schussgeschwindigkeit und Spawn
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
       // bullet.AddComponent<Rigidbody2D>().gravityScale = 0; // f�gt dem Schuss einen Rigidbody hinzu, und setzt gravity auf 0
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3
        ((x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed, // if statement in einer zeile, pr�fung ob x kleiner ist als 0, falls ja setzte den Wert auf - und f�hre die Rechnung aus
        (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
        0);
    }
}