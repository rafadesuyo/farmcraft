using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour
{
    [SerializeField] private Image imgSpeaker = null;
    [SerializeField] private TextMeshProUGUI txtStory = null;
    [SerializeField] private float delayBetweenLetters = 0.1f;

    private WaitForSeconds letterDelay;
    private Coroutine typeEffect = null;

    private StorySO currentStory = null;
    private int currentStoryFragmentIndex = -1;
    private bool isTyping = false;

    private const string INVISIBLE_TEXT_PREFIX = "<color=#00000000>";
    private const string INVISIBLE_TEXT_SUFFIX = "</color>";

    private void Awake()
    {
        letterDelay = new WaitForSeconds(delayBetweenLetters);
    }

    public void SetupStory(StorySO story)
    {
        currentStory = story;
        imgSpeaker.sprite = currentStory.speakgerSprite;

        currentStoryFragmentIndex = -1;

        ShowNextStory();
    }

    public void Continue()
    {
        if (isTyping)
        {
            isTyping = false;
            StopCoroutine(typeEffect);
            txtStory.text = currentStory.storyFragments[currentStoryFragmentIndex].text;
            return;
        }

        ShowNextStory();
    }

    private void ShowNextStory()
    {
        currentStoryFragmentIndex++;

        if (currentStoryFragmentIndex >= currentStory.storyFragments.Length)
        {
            StorytellingManager.Instance.FinishStory();
            return;
        }

        typeEffect = StartCoroutine(TypeTextEffect(currentStory.storyFragments[currentStoryFragmentIndex].text));
    }

    private IEnumerator TypeTextEffect(string text = "")
    {
        isTyping = true;
        txtStory.text = "";

        bool isRichText = false;
        string textToWrite = "";

        for (int i = 0; i < text.Length; i++)
        {
            if (!isRichText && string.Equals(text[i], '<'))
            {
                isRichText = true;
            }

            if (isRichText)
            {
                txtStory.text = $"{txtStory.text}{text[i]}";

                if (string.Equals(text[i], '>'))
                {
                    isRichText = false;
                }

                continue;
            }

            textToWrite = text.Substring(0, i);
            textToWrite = $"{textToWrite}{GetInvisibleText(text.Substring(i))}";
            txtStory.text = textToWrite;

            yield return letterDelay;
        }

        isTyping = false;
    }

    private string GetInvisibleText(string originalText)
    {
        string invisibleText = "";
        bool currentCharIsRichTextTag = false;

        invisibleText = $"{INVISIBLE_TEXT_PREFIX}";

        for (int i = 0; i < originalText.Length; i++)
        {
            if (!currentCharIsRichTextTag && string.Equals(originalText[i], '<'))
            {
                invisibleText = $"{invisibleText}{INVISIBLE_TEXT_SUFFIX}<";
                currentCharIsRichTextTag = true;
                continue;
            }

            if (currentCharIsRichTextTag && string.Equals(originalText[i], '>'))
            {
                invisibleText = $"{invisibleText}>{INVISIBLE_TEXT_PREFIX}";
                currentCharIsRichTextTag = false;
                continue;
            }

            invisibleText = $"{invisibleText}{originalText[i]}";
        }

        invisibleText = $"{invisibleText}{INVISIBLE_TEXT_SUFFIX}";

        return invisibleText;
    }
}