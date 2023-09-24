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
                Destroy(gameObject);
            else
                Instance = this;  
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
                //Debug.LogError(_theoryListJSON.theoryList[i].Title);
                //Debug.LogError(bList[i].count);
                //theoryCount++;
                //FillBiomeMissions(bList[i]);
            }

            topPassedLevel = Mathf.Clamp(topPassedLevel, 0, MapLevelButtons.Count - 1);
            int levelCount = 1;
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int clickIndex = i + 1;
                //Debug.Log("i : " + MapLevelButtons[i].name);
                MapLevelButtons[i].button.onClick.AddListener(() =>
                {
                    currentLevel = clickIndex;
                    if (MSound) MSound.SoundPlayClick(0, null);
                    Debug.Log("clickIndex : " + clickIndex);
                    ClickLevelButton(clickIndex);
                    //ButtonsManager.SetData(scene, 1000, true, true, 3);
                });
                levelCount++;
                if (levelCount >= 10)
                {
                    levelCount = 1;
                    i += 2;
                }
            }
            levelCount = 0;
            for (int i = 0; i < MapMissionButtons.Count; i++)
            {
                int clickIndex = i + 1 + 10;
                MapMissionButtons[i].button.onClick.AddListener(() =>
                {
                    currentLevel = clickIndex;
                    if (MSound) MSound.SoundPlayClick(0, null);
                    Debug.Log("clickIndex : " + clickIndex);
                    ClickLevelButton(clickIndex,true);
                    //ButtonsManager.SetData(scene, 1000, true, true, 3);
                });
                levelCount++;
                if (levelCount >= 2)
                {
                    levelCount = 1;
                    i += 10;
                }
            }
            parentCanvas = GetComponentInParent<Canvas>();
            sRect = GetComponentInParent<ScrollRect>();
            if (scrollToActiveButton) StartCoroutine(SetMapPositionToAciveButton()); //this script reverse prefabs 
            ButtonsManager = GameObject.FindObjectOfType<ButtonsManager>();

            
            CreateAllButtonsInJson();
            VereficationAllButtons();
        }

        //private void FillBiomeMissions(Biome biome)
        //{
        //    //biome.FillMissions();
        //    biome.missionButtons[0].SetIsActive(true, 2, false, ETypeLevel.mission1);
        //    biome.missionButtons[1].SetIsActive(true, 0, false, ETypeLevel.mission2);
        //}

        private void ClickTheoryButton(int id)
        {
            //if (MSound) MSound.SoundPlayClick(0, null);
            //ClickLevelButton(scene);
        }

        private void FillTheoryDataFromJSON()
        {
            string jsonPath = "/theory_list.json";

            if (!File.Exists(Application.persistentDataPath + jsonPath))
            {
                FileStream fs = File.Create(Application.persistentDataPath + jsonPath);
                fs.Dispose();
                TextAsset txt = (TextAsset)Resources.Load("theory_list", typeof(TextAsset));
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
            {
                //for (int i = 0; i < mapLevelButtons.Count; i++)               
                    ButtonsManager.CreateAllButtons();
                
            }
            _createDataButtonsInJson = false;
        }

        private void VereficationAllButtons() 
        {
            int levelCount = 0;
            for (int i = 0; i < MapLevelButtons.Count; i++)
            {
                int clickIndex = i + 1;
                ButtonData buttonData = DataLoader.GetLevelData(clickIndex);
                if (buttonData != null)
                {
                    //Debug.Log(i);
                    ETypeLevel typeLevel = ETypeLevel.simple;
                    if(buttonData.typeLevel == (int)ETypeLevel.additional)
                        typeLevel = ETypeLevel.additional;
                    if(buttonData.typeLevel == (int)ETypeLevel.final)
                        typeLevel = ETypeLevel.final;
                    SetButtonActive(i, buttonData.isActive, buttonData.isPassed, buttonData.activeStarsCount, typeLevel);
                }
                levelCount++;
                if (levelCount >= 10)
                {
                    levelCount = 1;
                    i += 2;
                }
            }
            levelCount = 0;
            for (int i = 0; i <= MapMissionButtons.Count; i++)
            {
                int clickIndex = i + 1 + 10;
                ButtonData buttonData = DataLoader.GetLevelData(clickIndex);
                if (buttonData != null)
                {
                    //Debug.Log(i);
                    ETypeLevel typeLevel = ETypeLevel.simple;
                    if (buttonData.typeLevel == (int)ETypeLevel.additional)
                        typeLevel = ETypeLevel.additional;
                    if (buttonData.typeLevel == (int)ETypeLevel.final)
                        typeLevel = ETypeLevel.final;
                    if (buttonData.typeLevel == (int)ETypeLevel.mission1)
                        typeLevel = ETypeLevel.mission1;
                    if (buttonData.typeLevel == (int)ETypeLevel.mission2)
                        typeLevel = ETypeLevel.mission2;
                    SetButtonMissionActive(i, buttonData.isActive, buttonData.isPassed, buttonData.activeStarsCount, typeLevel);
                }
                levelCount++;
                if (levelCount >= 2)
                {
                    levelCount = 1;
                    i += 10;
                }
            }
        }

        private IEnumerator SetMapPositionToAciveButton()
        {
            yield return new WaitForSeconds(0.001f);
            if (sRect)
            {
                int bCount = mapMaker.biomes.Count;
                if (mapMaker.mapType == MapType.Vertical)
                {
                    ////Debug.Log(ActiveButton.transform.position);
                    //float contentSizeY = content.sizeDelta.y / (bCount) * (bCount - 1.0f);
                    //float relPos = content.InverseTransformPoint(ActiveButton.transform.position).y; // Debug.Log("contentY : " + contentSizeY +  " ;relpos : " + relPos + " : " + relPos / contentSizeY);
                    //float vpos = (-contentSizeY / (bCount * 2.0f) + relPos) / contentSizeY; // 
                    //                                                                        //  sRect.verticalNormalizedPosition = Mathf.Clamp01(vpos); // Debug.Log("vpos : " + Mathf.Clamp01(vpos));

                    //SimpleTween.Cancel(gameObject, false);
                    //float start = sRect.verticalNormalizedPosition;

                    //SimpleTween.Value(gameObject, start, vpos, 0.25f).SetOnUpdate((float f) => { sRect.verticalNormalizedPosition = Mathf.Clamp01(f); });
                    Debug.Log(sRect.normalizedPosition);
                    Vector2 initialNormalizedPos = sRect.normalizedPosition;
                    Vector2 targetNormalizedPos = new Vector2(initialNormalizedPos.x, PlayerPrefs.GetFloat("ScrollYPointFocus", 1.0f));
                    //float speed = 5f;

                    //float t = 0f;
                    //while (t < 2f)
                    //{
                        //sRect.normalizedPosition = Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos, 1f - (1f - t) * (1f - t));
                    //    Debug.Log(t);
                    //    yield return null;
                    //    t += speed * Time.unscaledDeltaTime;
                    //}

                    sRect.normalizedPosition = targetNormalizedPos;
                    //Debug.Log(sRect.normalizedPosition);
                }
                else
                {
                    //float contentSizeX = content.sizeDelta.x / (bCount) * (bCount - 1.0f);
                    //float relPos = content.InverseTransformPoint(ActiveButton.transform.position).x;
                    //float hpos = (-contentSizeX / (bCount * 2.0f) + relPos) / contentSizeX; // 
                    ////sRect.horizontalNormalizedPosition = Mathf.Clamp01(hpos);

                    //SimpleTween.Cancel(gameObject, false);
                    //float start = sRect.horizontalNormalizedPosition;

                    //SimpleTween.Value(gameObject, start, hpos, 0.25f).SetOnUpdate((float f) => { sRect.horizontalNormalizedPosition = Mathf.Clamp01(f); });
                }
            }
            else
            {
                Debug.Log("no scrolling rect");
            }
        }

        private void SetButtonActive(int index, bool active, bool isPassed,int _activeStarsCount, ETypeLevel typeLevel = ETypeLevel.simple)
        {         
            MapLevelButtons[index].SetIsActive(active, _activeStarsCount, isPassed, typeLevel);
        }

        private void SetButtonMissionActive(int index, bool active, bool isPassed, int _activeStarsCount, ETypeLevel typeLevel = ETypeLevel.simple)
        {
            MapMissionButtons[index].SetIsActive(active, _activeStarsCount, isPassed, typeLevel);
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

        private void Update_rem()
        {
            Debug.Log(content.sizeDelta.y + " : " + content.InverseTransformPoint(ActiveButton.transform.position).y);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.verticalNormalizedPosition);
            Debug.Log("sRect.verticalNormalizedPosition: " + sRect.horizontalNormalizedPosition);
        }

        private void ClickLevelButton(int clickIndex, bool isMissionClicked = false)
        {
            Scene scene = SceneManager.GetSceneByName("WindowScene");
            if (scene.isLoaded)
                return;
            
            Settings.Current_Level = clickIndex;
            Settings.IsMisionClicked = isMissionClicked;
            PlayerPrefs.SetString("SceneToLoad", "QuestionAnswerTestCheckScene");
            
            SceneManager.LoadScene("WindowScene", LoadSceneMode.Additive);

            //SceneManager.LoadScene("QuestionAnswerTestCheckScene", LoadSceneMode.Single);
        }

        void OnDisable()
        {
            PlayerPrefs.SetFloat("ScrollYPointFocus", sRect.normalizedPosition.y);
            //Debug.Log("PrintOnDisable: script was disabled");
        }
    }
}