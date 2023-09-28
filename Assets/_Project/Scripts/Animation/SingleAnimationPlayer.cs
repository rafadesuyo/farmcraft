using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAnimationPlayer : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Animator animator;

    //Variables
    [Header("Variables")]
    [SerializeField] private bool applyClipOverrideAutomatically = true;

    [Space(10)]

    [SerializeField] private AnimationClip animationClip;

    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides clipOverrides;

    public void ApplyClipOverride(AnimationClip _animationClip)
    {
        clipOverrides[clipOverrides[0].Key.name] = _animationClip;
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }

    public void Play()
    {
        animator.Play(0, 0, 0);
    }

    private void Awake()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);

        animatorOverrideController.GetOverrides(clipOverrides);

        animator.runtimeAnimatorController = animatorOverrideController;

        if(applyClipOverrideAutomatically == true)
        {
            ApplyClipOverride(animationClip);
        }
    }
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));

            if (index != -1)
            {
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
            else
            {
                Debug.LogError($"No Override with the name \"{name}\" was found.");
            }
        }
    }
}
