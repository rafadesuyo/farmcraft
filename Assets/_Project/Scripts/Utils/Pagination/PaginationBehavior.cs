using System.Collections.Generic;
using UnityEngine;

public class PaginationBehavior : MonoBehaviour
{
    //Variables
    [SerializeField] private List<PaginationElement> elements = new List<PaginationElement>();
    
    private PaginationElement activeElement = null;

    //Getters
    public List<PaginationElement> Elements => elements;

    private void Awake()
    {
        SetupElements();
    }

    private void SetupElements()
    {
        foreach (PaginationElement element in elements)
        {
            element.Setup(() => OpenElement(element));
        }

        InitializePagination();
    }

    private void InitializePagination()
    {
        activeElement = elements[0];
        activeElement.Open();
    }

    private void OpenElement(PaginationElement element)
    {
        if (activeElement != element)
        {
            activeElement.Close();
            activeElement = element;
        }
    }
}
