using System.Collections; 
using System.Collections.Generic;
using UnityEngine; 
using System.IO;
using UnityEngine.UI;

public class JsonLoad : MonoBehaviour {

    private TimeManager timeManager;
    private Products products;
    private ImageLoad imageLoad;

    public Text title, positive, critical;
    public Text[] percentages;
    public GameObject distribution;
    private Slider[] sliders;

    string path;
    string jsonString;
    int index;

    void Start () {
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        imageLoad = GameObject.Find("LoadManager").GetComponent<ImageLoad>();

        sliders = distribution.GetComponentsInChildren<Slider>();

        path = Application.streamingAssetsPath + "/data.json";
        jsonString = File.ReadAllText(path);
        products = JsonUtility.FromJson<Products>("{\"products\":" + jsonString + "}");

        print(products.products[index].image_urls[0]);
        imageLoad.url = products.products[index].image_urls[0];
    }

    public void UpdateDistribution()
    {
        index = timeManager.trailCount;
        sliders[0].value = products.products[index].five_stars;
        sliders[1].value = products.products[index].four_stars;
        sliders[2].value = products.products[index].three_stars;
        sliders[3].value = products.products[index].two_stars;
        sliders[4].value = products.products[index].one_stars;

        for (int i = 0; i < 5; i++)
        {
            percentages[i].text = sliders[i].value.ToString() + "%";
        }
    }

    public void UpdateTitle()
    {
        index = timeManager.trailCount;
        title.text = products.products[index].title;
    }

    public void UpdateReviews()
    {
        index = timeManager.trailCount;
        positive.text = products.products[index].positive_review;
        critical.text = products.products[index].negative_review;
    }

    public void UpdateImage()
    {
        index = timeManager.trailCount;
        //Debug.Log(index);
        //imageLoad.url = products.products[index].image_urls[0];
        imageLoad.url = "images/image-" + (index + 1).ToString();
        Debug.Log("It's time to Update Image URL to " + imageLoad.url);
        imageLoad.UpdateImage(imageLoad.url);
    }

} 
[System.Serializable]
public class Products
{
    public Product[] products;
}

[System.Serializable] 
public class Product{
    public string title;
    public string price;
    public int five_stars;
    public int four_stars;
    public int three_stars;
    public int two_stars;
    public int one_stars;
    public string[] image_urls;
    public string positive_review;
    public string negative_review;
    
}