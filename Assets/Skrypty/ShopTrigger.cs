using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    private bool isShopOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenShop();
        }
    }

    private void Update()
    {
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape)) 
        {
            CloseShop();
        }
    }

    private void OpenShop()
    {
        shopUI.SetActive(true); 
        isShopOpen = true;
        Time.timeScale = 0f; 
    }

    private void CloseShop()
    {
        shopUI.SetActive(false); 
        Time.timeScale = 1f; 
        isShopOpen = false;
    }
}
