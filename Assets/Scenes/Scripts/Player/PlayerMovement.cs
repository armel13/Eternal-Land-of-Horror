using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : Player
{
    public float speed = 4f; // Kecepatan pemain
    Vector2 moveDir; // Vektor arah pergerakan pemain
    [SerializeField] Animator anim; // Komponen animator

    public bool isInteracting = false;

    public VisualEffect vfxRenderer;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    protected override void Start()
    {
        base.Start(); // Memanggil metode Start dari kelas dasar (Player)

        anim = GetComponent<Animator>(); // Mengambil komponen animator pada objek pemain
        anim.SetFloat("moveX", 0f); // Mengatur parameter "moveX" pada animator
        anim.SetFloat("moveY", -1); // Mengatur parameter "moveY" pada animator
    }

    IEnumerator OnDelayStart()
    {
        yield return new WaitForSeconds(0.5F);
    }


    private void Update()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            moveDir = Vector2.zero;

            if (!isInteracting)
            {
                moveDir.x = Input.GetAxisRaw("Horizontal");
                moveDir.y = Input.GetAxisRaw("Vertical");
            }

            if (Input.GetButtonDown("Attack") && currentState != PlayerState.Stagger)
            {
                if (currentState != PlayerState.Attack && currentState != PlayerState.Interact)
                    StartCoroutine(AttackCo());
            }
            else if (currentState == PlayerState.Walk || currentState == PlayerState.Interact)
            {
                UpdateAnimation();
            }
        }

        if(vfxRenderer != null) vfxRenderer.SetVector2("PosCollider", new Vector2(transform.position.x, transform.position.y));
    }

    public void SetInteracting(bool value)
    {
        Debug.Log("SetInteracting: " + value);
        isInteracting = value;

        // Tambahkan logika untuk menonaktifkan pergerakan saat berinteraksi
        if (isInteracting)
        {
            ChangeState(PlayerState.Interact);
            rb.velocity = Vector2.zero; // Menghentikan pergerakan pemain
        }
        else
        {
            ChangeState(PlayerState.Walk);
        }
    }


    IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true); // Mengatur parameter animator "attacking" menjadi true
        ChangeState(PlayerState.Attack); // Mengubah keadaan pemain menjadi Attack
        //sounds.PlayClip(sounds.swordSwing); // Memainkan suara serangan dengan senjata
        SoundsManager.GetInstance().PlayClip(SoundsManager.Sound.PlayerSwordSwing);
        yield return null; // Menunggu satu frame
        anim.SetBool("attacking", false); // Mengatur parameter animator "attacking" menjadi false
        yield return new WaitForSeconds(.33f); // Menunggu selama 0,33 detik
        ChangeState(PlayerState.Walk); // Mengubah keadaan pemain menjadi Walk
    }

    void UpdateAnimation()
    {
        if (moveDir != Vector2.zero) // Jika pemain bergerak
        {
            MoveCharacter(); // Memindahkan karakter
            anim.SetBool("moving", true); // Mengatur parameter animator "moving" menjadi true
            anim.SetFloat("moveX", moveDir.x); // Mengatur parameter animator "moveX" sesuai dengan arah horizontal
            anim.SetFloat("moveY", moveDir.y); // Mengatur parameter animator "moveY" sesuai dengan arah vertikal
        }
        else
        {
            rb.velocity = Vector2.zero; // Menghentikan pergerakan pemain
            if(anim != null)anim.SetBool("moving", false); // Mengatur parameter animator "moving" menjadi false
        }
    }

    void MoveCharacter()
    {
        // Calculate the new position based on the movement direction and speed
        Vector2 newPosition = (Vector2)transform.position + moveDir.normalized * speed * Time.deltaTime;

        // Move the player
        rb.MovePosition(newPosition);

        // Set the vfxRenderer position to match the player's position
        if (vfxRenderer != null) vfxRenderer.SetVector2("PosCollider", new Vector2(newPosition.x, newPosition.y));


    }

    // Ketika pemain sedang menyerang dan bersentuhan dengan collider lain
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == PlayerState.Attack)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(strength, gameObject);
                //sounds.PlayClip(sounds.swordHit); // Memainkan suara ketika senjata pemain mengenai target
                SoundsManager.GetInstance().PlayClip(SoundsManager.Sound.PlayerSwordHit);
            }
        }
    }
}
