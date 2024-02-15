using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject newObject;
    public GameObject soon;
    public bool toggleState;

    private void Update()
    {
        if (toggleState)
        {
            newObject.SetActive(true);
            soon.SetActive(false);
        }
        else
        {
            newObject.SetActive(false);
            soon.SetActive(true);
        }
    }
}