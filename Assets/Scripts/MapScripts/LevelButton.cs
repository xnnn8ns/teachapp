using UnityEngine;
using UnityEngine.UI;


namespace Mkey
{
    public class LevelButton : MonoBehaviour
    {
        public GameObject LeftStar;
        public GameObject MiddleStar;
        public GameObject RightStar;
        public GameObject Lock;
        public Button button;
        public Image LockState;
        public Image ActiveState;
        public Image PassState;
        public Text numberText;
        public bool Interactable { get; private set; }

        internal void SetIsActive(bool active, int activeStarsCount, bool isPassed)
        {
            Image imgButton = button.GetComponent<Image>();
            if (LeftStar) LeftStar.SetActive(activeStarsCount > 0);// && isPassed);
            if (MiddleStar) MiddleStar.SetActive(activeStarsCount > 1);// && isPassed);
            if (RightStar) RightStar.SetActive(activeStarsCount > 2);// && isPassed);
            Interactable = active;// || isPassed;
            if(button)  button.interactable = Interactable;

            if (isPassed)
            {
                imgButton.sprite = PassState.sprite;
                //Color color = button.GetComponent<Image>().color;
                //if (color != null)
                //{
                //    Debug.Log(color.a);
                //    color.a = 0.55f;
                //    button.GetComponent<Image>().color = color;
                //}

            }

            if (active)
            {
                //Debug.Log("active");
                MapController.Instance.ActiveButton = this;
                imgButton.sprite = ActiveState.sprite;
                numberText.color = Color.black;
            }
            else
            {
                numberText.color = Color.yellow;
            }


            if (!isPassed && !active)
            {
                //Lock.SetActive(!isPassed && !active);
                imgButton.sprite = LockState.sprite;

            }
            if (activeStarsCount == 0)
                numberText.text = "0%";
            else if(activeStarsCount == 1)
                numberText.text = "30%";
            else if (activeStarsCount == 2)
                numberText.text = "60%";
            else
                numberText.text = "100%";
            //button.GetComponent<Image>().sprite = imgButton.sprite;
            //if(LockText) LockText.SetActive(active || isPassed);
            numberText.gameObject.SetActive(active || isPassed);
        }
    }
}