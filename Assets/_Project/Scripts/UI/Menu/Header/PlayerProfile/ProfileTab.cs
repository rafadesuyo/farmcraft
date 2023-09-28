using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileTab : MonoBehaviour
{
    //Components
    [Header("Base Components")]
    [SerializeField] protected RectTransform content;

    public void TabSelected(bool value)
    {
        content.gameObject.SetActive(value);
        
        if(value == true)
        {
            gameObject.transform.SetAsLastSibling();
        }

        //TODO: depending on the assets received, the background image of the tab may change when it's selected or not
    }

    public virtual void UpdateVariables() { }

    public virtual void ResetVariables() { }
}
