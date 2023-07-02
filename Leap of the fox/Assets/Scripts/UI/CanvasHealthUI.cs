using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> hearts = new List<GameObject>();
    public Vector2 firstHeartPosition;
    public Sprite fullHeart, halfHeart, emptyHeart;
    public int heartsSize;
    [Range (-1000,1000)]
    public int spaceBetweenHearts;
    private int currentDisplayedHealth;

    void Start()
    {

        for (int heartIndex = 0; heartIndex < PlayerHealthController.instance.maxHealth / 2; heartIndex++)
        {
            hearts.Add(new GameObject()); //Create the GameObject
            Image NewImage = hearts[heartIndex].AddComponent<Image>(); //Add the Image Component script
            NewImage.sprite = fullHeart; //Set the Sprite of the Image Component on the new GameObject
            hearts[heartIndex].GetComponent<RectTransform>().SetParent(transform);

            hearts[heartIndex].GetComponent<RectTransform>().SetAsFirstSibling();
        }

        if (PlayerHealthController.instance.maxHealth % 2 != 0)
        {
            hearts.Add(new GameObject()); //Create the GameObject
            Image NewImage = hearts[hearts.Count - 1].AddComponent<Image>(); //Add the Image Component script
            NewImage.sprite = halfHeart; //Set the Sprite of the Image Component on the new GameObject
            hearts[hearts.Count - 1].GetComponent<RectTransform>().SetParent(transform);

            hearts[hearts.Count - 1].GetComponent<RectTransform>().SetAsFirstSibling();
        }

        for(int heartIndex = 0; heartIndex < hearts.Count; heartIndex++)
        {
            hearts[heartIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(heartsSize, heartsSize);

            hearts[heartIndex].GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            hearts[heartIndex].GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            hearts[heartIndex].SetActive(true); //Activate the GameObject
            hearts[heartIndex].GetComponent<RectTransform>().anchoredPosition = firstHeartPosition + new Vector2(heartIndex * (hearts[0].GetComponent<RectTransform>().rect.width * (1 + spaceBetweenHearts/100f)), 0);
        }

        currentDisplayedHealth = PlayerHealthController.instance.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDisplayedHealth != PlayerHealthController.instance.currentHealth)
        {
            updateHealth(PlayerHealthController.instance.currentHealth, PlayerHealthController.instance.maxHealth);
        }
    }

    private void updateHealth(int currentActualHealth, int maxHealth)
    {
        int heartIndex;
        for (heartIndex = 0; heartIndex < currentActualHealth / 2; heartIndex++)
        {
            hearts[heartIndex].GetComponent<Image>().sprite = fullHeart;
        }

        if (currentActualHealth % 2 == 1)
        {
            hearts[heartIndex].GetComponent<Image>().sprite = halfHeart;
            heartIndex++;
        }

        while(heartIndex < hearts.Count)
        {
            hearts[heartIndex].GetComponent<Image>().sprite = emptyHeart;
            heartIndex++;
        }
        currentDisplayedHealth = currentActualHealth;
    }
}
