using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MSTutorialController : MonoBehaviour
{
    public static event Action TutorialFinished;
    
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private RectTransform containerTransform;
    
    [SerializeField] private float movementThreshold = .8f;
    [SerializeField] private float textHideY = 130f;
    [SerializeField] private float textShowY = -180f;
    
    private MSTutorialState currentState = MSTutorialState.Start;

    private bool didMoveLeft;
    private bool didMoveRight;
    private bool didJump;

    private bool isActive;

    private Dictionary<MSTutorialState, String> texts = new()
    {
        { MSTutorialState.Start, "שוב מנסה לברוח הא?" },
        { MSTutorialState.Move, "נו טוב, אני אעזור, החצים - כדי לזוז" },
        { MSTutorialState.Jump, "לא רע, אולי יש לך סיכוי, X כדי לקפוץ!" },
        { MSTutorialState.Done, "תענוג, עכשיו עליך רק להתחמק מהמכשולים לפי הקצב" }
    };

    public void StartTutorial()
    {
        UpdateState(MSTutorialState.Start);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isActive) return;
        
        if (currentState != MSTutorialState.Start && currentState != MSTutorialState.Jump) return;

        if (currentState == MSTutorialState.Start)
        {
            UpdateState(MSTutorialState.Move);
        }
        else if (currentState == MSTutorialState.Jump)
        {
            didJump = true;
        
            UpdateState(MSTutorialState.Done);   
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isActive) return;
        
        if (currentState != MSTutorialState.Move) return;

        if (!context.performed) return;

        Vector2 movement = context.ReadValue<Vector2>();
        
        if (movement.x <= -movementThreshold)
        {
            didMoveLeft = true;
        }

        if (movement.x >= movementThreshold)
        {
            didMoveRight = true;
        }

        if (didMoveLeft && didMoveRight)
        {
            UpdateState(MSTutorialState.Jump);
        }
    }

    private void UpdateState(MSTutorialState newState)
    {
        StartCoroutine(UpdateStateCoroutine(newState));
    }

    private IEnumerator UpdateStateCoroutine(MSTutorialState newState)
    {
        isActive = false;

        if (newState != MSTutorialState.Start)
        {
            yield return PerformOutAnimation();
        }
        
        switch (newState)
        {
            case MSTutorialState.Move:
                break;
            
            case MSTutorialState.Jump:
                break;
            
            case MSTutorialState.Done:
                StartCoroutine(DoneCourtine());
                break;
        }

        tutorialText.text = texts[newState];

        currentState = newState;

        yield return new WaitForSeconds(.5f);

        yield return PerformInAnimation();

        isActive = true;
    }

    private YieldInstruction PerformOutAnimation()
    {
        Tween outTween = containerTransform.DOAnchorPosY(textHideY, 1f).From(new Vector2(0, textShowY)).SetEase(Ease.InBack);
        return outTween.WaitForCompletion();
    }

    private YieldInstruction PerformInAnimation()
    {
        Tween inTween = containerTransform.DOAnchorPosY(textShowY, 1f).From(new Vector2(0, textHideY)).SetEase(Ease.OutBack);
        return inTween.WaitForCompletion();
    }

    private IEnumerator DoneCourtine()
    {
        yield return new WaitForSeconds(3f);

        yield return PerformOutAnimation();
        
        TutorialFinished?.Invoke();
    }
}

public enum MSTutorialState
{
    Start, Move, Jump, Done
}