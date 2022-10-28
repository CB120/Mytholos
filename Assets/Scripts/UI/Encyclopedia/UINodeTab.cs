using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINodeTab : UIMenuNode
{
    public GameObject informationSector;
    [SerializeField] UIEncyclopediaManager encyclopediaManager;

    [Header("UI Object References")]
    public TextMeshProUGUI[] pageTitles;
    public TextMeshProUGUI[] descriptions;
    //public TextMeshProUGUI descriptionTwo;
    public Image[] images;
    // Sorry, i know the following isnt elegant... but im running out of time... ~ Christian
    public string leftPageNumber;
    public string rightPageNumber;
    private void OnEnable()
    {
        Invoke("lateEnable", 0.005f);
    }

    private void lateEnable() {
        
        if(images[0].sprite == null)
        {
            images[0].gameObject.SetActive(false);
            //descriptions[0].transform.localPosition = new Vector3(65, -155, 0);
            //descriptions[0].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 215);
        } else
        {
            images[0].gameObject.SetActive(true);
            //descriptions[0].transform.localPosition = new Vector3(65, -120, 0);
            //descriptions[0].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 140);
        }

        if (images[1].GetComponent<Image>().sprite == null)
        {
            images[1].gameObject.SetActive(false);
            //descriptions[1].transform.localPosition = new Vector3(330, -160, 0);
            //descriptions[1].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 190);
        }
        else
        {
            images[1].gameObject.SetActive(true);
            //descriptions[1].transform.localPosition = new Vector3(330, -100, 0);
            //descriptions[1].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 130);
        }
    }

    override public void OnAction(Action action, int playerNumber)
    {
        switch (action)
        {
            case Action.Cancel:
                encyclopediaManager.SetLibraryActive(true);
                encyclopediaManager.SetBookCanvas(false);
                UISFXManager.PlaySound("Cancel");
                break;
            default:
                break;
        }
    }
}
