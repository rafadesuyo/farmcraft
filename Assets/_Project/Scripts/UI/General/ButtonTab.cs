using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonTab : MonoBehaviour
{
    //Components
    [SerializeField] private Image buttonImage;

    //Events
    private UnityEvent<int> onButtonClicked = new UnityEvent<int>();

    //Variables
    [Header("Default Variables")]
    [SerializeField] private Color colorUnselected;
    [SerializeField] private Color colorSelected;

    private int index;

    //Getters
    public UnityEvent<int> OnButtonClicked => onButtonClicked;
    public int Index => index;

    public void Init(int index)
    {
        this.index = index;
    }

    public void ClickButton()
    {
        onButtonClicked?.Invoke(index);
    }

    public void ButtonSelected(bool value)
    {
        if(value == true)
        {
            buttonImage.color = colorSelected;
        }
        else
        {
            buttonImage.color = colorUnselected;
        }
    }
}
