using UnityEngine;
using UnityEngine.UI;


namespace Mkey
{
    public class LevelButton : MonoBehaviour
    {
        public GameObject LeftStar;
        public GameObject MiddleStar;
        public GameObject RightStar;
        public GameObject LeftStarEmpty;
        public GameObject MiddleStarEmpty;
        public GameObject RightStarEmpty;
        public GameObject Lock;
        public Button button;
        public Image targetImage;
        public Image LockState;

        public Image LockStateMirror;
        public Image ActiveState;

        public Image ActiveStateMirror;
        public Image PassState;

        public Image PassStateMirror;
        public Image AdditionState;
        public Image AdditionStatePassed;
        public Image FinalState;
        public Image FinalStatePassed;
        public Text numberText;
        public bool Interactable { get; private set; }
        private ETypeLevel _typeLevel = ETypeLevel.simple;
        public bool IsNeedMissRebuild = false;
        public int ID = 0;

        public Slider slider;
        public GameObject sliderFill;

        internal void SetIsActive(int id, bool active, int activeStarsCount, int passCount, bool isPassed, ETypeLevel typeLevel = ETypeLevel.simple)
        {
            ID = id;
            numberText.text = "";
            _typeLevel = typeLevel;
            //targetImage = targetImage.GetComponent<Image>();
            if (LeftStar) LeftStar.SetActive(activeStarsCount > 0);// && isPassed);
            if (MiddleStar) MiddleStar.SetActive(activeStarsCount > 1);// && isPassed);
            if (RightStar) RightStar.SetActive(activeStarsCount > 2);// && isPassed);
            Interactable = active;// || isPassed;
            //if(button)  button.interactable = Interactable;
            if (button) button.interactable = true;
            bool isMirror = IsMirror(id);
            // Debug.Log(id);
            // Debug.Log(lastNumberFromID);
            slider?.gameObject.SetActive(false);
            if (isPassed)
            {
                if(typeLevel == ETypeLevel.final)
                    targetImage.sprite = FinalStatePassed?.sprite;
                else if (typeLevel == ETypeLevel.additional)
                    targetImage.sprite = AdditionStatePassed?.sprite;
                else {
                    if (isMirror) 
                       targetImage.sprite = PassStateMirror?.sprite; 
                    else
                        targetImage.sprite = PassState?.sprite;
                }
            }
            

            if (active)
            {
                //Debug.Log("active");
                MapController.Instance.ActiveButton = this;

                if (typeLevel == ETypeLevel.final)
                {
                    targetImage.sprite = FinalState.sprite;
                    numberText.text = "";
                }
                else if (typeLevel == ETypeLevel.additional)
                {
                    targetImage.sprite = AdditionState.sprite;
                    numberText.text = "";
                }
                else if (typeLevel == ETypeLevel.mission1
                    || typeLevel == ETypeLevel.mission2)
                {
                    //imgButton.sprite = AdditionState.sprite;
                    numberText.text = "";
                }
                else
                {
                    if (isMirror) 
                        targetImage.sprite = ActiveStateMirror.sprite;
                    else
                        targetImage.sprite = ActiveState.sprite;
                    numberText.color = Color.black;
                }
            }
            //else
            //{
            //    numberText.color = Color.yellow;
            //}


            if (!isPassed && !active)
            {
                //Lock.SetActive(!isPassed && !active);
                
                if (typeLevel == ETypeLevel.final)
                    targetImage.sprite = FinalStatePassed.sprite;
                else if (typeLevel == ETypeLevel.additional)
                    targetImage.sprite = AdditionStatePassed.sprite;
                //else if (typeLevel == ETypeLevel.mission1
                //    || typeLevel == ETypeLevel.mission2)
                    //imgButton.sprite = AdditionStatePassed.sprite;
                else if (typeLevel == ETypeLevel.simple)
                {
                    if (isMirror) 
                        targetImage.sprite = LockStateMirror.sprite;
                    else
                        targetImage.sprite = LockState.sprite;
                }

            }
            if (typeLevel == ETypeLevel.simple)
            {
                int valueSlider;
                if (passCount == 0)
                    valueSlider = 0;
                else if (passCount == 1)
                    valueSlider = 40;
                else if (passCount == 2)
                    valueSlider = 70;
                else
                    valueSlider = 100;
                if(valueSlider > 0)
                    numberText.text = valueSlider.ToString() + " %";
                slider.gameObject.SetActive(true);
                slider.value = valueSlider/100f;
                if(valueSlider > 0)
                    sliderFill.SetActive(true);
                else
                    sliderFill.SetActive(false);    
            }
            else if(typeLevel != ETypeLevel.mission1
                    && typeLevel != ETypeLevel.mission2)
            {
                numberText.text = "";
                LeftStar.SetActive(false);
                MiddleStar.SetActive(false);
                RightStar.SetActive(false);
                LeftStarEmpty.SetActive(false);
                MiddleStarEmpty.SetActive(false);
                RightStarEmpty.SetActive(false);
                RectTransform rect = targetImage.GetComponent<RectTransform>();
                Vector2 size = new Vector2(50, 50);
                if(typeLevel == ETypeLevel.final)
                {
                    size = new Vector2(100, 100);
                }
                rect.sizeDelta = size;
            }
            else if (typeLevel == ETypeLevel.mission1
                   || typeLevel == ETypeLevel.mission2)
            {
                numberText.text = "";
            }
            //button.GetComponent<Image>().sprite = imgButton.sprite;
            //if(LockText) LockText.SetActive(active || isPassed);
            numberText.gameObject.SetActive(active || isPassed);
            //numberText.gameObject.SetActive(true);
            Debug.Log("ID: " + ID + " --- numberText: " + numberText.text + " --- IsActive: " + active + " --- isPassed: " + isPassed);
        }
    
        private int GetLastNumber(int numbers)
        {
            numbers -= (Settings.GetTopicFromButtonOnMapID(numbers)-1)*2;
            char[] numbersList = ConvertIntToCharArray(numbers);
            return int.Parse(numbersList[numbersList.Length - 1].ToString());
        }

        private char[] ConvertIntToCharArray(int number)
        {
            return number.ToString().ToCharArray();
        }
        
        private bool IsMirror(int id){
            int lastNumberFromID = GetLastNumber(id);
            if(Settings.GetTopicFromButtonOnMapID(id) % 2 == 1){
                if ((lastNumberFromID <= 3 || lastNumberFromID >= 8)) 
                    return true;
                else
                    return false;
            }else {
                if ((lastNumberFromID <= 3 || lastNumberFromID >= 8)) 
                    return false;
                else
                    return true;
            }
            
        }
    }
}