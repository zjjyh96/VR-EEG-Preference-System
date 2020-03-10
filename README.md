# VR-EEG-Preference-System - Yinhao Jiang's Master's Project Report

## Background

Virtual reality applications have been a trending topic in the past few years. Many HCI researchers are exploring the differences on the same application between VR and our normal environment. One aspect of it is VR shopping. Browsing and purchasing products in virtual reality (VR) become incrementally popular due to its immersive and realism experience, which also makes VR an ideal space for online shopping in the future.

Our research group in CSC lab is trying to investigate individual preferences when viewing products in VR.  Notably, we focus on the inﬂuences of displaying the collective preference from others on individual preferences. Besides measuring self-report preferences and behaviors, we applied a VR-embedded portable electroencephalogram (EEG) headset to acquire participants’ cognitive states associated with subjective preferences in the experiment.

My goal is to build an system for HCI research in user preference exploring with VR and EEG devices. This system should fulfill not only the basic experiment needs about display, rating and recording, but also the scalability about future modification on simliar experiments.



## Design Requirements

Our plan is to conduct a within-subject design experiment. The general idea is to show participants basic information about a product, and record their preference as baseline. Then showing them social influence information in different degrees and record their preference change.

Here are some detailed design requirements:

First, we would like the participants to rate the same product twice, but we don't want them to remember their previous choice when they rate the second time. Thus, we should design the time interval between two ratings of the same product to be as long as possible. To deal with this situation, we design the experiment into two phases. The first phase will only show basic informations without any social information. The second phase will show products with social information in different degrees.

Second, with time goes by, it would be harder for us to get participants' attention always focus on the rating. When they lose patience, the data will not be as effective as before. One thing we can do is to shuffle all the products for every participants, so that we can get the same ratio of valid data on each product. Another thing is that, we can intert a breaking period between two phases so that participants can have time to take a breathe and have some rest.

Third, there will be concern about the social influence on the previous product will get some remains and will also influence the participant's rating for the next product. Since we already shuffle all the products at the begining, we should also counterbalance the order of three conditions in second phase.



![experiment design](/Users/inhao/Downloads/experiment design.jpg)



### Data Collection

We would like to collect data from our participants both subjective and objective ways.

#### Self-report Rating

This is a subjective data, we would ask the participants to rate their preference of the products from stars 1 to 5, each time after they are displayed the product's information (with/without social influence).

#### EEG

We have concern that, although self-reporting data is widely adapted in lots of experiments, there is still a small chance that what participants' saying is not what they really thinking. That's why we need some objective measurements. And EEG is a good way to measure it, but the difficulty lies in collection and analysis.

EEG is really a sensitive signal which could be influenced easily by background noise and participants' movement. To collect EEG, we would need a calibration or fixation section everytime the participant put on the equipment. The participants would be required to stare at the center of a fixed cross on the screen for 5 seconds so we can use the signal as baseline, assuming the participants are not thinking about anything. Then during the experiment, we would require the  participants trying not to move their head and not using their hand to touch the headset.

#### Reaction Time

Reaction time measures how long does it takes for a participant to make a decision of rating. We can use it to analyze whether the participant is making this decision subconsciously, and also find the pattern about whether this part of data is valid or not. We approximate the reaction time to be the time between we show participants the rating panel and he/her make that rating decision and hit submission button.



### Experiment Procedure

For our standard experiment porcedure, we should also add some stages like Introduction, Practice Trail, etc to help participants prepared for our experiment.



![Procedure](/Users/inhao/Downloads/Procedure.jpg)



## Products Data Pre-Processing

### Products selection

The first thing we need to do is to select products from Amazon.com website. To avoid social influence from the begining, we need to avoid products with well-known brands, for example, people always heard about news about brands like apple, their opinions are already influenced by these news. Thus, the products we choose should be rarely known by people. We choose the electronic catagory because it has more products that people are unfamiliar with. The price of these selected products should be in accordance with normal distribution to simulate an online shopping environment.

Then we need to determine how much information we need to show to participants.

### Description information selection

As we all know, people's instinctive reaction made without second thought are our best chance to get preference data without any influences. Thus, we need to show them as less information as possible, yet, still sufficient for them to understand what this product is about.

Another thing we need to concern about our VE/EEG device is that, EEG signal can easily be influenced by slight movement of the headset. We have to put all the information at the center of our screen, to avoid  participants moving their heads around subconsciously while reading. This also constrains the amount of information we can show to participants each time.

Through all the materials we can get from Amazon.com, we choose to use image to give participants a general idea about what this product looks like, and a title to tell them the functions. For the image, normally Amazon.com provide the same product with images taken from different angles. We choose to use landing image, the first image on the website, which generally taken from the front of products. For the title, we also need to simplified them because it usually contains reduntant words like Model Number, which is not helpful for users to make decisions in our situation.

### Social influence information selection

As we discussed before, images and graphs can convey more high level understanding than words. We can see informations from other customers like ratings, and comments. The question is how we are going to show them to participants.

For the ratings, raw numbers normally don't convey a good understanding, and people usually get different standards for ratings. We choose to use rating distributions with both a bar chart and numbers, which specificlly tell users how many people rates on each scores from 1 to 5 stars.

For the comments, one thing we want to examing is to specificly measure how much participants are influenced by positive reviews and negative reviews separately. However, the more conditions we need to measure for a within-subject design, more time the experiment will be, more time the participant need to wear the headset, less useful data we can get. Thus, to minimize the differences of reviews between products, we choose to use the most positive and negative reviews calculated by Amazon.com. We also simplied reviews to avoid being too long for users to read.



## System Design Overview

In this section, we will discuss the how we are going to design a system that fulfill all the requirement we are talking above, and what other details we need to consider.

### Views Design

Since we can only put limited information on each view, we choose to separate the information we get from previous section into three views - Description, Distribution, and Reviews. Besides that, we would also need a toolbar indicating current progress and the time remaining for each view.

#### Toorbar

There will always be some accidents occured during the experiment, for example, sensor detachment, participant feeling uncomfortable, and all kinds of interruptions. So we need some control mechanism to help us deal with such situations. However, we won't want to give participants more control to the experiment, instead, we would need the experimenter to decide the procedure when accident occurs.

This toolbar in the user view will only showing the procedure progress, but not control it. It will show two parts of information - overall progress in the whole experiment and the remaining time on current view. It can in some degree relief the tension but also remind them about the time limitation.

#### Description

As we discussed above, we would prefer to arrange the UI horizontally and gather them in the center, to avoid users moving their head back and forth and interfering with the EEG signal. In the Description part, we put landing image at the center, just below the toorbar, and put the title at the buttom.

<img src="/Users/inhao/Documents/Description.png" alt="Description" style="zoom:67%;" />

#### Distribution

The distribution view is a bar chart showing the number of each ratings, placed at the center of the panel.

<img src="/Users/inhao/Documents/Distribution.png" alt="Distribution" style="zoom:67%;" />

#### Reviews

The review view shows the most positive and negative review titles horizontally at the center of the panel.

<img src="/Users/inhao/Documents/review.png" alt="review" style="zoom:67%;" />

#### Rating

The Rating view is an interface for participants to rate their preference, which is also the only view they can take some actions. Since we are using VR equipments, it is better for participants to use VR controller rather than mouse & keyboard. Another consideration is that, this view is the view we are measuring reaction time of participants' choices. Pressing buttons to plus/minus the rating can cause a major difference in reaction time, for example, 5 star would definately take more time than 1 star for pressing 4 more times. I chose to use the trackpad on HTC Vive controller to do the ranking. Users can slide their thumb from left to right on the trackpad to modify their ratings. And use the trigger on the controller to submit the rating.

<img src="/Users/inhao/Documents/Rating.png" alt="Rating" style="zoom:67%;" />

### Logic Design

According to the requirements above, the basic logic contains Five components:

- **LoadManager** - Loading data which we crawled from Amazon.com, and keep track of product id
- JsonLoader - the products menu is stored in json, this loader extract the information such as title, ratings, reviews, and landing image URL from json file.
  - ImageLoader - After we get the Image URL from json file, we need to load the image and render it on the pannel, which requires a http request.

- **TimeManager** - Controlling the time count down and scene switch, and also recording reaction time

  - PhaseSwitcher - When counting to 90 products, switch to next phase
  - ConditionSwitcher - When counting to 30 products in phase 2, switch to next condition
  - ViewSwitcher - When the current view time runs up, switch to next view according to current phase and condition

- **RateManager** - Get inputs from users, send rating value to Display to show stars, send value to LogManager for recording.

- **LogManager** - Keep logging the time and operation of each step, logging the current condition and view, and also user's rating value and their reaction time.

- **Display** - According to phase, condition, view information get from TimeManager, display corresponding information get from LoadManager, and RateManager.



![Logic](/Users/inhao/Downloads/Logic.png)



## Implementation

To implement this system I choose to use Unity Game engine 2019.2.10 on Windows 10 Platform. The headset is a HTC Vive with Looxid Link to it to collect EEG signal. The Looxid Link comes with Unity APIs and an application which we can use to monitor the 6-channel EEG collecting process in real-time.      

The inplementation in Unity Game Engine has two parts, one is creating and design the scene you want to display using its Canvas UI components, another is coding scripts to control the animation and logic switching.

### Canvas - UI

- **Camera**

  Camera is staring at the center of canvas. To make sure the UI is displayed in 2D model, the camera is set as Orthographic View.

- **Toolbar**

  The toolbar contains two parts: ProgressBar and CircularTimer

  - Progress bar has a darker background indicating the total progress, using a light green bar with length transformation to indicate the current progress. The transformation value is get from current trail number.
  - Circular timer has a number at the center of the progress indicating remaining time, and the circle also indicatin the remaining time with its angle.

- **InfoBox, distribution, reviews, rating Views**

  Just as describe above, but all the image and text conponents should be announced as public and allow load and update scripts to modified the content easily.

### Scripts

- **TimeManager**

  1. The TimeManager Stores Consistant Values as `Public Enums`, and also make the status public for other scripts to get.

     ```c#
     public enum Conditions { NoDisplay, SomeDisplay, FullDisplay };
     public enum Views { Description, Distribution, Review, Rating, Blank, Fixation };
     public enum ConditionOrders { Order123, Order132, Order213, Order231, Order312, Order321 };

     public Conditions condition;
     public Views view;
     public ConditionOrders order;
     ```

  2. Create a conditionMap to store the map between `ConditionOrders` and `Conditions`, for example, order123 means NoDisplay, SomeDisplay, FullDisplay order. It can help us to switch between Conditions by simply increase the index.

     ```c#
     private Dictionary<ConditionOrders, Conditions[]> conditionMap;
     ...
     conditionMap.Add(ConditionOrders.Order123, new Conditions[] { Conditions.NoDisplay, Conditions.SomeDisplay, Conditions.FullDisplay });
     ...
     ```

  3. Create a viewNameMap to map between views and its String name for Logging usage.

     ```c#
     private Dictionary<Views, string> viewNameMap;
     ...
     viewNameMap.Add(Views.Description, "Description");
     ...
     ```

  4. I use three function in the Timer Conponent to control the workflow: `StartTimer()`, `StopTimer()`, and `DidFinishedTimer()`. The most important one is `DidFinishedTimer()`, which will be called everytime time runs up. There are a few thing we need to do in it.

     - Logging current time, conditon, view, everytime time runs up.

       ```c#
       WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "View: " + viewNameMap[view] + "; Condition: " + condition.ToString() + "; Finished");
       ```

     - If current finished view is Rating, Log the product ID and its Rating value as well.

       ```c#
       if (view == Views.Rating)
       {
           int rate = rateManager.GetComponent<RatePreference>().rate;
           WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Rate for product " + trailCount.ToString() + " is " + rate.ToString());
       }
       ```

     - Increase trailCount(product ID) if Rating finished.

       ```c#
       if (view == Views.Rating) trailCount++;
       ```

     - If Phase finished, stop everyhting and switch into Breaking View

       ```c#
       isPhase1 = false;
       isBreaking = true;
       ```

     - If current condition finished, switch into next condition

       ```c#
       if (trailCount == (int)(totalTrail/3)) condition = GetCondition(order, 1);
       if (trailCount == (int)(totalTrail/3*2)) condition = GetCondition(order, 2);
       ```

     - Switch into next view

       ```c#
       view = GetNextView(condition, view);
       ```

  5. `Display(Views view)` switches the information should be displayed on the screen simply by turning on and off the visibility of each UI conponent. It also adjust the time duration for each timer on this view. For example, for the distribution view, we turn of the previous description view, get data from `jsonLoader` and set the timer into 4 seconds.

     ```c#
     if (view == Views.Distribution)
     {
         timer.duration = 3;
         jsonLoader.UpdateDistribution();
         infoBox.SetActive(false);
         distribution.SetActive(true);
     }
     ```

- **ViveInput**

  Vive Input is listening all the changes get from controllers. In this experiment, users only have two operations - pull trigger to confirm, slide the trackpad to rate.

  The value from trigger is a float value from 0 to 1. By trying different force to pull the trigger, get the feedback from users, we believe that some users will also like to pull the trigger slightly even though they are not making any decisions, which can cause small readings on the trigger. We choose 0.99f as the threshold for making a confirmation.

  The value from trackpad is a two dimensional float vector from -1 to 1. We can access the two values with `touchPadValue.x` and `touchPadValue.y`. I divide the value into 5 equal parts mapping to 5 scores.

  ```c#
  if(touchPadValue != Vector2.zero)
  {
    double value = touchPadValue.x + 1.0;
    rate.rate = (int)(value / 0.4) + 1;
  }
  ```

- **JsonLoad**

  Reading json file has many different implementations in Unity. Considering the higher fresh rate will help users to get a better experience, I read the json file as stream.

  ```c#
  path = Application.streamingAssetsPath + "/data.json";
  jsonString = File.ReadAllText(path);
  products = JsonUtility.FromJson<Products>("{\"products\":" + jsonString + "}");
  ```

  Using a serializable helps us to mapping the stream into a object we created.

  ```c#
  [System.Serializable]
  public class Products{
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
  ```

- **ImageLoad**

  In ImageLoad class, I implement ways to update and render the image from both Internet and Local path.

  The web request went through http and has to `yield` and waiting response. It would take longer time and has to handle Network Errors.

  ```c#
  IEnumerator DownloadImage(string MediaUrl)
  {   
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
    yield return request.SendWebRequest();
    if(request.isNetworkError || request.isHttpError)
    		Debug.Log(request.error);
    else
    		image.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
  }
  ```

  The local file loading is much easier and faster.

  ```c#
  public void UpdateImage(string loaclPath)
  {
      Texture2D myTexture = Resources.Load(loaclPath) as Texture2D;
      image.texture = myTexture;
  }
  ```

- **RatePreference**

  This script turns the rate value into stars on and off in the views, which is implemented with a for loop. We have to reset rating back to 0 every time rating is finished.

  ```c#
  for (int i = 0; i < rate; i++) starImages[i].enabled = true;
  for (int i = rate; i < 5; i++) starImages[i].enabled = false;
  if (!rating.activeSelf) ResetRating();
  ```

  In case that the controller is broken or the battery is dead, I also write a way to adjust rating using keyboard by pressing up and down.

  ```c#
  if (Input.GetKeyDown("up")) { rate++; if (rate > 5) rate = 5; }
  if (Input.GetKeyDown("down")) { rate--; if (rate < 1) rate = 1; }
  ```

- Logging**

  There are two different logs used in this system.

  One is the logging file we created for recording reaction time and products' rating values.

  ```c#
  [MenuItem("Tools/Write file")]
  static void WriteString(string message)
  {
    string path = "Assets/RatingLog.txt";
    StreamWriter writer = new StreamWriter(path, true);
    writer.WriteLine(message);
    writer.Close();
  }
  ```

  Another is provided by LooxidVR to record events. The reason is that, when we record EEG signals using Looxid API, the signal is recorded in it's own time system. We have to also log the events time in Looxid API to get precise signal segmentation.

  ```c#
  [Serializable]
  public class Event
  {
    public Conditions Condition;
    public Views View;
    public int ProductId;
  };
  ...
  string curEventDetail = JsonUtility.ToJson(curEvent);
  LXVRManager.Instance.RecordEvent(curEventDetail, recordEventResultCallback);
  ```



## Results

**Description View**

<img src="/Users/inhao/Desktop/re1.png" width = "500" />

**Distribution View**

<img src="/Users/inhao/Desktop/re2.png" width = "500" />

**Reviews View**

<img src="/Users/inhao/Desktop/re3.png" width = "500" />

**Rating View**

<img src="/Users/inhao/Desktop/re4.png" width = "500" />




## Testing

1. Basic Requirements
2. Change of Time on Each View, Trial Numbers
3. Change of Views, Conditions, Phases
4. VR/Non-VR Platform Switching

## Future Work

1. Experiment Design Switching (Between-subject design/Within-subject Design)

   The system could add modules to separate the controll loop from TimeManager, and make a adjustable prefab for experimenters adjust the system to be between-subject or within-subject design.

2. Remote control

   Since when participants are using the equipment, experimenters can hardly do adjustments to the system without interrupting the experiment. It is better to add a remote control client, and migrate our system into a backend server, so that we can do experiments more efficiently.
