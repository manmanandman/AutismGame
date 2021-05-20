using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionManager : MonoBehaviour
{
    private static TransactionManager instance;
    public static TransactionManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TransactionManager>();

            if (instance == null)
                Debug.Log("There is no SceneManager");

            return instance;
        }
    }

    [HideInInspector]
    public string m_TouchText;
    [HideInInspector]
    public List<TouchSampling> touchSamp;

    private string firebaseLog;
    private Vector2 startPos;
    private float duration;
    private string tempModule;
    private float timeSamp = 0.2f;
    private float tempSamp = 0.2f;
    private float ratio = 4f / 3f;
    private bool started = false;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                firebaseLog = "Firebase OK";
                print(firebaseLog);
            }
            else
            {
                firebaseLog = "Firebase error with " + task.Result;
                print(firebaseLog);
            }
        });
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 positionByImage = new Vector2(touch.position.x - (Screen.width - Screen.height * ratio) / 2, touch.position.y);
            firebaseLog = "";
            if (positionByImage.x >= 0 && positionByImage.x <= Screen.height * ratio)
            {
                if (tempSamp <= 0)
                {
                    touchSamp.Add(new TouchSampling(positionByImage, duration));
                    tempSamp = timeSamp;
                }
                else
                {
                    tempSamp -= 1 * Time.deltaTime;
                }

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        started = true;
                        duration = 0f;
                        //tempModule = m_Background.Find(x => x.activeSelf).name;
                        positionByImage = new Vector2(touch.position.x - (Screen.width - Screen.height * ratio) / 2, touch.position.y);
                        startPos = positionByImage;
                        m_TouchText = "Touch Position : " + positionByImage + "\n";
                        break;

                    case TouchPhase.Moved:
                        if (started)
                        {
                            duration += 1 * Time.deltaTime;
                            positionByImage = new Vector2(touch.position.x - (Screen.width - Screen.height * ratio) / 2, touch.position.y);
                            m_TouchText = "Touch Position : " + positionByImage + "\n";
                        }
                        break;

                    case TouchPhase.Stationary:
                        if (started)
                        {
                            duration += 1 * Time.deltaTime;
                        }
                        break;

                    case TouchPhase.Ended:
                        if (started)
                        {
                            positionByImage = new Vector2(touch.position.x - (Screen.width - Screen.height * ratio) / 2, touch.position.y);
                            TransactionRecord(SystemInfo.deviceUniqueIdentifier, startPos, positionByImage, duration, "HOME", tempModule, touchSamp);
                            tempModule = null;
                            if (Input.touchCount > 1)
                            {
                                startPos = new Vector2(Input.GetTouch(1).position.x - (Screen.width - Screen.height * ratio) / 2, Input.GetTouch(1).position.y);
                            }
                            touchSamp.Clear();
                            tempSamp = timeSamp;
                            started = false;
                        }
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startPos = new Vector2(Input.mousePosition.x - (Screen.width - Screen.height * ratio) / 2, Input.mousePosition.y);
            if (startPos.x >= 0 && startPos.x <= Screen.height * ratio)
            {
                started = true;
                duration = 0f;
                //tempModule = m_Background.Find(x => x.activeSelf).name;
                m_TouchText = "Click Position : " + startPos + "\n";
            }
        }
        else if (Input.GetMouseButton(0) && started)
        {
            if (started)
            {
                duration += 1 * Time.deltaTime;
                Vector2 positionByImage = new Vector2(Input.mousePosition.x - (Screen.width - Screen.height * ratio) / 2, Input.mousePosition.y);
                m_TouchText = "Click Position : " + positionByImage + "\n";
                if (tempSamp <= 0)
                {
                    touchSamp.Add(new TouchSampling(positionByImage, duration));
                    tempSamp = timeSamp;
                }
                else
                {
                    tempSamp -= 1 * Time.deltaTime;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && started)
        {
            if (started)
            {
                Vector2 positionByImage = new Vector2(Input.mousePosition.x - (Screen.width - Screen.height * ratio) / 2, Input.mousePosition.y);
                TransactionRecord(SystemInfo.deviceUniqueIdentifier, startPos, positionByImage, duration, "HOME", "Temp", touchSamp);
                touchSamp.Clear();
                tempModule = null;
                tempSamp = timeSamp;
                started = false;
            }
        }
        else
        {
            m_TouchText = "Image Resolution = " + Screen.height * ratio + " x " + Screen.height + "\n" + firebaseLog;
        }
    }

    void TransactionRecord(string userID, Vector2 startPosition, Vector2 endPosition, float duration, string scene, string module, List<TouchSampling> touchSampling)
    {
        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;
        string now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        var json = new TouchTimestamps(now, startPosition, endPosition, duration, scene, module, touchSampling);
        db.Child(userID).Child(now).SetRawJsonValueAsync(JsonUtility.ToJson(json));
    }

    [System.Serializable]
    public class TouchTimestamps
    {
        public string timestamp;
        public Vector2 touchStart;
        public Vector2 touchEnd;
        public float touchDuration;
        public string scene;
        public string module;
        public TouchSampling[] touchSampling;

        public TouchTimestamps() { }

        public TouchTimestamps(string timestamp, Vector2 touchStart, Vector2 touchEnd, float touchDuration, string scene, string module, List<TouchSampling> touchSampling)
        {
            this.timestamp = timestamp;
            this.touchStart = touchStart;
            this.touchEnd = touchEnd;
            this.touchDuration = touchDuration;
            this.scene = scene;
            this.module = module;
            this.touchSampling = touchSampling.ToArray();
        }   
    }

    [System.Serializable]
    public class TouchSampling
    {
        public Vector2 position;
        public float timeFromStart;

        public TouchSampling() { }

        public TouchSampling(Vector2 position, float timeFromStart)
        {
            this.position = position;
            this.timeFromStart = timeFromStart;
        }
    }
}
