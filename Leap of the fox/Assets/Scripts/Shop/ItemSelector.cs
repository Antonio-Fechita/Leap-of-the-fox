using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] Transform upperPoint;
    [SerializeField] Transform lowerPoint;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject priceTag;
    [SerializeField] Image toggleImage;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    private int selectedItemIndex = 0;
    private List<GameObject> items = new List<GameObject>();
    private Coroutine animationCoroutine;
    public static ItemSelector instance;
    List<bool> owned;
    List<bool> active;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        owned = new List<bool>(SaveSystem.LoadShop().owned);
        active = new List<bool>(SaveSystem.LoadShop().active);

        Debug.Log("Shop scene");
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            items.Add(g.gameObject);
        }

        items.Remove(transform.gameObject);
        items = items.OrderBy(item => item.transform.position.x).ToList();

        //animationCoroutine = StartCoroutine(animateItem(items[selectedItemIndex].transform));
        changeSelectedItem();
        currencyText.text = SaveSystem.LoadGems().currentGems.ToString();
        Debug.Log("currency text: " + currencyText);
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetButtonDown("Left") || ControllerInput.instance.GetButtonDown(DirectionEnum.left)) && selectedItemIndex > 0)
        {
            selectedItemIndex--;
            changeSelectedItem();
            //SaveSystem.SaveShop(new ShopData(items.Select(go => go.GetComponent<Item>()).ToList(), items.Select(go => go.GetComponent<Item>()).ToList(), 4));
        }
        else if ((Input.GetButtonDown("Right") || ControllerInput.instance.GetButtonDown(DirectionEnum.right)) && selectedItemIndex < items.Count - 1)
        {
            selectedItemIndex++;
            changeSelectedItem();
            //SaveSystem.SaveShop(new ShopData(items.Select(go => go.GetComponent<Item>()).ToList(), items.Select(go => go.GetComponent<Item>()).ToList(), 4));
        }
        else if (Input.GetButtonDown("Escape"))
        {
            SceneManager.LoadScene("map select level", LoadSceneMode.Single);
        }
        else if (Input.GetButtonDown("Select"))
        {
            int availableGems = SaveSystem.LoadGems().currentGems;
            int price = items[selectedItemIndex].GetComponent<Item>().price;




            if (!owned[selectedItemIndex])
            {
                if (availableGems >= price)
                {
                    availableGems -= price;

                    owned[selectedItemIndex] = true;
                    active[selectedItemIndex] = true;

                    SaveSystem.SaveGems(new GemsSaveData(availableGems));
                    currencyText.text = availableGems.ToString();
                    SaveSystem.SaveShop(new ShopData(owned, active));

                    Debug.Log("item is now owned and active");
                    priceTag.SetActive(false);
                    toggleImage.sprite = active[selectedItemIndex] ? onSprite : offSprite;
                    toggleImage.transform.gameObject.SetActive(true);

                }
                else
                {
                    Debug.Log("not enough gems");
                }
            }
            else
            {
                active[selectedItemIndex] = !active[selectedItemIndex];
                SaveSystem.SaveShop(new ShopData(owned, active));
                Debug.Log("setted item to " + active[selectedItemIndex]);
                toggleImage.sprite = active[selectedItemIndex] ? onSprite : offSprite;
            }


        }

    }

    private void changeSelectedItem()
    {
        if(animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(animateItem(items[selectedItemIndex].transform));
        nameText.text = items[selectedItemIndex].GetComponent<Item>().name;
        descriptionText.text = items[selectedItemIndex].GetComponent<Item>().description;
        priceText.text = items[selectedItemIndex].GetComponent<Item>().price.ToString();

        if (owned[selectedItemIndex])
        {
            priceTag.SetActive(false);
            toggleImage.sprite = active[selectedItemIndex] ? onSprite : offSprite;
            toggleImage.transform.gameObject.SetActive(true);
        }

        else
        {
            priceTag.SetActive(true);
            toggleImage.transform.gameObject.SetActive(false);
        }

    }

    IEnumerator animateItem(Transform item)
    {
        float t = 0;
        bool goingUp = true;
        float timeElapsed = Time.deltaTime;
        while (true)
        {
            item.position = new Vector3(item.position.x, Mathf.Lerp(lowerPoint.position.y, upperPoint.position.y, t), item.position.z);
            //item.position = new Vector3(item.position.x, Mathf.Lerp(lowerPoint.position.y, upperPoint.position.y, timeElapsed / duration), item.position.z);
            if ((goingUp && t > 1) || (!goingUp && t < 0))
            {
                goingUp = !goingUp;
            }

            t = (goingUp) ? t + 0.05f : t - 0.05f;
            yield return new WaitForSeconds(0.03f);

            timeElapsed += Time.deltaTime;
            
        }
    }
}
