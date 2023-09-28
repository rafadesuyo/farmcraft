using UnityEngine;

public abstract class UIController : MonoBehaviour
{
    //Components
    [Header("Base Components")]

    [Tooltip("The content of the UI.\nIt needs to have a CanvasGroup.")]
    [SerializeField] protected RectTransform content;

    [Tooltip("A background covering the whole screen behind the content, to block the user from clicking in things behind it.\nIt needs to have a CanvasGroup.")]
    [SerializeField] protected RectTransform blockerBackground;

    protected CanvasGroup contentCanvasGroup;
    protected CanvasGroup blockerBackgroundCanvasGroup;

    //Variables
    [Header("Options")]
    [Tooltip("Activates the Blocker Background when the UI is opened.\nIt prevents the user of clicking in things behind the UI.")]
    [SerializeField] protected bool activateBlockerBackground = true;

    [Space(10)]

    [SerializeField] protected bool playAnimation = true;

    protected bool isOpen = false;

    protected bool animationIsPlaying = false;

    protected Vector2 initialAnchoredPosition;
    protected Vector3 initialLocalScale;

    //Getters
    public bool IsOpen => isOpen;

    protected void Awake()
    {
        //Components
        contentCanvasGroup = content.GetComponent<CanvasGroup>();
        blockerBackgroundCanvasGroup = blockerBackground.GetComponent<CanvasGroup>();

        if(contentCanvasGroup == null)
        {
            throw new System.Exception("The Content RectTransform doesn't have a CanvasGroup!");
        }

        if (blockerBackgroundCanvasGroup == null)
        {
            throw new System.Exception("The Blocker Background RectTransform doesn't have a CanvasGroup!");
        }

        //Variables
        initialAnchoredPosition = content.anchoredPosition;
        initialLocalScale = content.localScale;

        OnAwake();

        //Makes the UI start closed
        ResetUIVariables();
    }

    #region Events

    protected virtual void OnAwake() { }
    protected virtual void OnOpen() { }
    protected virtual void OnAfterOpenAnimation() { }
    protected virtual void OnClose() { }
    protected virtual void OnBeforeCloseAnimation() { }

    #endregion

    #region Animation

    /// <summary>
    /// The method to play the open animation.
    /// <br>The animation must start with <see cref="OnAnimationStarted"/> and finish with <see cref="OnOpenAnimationEnded"/>.</br>
    /// </summary>
    protected abstract void PlayOpenAnimation();

    /// <summary>
    /// The method to play the close animation.
    /// <br>The animation must start with <see cref="OnAnimationStarted"/> and finish with <see cref="OnCloseAnimationEnded"/>.</br>
    /// </summary>
    protected abstract void PlayCloseAnimation();

    protected void AnimationStarted()
    {
        animationIsPlaying = true;
        contentCanvasGroup.blocksRaycasts = false;
    }

    protected void AnimationEnded()
    {
        animationIsPlaying = false;
        contentCanvasGroup.blocksRaycasts = true;
    }

    protected void OnAnimationStarted()
    {
        AnimationStarted();
    }

    protected void OnOpenAnimationEnded()
    {
        OnAfterOpenAnimation();
        AnimationEnded();
    }

    protected void OnCloseAnimationEnded()
    {
        OnClose();
        ResetUIVariables();
        AnimationEnded();
    }

    #endregion

    public void OpenOrCloseUI()
    {
        if(isOpen == true)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    public void OpenUI()
    {
        if (isOpen == true || animationIsPlaying == true)
        {
            return;
        }

        isOpen = true;

        ResetUIVisuals();
        
        content.gameObject.SetActive(true);
        blockerBackground.gameObject.SetActive(activateBlockerBackground);

        OnOpen();

        if(playAnimation == false)
        {
            OnAfterOpenAnimation();
        }
        else
        {
            PlayOpenAnimation();
        }
    }

    public void CloseUI()
    {
        if(isOpen == false || animationIsPlaying == true)
        {
            return;
        }

        OnBeforeCloseAnimation();

        if(playAnimation == false)
        {
            OnClose();
            ResetUIVariables();
        }
        else
        {
            PlayCloseAnimation();
        }
    }

    protected void ResetUIVariables()
    {
        isOpen = false;

        ResetUIVisuals();
        
        content.gameObject.SetActive(false);
        blockerBackground.gameObject.SetActive(false);
    }

    protected void ResetUIVisuals()
    {
        content.anchoredPosition = initialAnchoredPosition;
        content.localScale = initialLocalScale;
        contentCanvasGroup.alpha = 1;
        blockerBackgroundCanvasGroup.alpha = 1;
    }
}
