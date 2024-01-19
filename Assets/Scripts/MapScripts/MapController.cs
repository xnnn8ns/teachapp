using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.IO;
using Newtonsoft.Json;
using ResponseTheoryListJSON;

namespace Mkey
{
    public class MapController : MonoBehaviour
    {
        private List<LevelButton> mapLevelButtons;
        public List<LevelButton> MapLevelButtons
        {
            get { return mapLevelButtons; }
            set { mapLevelButtons = value; }
        }
        private List<LevelButton> mapMissionButtons;
        public List<LevelButton> MapMissionButtons
        {
            get { return mapMissionButtons; }
            set { mapMissionButtons = value; }
        }
        public static MapController Instance;
        private LevelButton activeButton;
        public LevelButton ActiveButton
        {
            get { return activeButton; }
            set { activeButton = value; }
        }
        [HideInInspector]
        public Canvas parentCanvas;
        //private MapMaker mapMaker;
        private ScrollRect sRect;
        private RectTransform content;
        //private int biomesCount = 6;

        public static int currentLevel = 0; // set from this script by clicking on button. Use this variable to load appropriate level.
        //public static int topPassedLevel = 30; // set from game MapController.topPassedLevel = 2; 

        [Header("If true, then the map will scroll to the Active Level Button", order = 1)]
        public bool scrollToActiveButton = true;

        private SoundMaster MSound => SoundMaster.Instance;

        [SerializeField]
        private ButtonsManager ButtonsManager;

        [SerializeField]
        private bool _createDataButtonsInJson = true;

        private TheoryListJSON _theoryListJSON;

        List<Biome> bList = new List<Biome>();

        //[SerializeField]
        //private AudioSource _clickAudio;

        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            else
                Instance = this;  
        }

        void Start()
        {

            FillTheoryDataFromJSON();

            Biome[] list = GetComponentsInChildren<Biome>();
            bList = new List<Biome>();
            foreach (var item in list)
            {
                bList.Add(item);
            }
            //List<Biome> bList = new List<Biome>(mapMaker.biomes);
            bList.RemoveAll((b) => { return b == null; });
            //Debug.LogError(bList.Count);
            //Debug.LogError(_theoryListJSON.theoryList.Count);
            //if (mapMaker.mapType == MapType.Vertical) bList.Reverse();
            MapLevelButtons = new List<LevelButton>();
            MapMissionButtons = new List<LevelButton>();
            //int theoryCount = 0;

            for (int i = 0; i < bList.Count; i++)
            {
                if (i >= _theoryListJSON.theoryList.Count)
                    break;
                
                bList[i].ID = _theoryListJSON.theoryList[i].ID;
                MapLevelButtons.AddRange(bList[i].levelButtons);
                MapMissionButtons.AddRange(bList[i].missionButtons);

                bList[i].FillTitleAndSubTitle(_theoryListJSON.theoryList[i].Title, _theoryListJSON.theoryList[i].Description);
            }

            
            parentCanvas = GetComponentInParent<Canvas>();
            sRect = GetComponentInParent<ScrollRect>();
            if (scrollToActiveButton) StartCoroutine(SetMapPositionToAciveButton()); //this script reverse prefabs 
            ButtonsManager = GameObject.FindObjectOfType<ButtonsManager>();

            
            CreateAllButtonsInJson();
            VereficationAllButtons();

            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int id = MapLevelButtons[i].ID;
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    if (Settings.IsModalWindowOpened)
                        return;
                    if (MSound) MSound.SoundPlayClick(0, null);
                    ClickLevelButton(id);
                });
            }
            for (int i = 0; i < MapMissionButtons.Count; i++)
            {
                int id = MapMissionButtons[i].ID;
                MapMissionButtons[i].button.onClick.AddListener(() =>
                {
                    if (Settings.IsModalWindowOpened)
                        return;
                    if (MSound) MSound.SoundPlayClick(0, null);
                    ClickLevelButton(id, true);
                });
            }
            Application.targetFrameRate = 60;
            //Debug.Log(Application.targetFrameRate);
        }

        public void UpdateLang()
        {
            Debug.Log("UpdateLang");
            FillTheoryDataFromJSON();
            for (int i = 0; i < bList.Count; i++)
            {
                bList[i].FillTitleAndSubTitle(_theoryListJSON.theoryList[i].Title, _theoryListJSON.theoryList[i].Description);
            }
        }

        private void FillTheoryDataFromJSON()
        {
            string corePath = "theory_list_en";

            if (LangAsset.CurrentLangLocation == LangLocation.En)
                corePath = "theory_list_en";
            else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
                corePath = "theory_list_ru";
            else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
                corePath = "theory_list_ge";

            string jsonPath = Settings.jsonTheoryFilePath;

            Debug.Log(corePath);

            if (!File.Exists(Application.persistentDataPath + jsonPath))
            {
                FileStream fs = File.Create(Application.persistentDataPath + jsonPath);
                fs.Dispose();
                
                TextAsset txt = (TextAsset)Resources.Load("Keys/theory/" + corePath, typeof(TextAsset));
                string jsonTemp = txt.text;
                File.WriteAllText(Application.persistentDataPath + jsonPath, jsonTemp);
            }
            else
            {
                TextAsset txt = (TextAsset)Resources.Load("Keys/theory/" + corePath, typeof(TextAsset));
                string jsonTemp = txt.text;
                File.WriteAllText(Application.persistentDataPath + jsonPath, jsonTemp);
            }

            string json = File.ReadAllText(Application.persistentDataPath + jsonPath);
            //List<TheoryListItemJSON> theoryDataList = new List<TheoryListItemJSON>();

            _theoryListJSON = JsonConvert.DeserializeObject<TheoryListJSON>(json);
            Debug.Log("FillTheoryDataFromJSON - finish");
        }

        private void CreateAllButtonsInJson()
        {
            if (_createDataButtonsInJson)
                ButtonsManager.CreateAllButtons();

            _createDataButtonsInJson = false;
        }

        private void VereficationAllButtons() 
        {
            //int levelCount = 0;
            int clickIndex = 1;
            for (int i = 1; i <= MapLevelButtons.Count; i++)
            {
                ButtonData buttonData = DataLoader.GetLevelData(clickIndex);
                //Debug.Log(buttonData.id);
                if (buttonData != null)
                    SetButtonActive(buttonData.id, i - 1, buttonData.isActive, buttonData.isPassed, buttonData.activeStarsCount, buttonData.passCount, (ETypeLevel)buttonData.typeLevel);
                
                clickIndex++;
                if(i % 10 == 0)
                    clickIndex = clickIndex + 2;
            }
            //levelCount = 0;
            clickIndex = 11;
            for (int i = 1; i <= MapMissionButtons.Count; i++)
            {
                //Debug.Log(clickIndex);
                ButtonData buttonData = DataLoader.GetLevelData(clickIndex);
                if (buttonData != null)
                    SetButtonMissionActive(buttonData.id, i - 1, buttonData.isActive, buttonData.isPassed, buttonData.activeStarsCount, buttonData.passCount, (ETypeLevel)buttonData.typeLevel);
                
                clickIndex++;
                if (i % 2 == 0)
                    clickIndex = clickIndex + 10;
            }
        }

        private IEnumerator SetMapPositionToAciveButton()
        {
            yield return new WaitForSeconds(0.001f);
            if (sRect)
            {
                //if (mapMaker.mapType == MapType.Vertical)
                //{
                    Debug.Log(sRect.normalizedPosition);
                    Vector2 initialNormalizedPos = sRect.normalizedPosition;
                    Vector2 targetNormalizedPos = new Vector2(initialNormalizedPos.x, PlayerPrefs.GetFloat("ScrollYPointFocus", 1.0f));

                    sRect.normalizedPosition = targetNormalizedPos;
                //}
            }
            else
            {
                Debug.Log("no scrolling rect");
            }
        }

        private void SetButtonActive(int id, int index, bool active, bool isPassed,int _activeStarsCount, int passCount, ETypeLevel typeLevel = ETypeLevel.simple)
        {
            //Debug.Log(index.ToString()+"-"+ MapLevelButtons.Count.ToString());
            MapLevelButtons[index].SetIsActive(id, active, _activeStarsCount, passCount, isPassed, typeLevel);
        }

        private void SetButtonMissionActive(int id, int index, bool active, bool isPassed, int _activeStarsCount, int passCount, ETypeLevel typeLevel = ETypeLevel.simple)
        {
            //Debug.Log(index.ToString() + "-" + MapMissionButtons.Count.ToString());
            MapMissionButtons[index].SetIsActive(id, active, _activeStarsCount, passCount, isPassed, typeLevel);
        }

        public void SetControlActivity(bool activity)
        {
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                if (!activity)
                    MapLevelButtons[i].button.interactable = activity;
                else
                    MapLevelButtons[i].button.interactable = MapLevelButtons[i].Interactable;
            }
        }

        private void ClickLevelButton(int clickIndex, bool isMissionClicked = false)
        {
            //Debug.Log(clickIndex);
            ButtonData buttonData = DataLoader.GetLevelData(clickIndex);
            if (buttonData != null)
            {
                if (buttonData.isPassed)
                {
                    ClickPassButton(clickIndex, isMissionClicked);
                    return;
                }
                if (!buttonData.isActive)
                {
                    ClickFutureButton();
                    return;
                }
            }
            ClickCurrentLevelButton(clickIndex, isMissionClicked);


            //SceneManager.LoadScene("QuestionAnswerTestCheckScene", LoadSceneMode.Single);
        }

        private void ClickCurrentLevelButton(int clickIndex, bool isMissionClicked = false)
        {
            //_clickAudio?.Play();
            Vibration.VibratePop();
            Scene scene = SceneManager.GetSceneByName("WindowScene");
            if (scene.isLoaded)
                return;
            Debug.Log(clickIndex);
            Settings.Current_ButtonOnMapID = clickIndex;
            Settings.IsMisionClicked = isMissionClicked;
            PlayerPrefs.SetString("SceneToLoad", "QuestionAnswerTestCheckScene");

            if (LastLevel.IsLastLevelCompleted)
            {
                SceneManager.LoadScene("WindowLastScene", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("WindowScene", LoadSceneMode.Additive);
            }
        }

        private void ClickPassButton(int clickIndex, bool isMissionClicked = false)
        {
            //_clickAudio?.Play();
            Vibration.VibratePop();
            Debug.Log("ClickPassButton");
            Scene scene = SceneManager.GetSceneByName("WindowScene");
            if (scene.isLoaded)
                return;

            Settings.Current_ButtonOnMapID = clickIndex;
            Settings.IsMisionClicked = isMissionClicked;
            PlayerPrefs.SetString("SceneToLoad", "QuestionAnswerTestCheckScene");

            if (LastLevel.IsLastLevelCompleted)
            {
                SceneManager.LoadScene("WindowLastScene", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("WindowScene", LoadSceneMode.Additive);
            }
        }

        private void ClickFutureButton()
        {
            //_clickAudio?.Play();
            Debug.Log("ClickFutureButton");
            Vibration.VibratePop();
            Scene scene = SceneManager.GetSceneByName("WindowSimpliMessageScene");
            if (scene.isLoaded)
                return;
            //PlayerPrefs.SetString("MessageForWindow", "Данный уровень\nпока не доступен");
            SceneManager.LoadScene("WindowSimpliMessageScene", LoadSceneMode.Additive);
            Debug.Log("ClickFutureButton2");
            Settings.IsModalWindowOpened = true;
        }

        void OnDisable()
        {
            PlayerPrefs.SetFloat("ScrollYPointFocus", sRect.normalizedPosition.y);
            //Debug.Log("PrintOnDisable: script was disabled");
        }
    }
}