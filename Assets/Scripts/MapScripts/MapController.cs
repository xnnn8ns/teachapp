﻿using UnityEngine;
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
        public static MapController Instance;
        private LevelButton activeButton;
        public LevelButton ActiveButton
        {
            get { return activeButton; }
            set { activeButton = value; }
        }
        [HideInInspector]
        public Canvas parentCanvas;
        private MapMaker mapMaker;
        private ScrollRect sRect;
        private RectTransform content;
        //private int biomesCount = 6;

        public static int currentLevel = 0; // set from this script by clicking on button. Use this variable to load appropriate level.
        public static int topPassedLevel = 30; // set from game MapController.topPassedLevel = 2; 

        [Header("If true, then the map will scroll to the Active Level Button", order = 1)]
        public bool scrollToActiveButton = true;

        private SoundMaster MSound => SoundMaster.Instance;
        [SerializeField] private ButtonsManager ButtonsManager;

        [SerializeField]private bool _createDataButtonsInJson = true;

        private TheoryListJSON _theoryListJSON;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
        }

        void Start()
        {
            //Debug.Log("Map controller started");
            if (mapMaker == null) mapMaker = GetComponent<MapMaker>();

            if (mapMaker == null)
            {
                Debug.LogError("No <MapMaker> component. Add <MapMaker.>");
                return;
            }

            if (mapMaker.biomes == null)
            {
                Debug.LogError("No Maps. Add Biomes to MapMaker.");
                return;
            }

            content = GetComponent<RectTransform>();
            if (!content)
            {
                Debug.LogError("No RectTransform component. Use RectTransform for MapMaker.");
                return;
            }

            FillTheoryDataFromJSON();


            List<Biome> bList = new List<Biome>(mapMaker.biomes);
            bList.RemoveAll((b) => { return b == null; });

            //if (mapMaker.mapType == MapType.Vertical) bList.Reverse();
            MapLevelButtons = new List<LevelButton>();
            //int theoryCount = 0;

            for (int i = 0; i < bList.Count; i++)
            {
                if (i >= _theoryListJSON.theoryList.Count)
                    break;
                
                bList[i].ID = _theoryListJSON.theoryList[i].ID;
                MapLevelButtons.AddRange(bList[i].levelButtons);

                bList[i].FillTitleAndSubTitle(_theoryListJSON.theoryList[i].Title, _theoryListJSON.theoryList[i].Description);
                //Debug.LogError(b.name);
                //Debug.LogError(b.count);
                //theoryCount++;
            }

            topPassedLevel = Mathf.Clamp(topPassedLevel, 0, MapLevelButtons.Count - 1);
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int scene = i + 1;
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    currentLevel = scene;
                    if (MSound) MSound.SoundPlayClick(0, null);
                    Debug.Log("load scene : " + scene);
                    ClickLevelButton(scene);
                    //ButtonsManager.SetData(scene, 1000, true, true, 3);
                });

                //SetButtonActive(scene, (currentLevel == scene || scene == topPassedLevel + 1), (topPassedLevel >= scene));
                MapLevelButtons[i].numberText.text = (scene).ToString();
            }
            parentCanvas = GetComponentInParent<Canvas>();
            sRect = GetComponentInParent<ScrollRect>();
            //if (scrollToActiveButton) StartCoroutine(SetMapPositionToAciveButton()); //this script reverse prefabs 
            ButtonsManager = GameObject.FindObjectOfType<ButtonsManager>();

            
            CreateAllButtonsInJson();
            VereficationAllButtons();
        }

        private void ClickTeoryButton(int id)
        {
            //if (MSound) MSound.SoundPlayClick(0, null);
            //ClickLevelButton(scene);
        }

        private void FillTheoryDataFromJSON()
        {
            string jsonLeaderBoardPath = "/theory_list.json";

            if (!File.Exists(Application.persistentDataPath + jsonLeaderBoardPath))
            {
                FileStream fs = File.Create(Application.persistentDataPath + jsonLeaderBoardPath);
                fs.Dispose();
                TextAsset txt = (TextAsset)Resources.Load("theory_list", typeof(TextAsset));
                string jsonTemp = txt.text;
                File.WriteAllText(Application.persistentDataPath + jsonLeaderBoardPath, jsonTemp);
            }

            string json = File.ReadAllText(Application.persistentDataPath + jsonLeaderBoardPath);
            List<TheoryListItemJSON> theoryDataList = new List<TheoryListItemJSON>();

            _theoryListJSON = JsonConvert.DeserializeObject<TheoryListJSON>(json);
            Debug.Log("FillTheoryDataFromJSON - finish");
        }

        private void CreateAllButtonsInJson() 
        {
            if (_createDataButtonsInJson)
            {
                for (int i = 0; i < mapLevelButtons.Count; i++)               
                    ButtonsManager.CreateAllButtons(i + 1);
                
            }
            _createDataButtonsInJson = false;
        }

        private void VereficationAllButtons() 
        {

            for (int i = 0; i < mapLevelButtons.Count; i++)
            {
                ButtonData buttonData = DataLoader.GetLevelData(i + 1);
                if (buttonData != null)
                {
                    SetButtonActive(buttonData.id, buttonData.isActive, buttonData.isPassed, buttonData.activeStarsCount);
                }

            }
        }

        private IEnumerator SetMapPositionToAciveButton()
        {
            yield return new WaitForSeconds(0.1f);
            if (sRect)
            {
                int bCount = mapMaker.biomes.Count;
                if (mapMaker.mapType == MapType.Vertical)
                {
                    float contentSizeY = content.sizeDelta.y / (bCount) * (bCount - 1.0f);
                    float relPos = content.InverseTransformPoint(ActiveButton.transform.position).y; // Debug.Log("contentY : " + contentSizeY +  " ;relpos : " + relPos + " : " + relPos / contentSizeY);
                    float vpos = (-contentSizeY / (bCount * 2.0f) + relPos) / contentSizeY; // 
                  //  sRect.verticalNormalizedPosition = Mathf.Clamp01(vpos); // Debug.Log("vpos : " + Mathf.Clamp01(vpos));

                    SimpleTween.Cancel(gameObject, false);
                    float start = sRect.verticalNormalizedPosition;

                    SimpleTween.Value(gameObject, start, vpos, 0.25f).SetOnUpdate((float f) => { sRect.verticalNormalizedPosition = Mathf.Clamp01(f); });

                }
                else
                {
                    float contentSizeX = content.sizeDelta.x / (bCount) * (bCount - 1.0f);
                    float relPos = content.InverseTransformPoint(ActiveButton.transform.position).x;
                    float hpos = (-contentSizeX / (bCount * 2.0f) + relPos) / contentSizeX; // 
                    //sRect.horizontalNormalizedPosition = Mathf.Clamp01(hpos);

                    SimpleTween.Cancel(gameObject, false);
                    float start = sRect.horizontalNormalizedPosition;

                    SimpleTween.Value(gameObject, start, hpos, 0.25f).SetOnUpdate((float f) => { sRect.horizontalNormalizedPosition = Mathf.Clamp01(f); });
                }
            }
            else
            {
                Debug.Log("no scrolling rect");
            }
        }

        private void SetButtonActive(int sceneNumber, bool active, bool isPassed,int _activeStarsCount)
        {         
            MapLevelButtons[sceneNumber - 1].SetActive(active, _activeStarsCount, isPassed);
        }

        public void SetControlActivity(bool activity)
        {
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                if (!activity) MapLevelButtons[i].button.interactable = activity;
                else
                {
                    MapLevelButtons[i].button.interactable = MapLevelButtons[i].Interactable;
                }
            }
        }

        private void Update_rem()
        {
            Debug.Log(content.sizeDelta.y + " : " + content.InverseTransformPoint(ActiveButton.transform.position).y);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.verticalNormalizedPosition);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.horizontalNormalizedPosition);
        }

        private void ClickLevelButton(int clickIndex)
        {
            Scene scene = SceneManager.GetSceneByName("WindowScene");
            if (scene.isLoaded)
                return;
            
            Settings.Current_Level = clickIndex;
            PlayerPrefs.SetString("SceneToLoad", "QuestionAnswerTestCheckScene");
            SceneManager.LoadScene("WindowScene", LoadSceneMode.Additive);

            //SceneManager.LoadScene("QuestionAnswerTestCheckScene", LoadSceneMode.Single);
        }
    }
}