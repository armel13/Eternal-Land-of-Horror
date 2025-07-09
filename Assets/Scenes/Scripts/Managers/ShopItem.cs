using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image goldIcon;
    public Image itemImage;
    private Button buyButton;
    [SerializeField] private int amountOfItem = 1;
    public Item item;  // Tambahkan akses publik ke item
    public int shopID;

    private Inventory playerInventory;

    public void Initialize(Item newItem, Inventory newPlayerInventory, Button newBuyButton)
    {
        item = newItem;
        playerInventory = newPlayerInventory;
        buyButton = newBuyButton;

        //if (nameText != null) nameText.text = item.itemName;
        if (priceText != null) priceText.text = item.itemPrice.ToString("0");

        //// Pastikan Anda memberikan referensi Image untuk goldIcon dan itemImage
        //if (goldIcon != null) goldIcon.sprite = Resources.Load<Sprite>("goldIcon");
        //if (itemImage != null) itemImage.sprite = Resources.Load<Sprite>("ItemImage");

        if (buyButton != null)
        {
            // Menambahkan fungsi untuk tombol pembelian
            buyButton.onClick.AddListener(BuyItem);

            // Menonaktifkan tombol jika item sudah dibeli
            if (ItemAlreadyBought())
            {
                buyButton.interactable = false;
            }
        }
        StartCoroutine(UpdateShopOnDelay());

    }

    IEnumerator UpdateShopOnDelay()
    {
        yield return new WaitForSeconds(0.1f);
        switch (shopID)
        {
            case 0:
                if (InfoManager.Instance.hasBoughtArrowForest)
                {
                    buyButton.interactable = false;
                }
                break;
            case 1:
                if (InfoManager.Instance.hasBoughtHealthForest)
                {
                    buyButton.interactable = false;
                }
                break;
            case 2:
                if (InfoManager.Instance.hasBoughtPotionArrowForest)
                {
                    buyButton.interactable = false;
                }
                break;
            case 3:
                if (InfoManager.Instance.hasBoughtArrowIsland)
                {
                    buyButton.interactable = false;
                }
                break;
            case 4:
                if (InfoManager.Instance.hasBoughtHealthIsland)
                {
                    buyButton.interactable = false;
                }
                break;
            case 5:
                if (InfoManager.Instance.hasBoughtPotionArrowIsland)
                {
                    buyButton.interactable = false;
                }
                break;
            case 6:
                if (InfoManager.Instance.hasBoughtPotionHealthIsland)
                {
                    buyButton.interactable = false;
                }
                break;
        }
    }

    public void SetBuyButton(Button button)
    {
        buyButton = button;
    }

    public void SetPlayerInventory(Inventory inventory)
    {
        playerInventory = inventory;
    }

    public void BuyItem()
    {
        Debug.Log("Coba membeli item...");

        if (playerInventory != null)
        {
            Debug.Log("Inventory player ditemukan!");

            if (playerInventory.money >= item.itemPrice)
            {
                Debug.Log("Uang cukup untuk membeli item!");

                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    if (InfoManager.Instance.currentMissionIndex < 4)
                    {
                        InfoManager.Instance.SetMissionIndex(4);
                    }
                }
                else if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    if (AfterShopDialogue.GetInstance() != null && AfterShopDialogue.GetInstance().gameObject.activeSelf) AfterShopDialogue.GetInstance().StartAfterShopDialogue();
                }


                // Menghapus uang dari inventory sesuai dengan harga item
                playerInventory.RemoveItem(ItemType.Money, item.itemPrice);

                // Menggunakan ReceiveItem untuk menambah item yang dibeli ke inventory
                playerInventory.ReceiveItem(item.gameObject, amountOfItem);

                // Memproses manfaat item
                ProcessItemBenefits();

                Debug.Log("Item berhasil dibeli!");

                // Menonaktifkan tombol setelah pembelian
                buyButton.interactable = false;

                switch(shopID)
                {
                    case 0:
                        InfoManager.Instance.hasBoughtArrowForest = true;
                        break;
                    case 1:
                        InfoManager.Instance.hasBoughtHealthForest = true;
                        break;
                    case 2:
                        InfoManager.Instance.hasBoughtPotionArrowForest = true;
                        break;
                    case 3:
                        InfoManager.Instance.hasBoughtArrowIsland = true;
                        break;
                    case 4:
                        InfoManager.Instance.hasBoughtHealthIsland = true;
                        break;
                    case 5:
                        InfoManager.Instance.hasBoughtPotionArrowIsland = true;
                        break;
                    case 6:
                        InfoManager.Instance.hasBoughtPotionHealthIsland = true;
                        break;
                }
            }
            else
            {
                Debug.Log("Uang tidak cukup untuk membeli item!");
            }
        }
        else
        {
            Debug.Log("Inventory player tidak ditemukan!");
        }
    }



    void ProcessItemBenefits()
    {
        switch (item.type)
        {
            case ItemType.Heart:
                FindObjectOfType<Player>().Heal(1);
                break;
                // Tambahkan case untuk jenis item lain jika diperlukan
        }
    }

    bool ItemAlreadyBought()
    {
        
        return false;
    }
}

