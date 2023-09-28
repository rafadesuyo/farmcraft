using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelectionHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform iconsHolder;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private float timeToCenterTheCurrentIcon = 0.5f;

    private GameModeIcon[] gameModeIcons;

    private int currentIcon;

    public void Init()
    {
        CreateIconList();
    }

    public void SetIconOnOpen()
    {
        StartCoroutine(SetIconOnOpenCorroutine());
    }

    private void CreateIconList()
    {
        gameModeIcons = iconsHolder.GetComponentsInChildren<GameModeIcon>(true);

        for (int i = 0; i < gameModeIcons.Length; i++)
        {
            //Icon
            GameModeIcon gameModeIcon = gameModeIcons[i];
            gameModeIcon.Init(iconsHolder);

            gameModeIcon.OnIconMovedToLeft += IconMovedToLeft;
            gameModeIcon.OnIconMovedToRight += IconMovedToRight;
            gameModeIcon.OnIconEndDrag += OnIconEndDrag;

            //Move Buttons
            gameModeIcon.MoveToRightButton.gameObject.SetActive(i > 0);
            gameModeIcon.MoveToLeftButton.gameObject.SetActive(i < gameModeIcons.Length - 1);
        }
    }

    private void SetIcon(int index)
    {
        SetIcon(index, false);
    }

    private void SetIcon(int index, bool ignoreAnimation)
    {
        currentIcon = index;

        CenterCurrentWorld(ignoreAnimation);
    }

    private void CenterCurrentWorld(bool ignoreAnimation = false)
    {
        float posX = gameModeIcons[currentIcon].transform.localPosition.x;

        Vector3 currentPosition = iconsHolder.transform.localPosition;
        Vector3 newPosition = new Vector3(-posX, currentPosition.y, currentPosition.z);

        if (ignoreAnimation == false)
        {
            canvasGroup.blocksRaycasts = false;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(iconsHolder.DOLocalMove(newPosition, timeToCenterTheCurrentIcon));
            sequence.AppendCallback(() => canvasGroup.blocksRaycasts = true);
        }
        else
        {
            iconsHolder.transform.localPosition = newPosition;
        }
    }

    private void IconMovedToLeft()
    {
        currentIcon++;

        if (currentIcon > gameModeIcons.Length - 1)
        {
            currentIcon = gameModeIcons.Length - 1;
        }
    }

    private void IconMovedToRight()
    {
        currentIcon--;

        if (currentIcon < 0)
        {
            currentIcon = 0;
        }
    }

    private void OnIconEndDrag()
    {
        SetIcon(currentIcon);
    }

    private IEnumerator SetIconOnOpenCorroutine()
    {
        //Wait one frame so that the Layout Group can update the icons positions
        yield return null;

        SetIcon(0, true);
    }
}
