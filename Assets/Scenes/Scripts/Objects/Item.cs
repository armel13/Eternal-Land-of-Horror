using System.Collections;
using UnityEngine;
using DG.Tweening;

// Enum yang mendefinisikan berbagai tipe item
public enum ItemType
{
    CommonKey,
    UncommonKey,
    BossKey,
    Heart,
    Money,
    Bomb,
    Stick,
    Arrow,
    MagicBottle,
    HeartContainer
}

// Kelas yang mengatur perilaku item
public class Item : MonoBehaviour
{
    // Gambar item yang ditampilkan dalam konteks item
    public Sprite contextImage;
    // Suara yang diputar saat item diambil
    public AudioClip pickupSound;
    // Kekuatan lompatan saat item diambil
    public float jumpStrength = 1;
    // Jumlah lompatan yang dapat dilakukan setelah mengambil item
    public int jumps = 1;

    // Prefab yang digunakan untuk menampilkan item saat diambil
    [Tooltip("Always 'CollectableContext' Prefab")]
    public GameObject itemDisplay;
    // Tipe item
    public ItemType type;
    // Nama item
    public string itemName;
    // Deskripsi item
    [Multiline]
    public string itemDescription;
    // Nilai item
    public int value = 1;

    // Harga item
    public int itemPrice;

    void Start()
    {
        // Harga item dapat diatur di Inspector
        // Tidak perlu menggunakan "new" untuk MonoBehaviour
    }

    // Metode yang dijalankan saat objek bersentuhan dengan item
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayItem(other));
        }
    }

    // Metode untuk menampilkan item saat diambil
    IEnumerator DisplayItem(Collider2D other)
    {
        // Menonaktifkan render dan collider item yang sedang diambil
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (boxCollider != null)
            boxCollider.enabled = false;

        if (itemDisplay != null)
        {
            // Membuat objek tampilan item yang mengikuti pemain
            GameObject display = Instantiate(
                itemDisplay,
                other.transform.position + Vector3.up * 1.5f,
                transform.rotation,
                other.transform
            );

            // Mengatur sprite tampilan item
            SpriteRenderer displayRenderer = display.GetComponent<SpriteRenderer>();
            if (displayRenderer != null)
            {
                displayRenderer.sprite = contextImage;
            }

            // Membuat objek tampilan item melompat sedikit
            display.transform.DOLocalJump(Vector3.zero, jumpStrength, jumps, 1);

            yield return new WaitForSeconds(1f);

            // Menghapus tampilan item setelah animasi selesai
            Destroy(display);
        }

        // Menambahkan item ke inventori pemain
        Collect(other.GetComponent<Inventory>());

        // Menghapus objek item dari scene
        Destroy(gameObject);
    }

    // Metode untuk mengumpulkan item dan menambahkannya ke inventori pemain
    public virtual void Collect(Inventory playerInventory)
    {
        if (playerInventory != null)
        {
            playerInventory.ReceiveItem(gameObject, value);
            Debug.Log($"Item {itemName} dengan nilai {value} telah ditambahkan ke inventori.");
        }
        else
        {
            Debug.LogWarning("Player tidak memiliki komponen Inventory!");
        }
    }
}
