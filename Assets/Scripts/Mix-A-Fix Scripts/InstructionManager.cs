//using System.Collections.Generic;
//using UnityEngine;

//public class InstructionManager : MonoBehaviour
//{
//    public static InstructionManager Instance;

//    [Header("Mixing Steps")]
//    public List<MixStep> steps = new List<MixStep>();

//    private int currentStepIndex = 0;
//    private int currentCollectedAmount = 0;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    public MixStep GetCurrentStep()
//    {
//        return steps[currentStepIndex];
//    }

//    // Called by bowl when an ingredient is added
//    public bool ValidateIngredient(IngredientType addedType)
//    {
//        MixStep currentStep = GetCurrentStep();

//        if (addedType != currentStep.ingredientType)
//            return false; // wrong ingredient

//        currentCollectedAmount++;

//        if (currentCollectedAmount >= currentStep.requiredAmount)
//        {
//            // step finished -> go to next step
//            currentCollectedAmount = 0;
//            currentStepIndex++;

//            if (currentStepIndex >= steps.Count)
//            {
//                GameCompleted();
//            }
//            else
//            {
//                ShowNextInstruction();
//            }
//        }

//        return true;
//    }

//    private void ShowNextInstruction()
//    {
//        Debug.Log("Next step: " + GetCurrentStep().ingredientType +
//                  " x" + GetCurrentStep().requiredAmount);
//        // later we will update UI/voice here
//    }

//    private void GameCompleted()
//    {
//        Debug.Log("All steps completed!");
//        // trigger success animation
//    }
//}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager Instance;

    [Header("Instruction Steps")]
    public List<MixStep> steps = new List<MixStep>();
    private int currentStepIndex = 0;
    private int currentCount = 0;

    [Header("UI")]
    public Text instructionText; // Optional: show current instruction
    public FeedbackManager feedbackManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowInstruction();
    }

    // ✅ THIS METHOD IS REQUIRED
    public void IngredientAdded(IngredientType added)
    {
        MixStep step = steps[currentStepIndex];

        if (added != step.ingredientType)
        {
            feedbackManager.ShowWrong();
            return;
        }

        currentCount++;

        if (currentCount >= step.requiredAmount)
        {
            feedbackManager.ShowCorrect();
            currentCount = 0;
            currentStepIndex++;

            if (currentStepIndex >= steps.Count)
            {
                GameComplete();
                return;
            }

            ShowInstruction();
        }
        else
        {
            feedbackManager.ShowPartial(step.requiredAmount - currentCount);
        }
    }

    private void ShowInstruction()
    {
        MixStep step = steps[currentStepIndex];
        instructionText.text = $"Add {step.requiredAmount} of {step.ingredientType}";
        Debug.Log("Next instruction: " + instructionText.text);
    }

    private void GameComplete()
    {
        instructionText.text = "✅ All steps completed!";
        feedbackManager.ShowGameComplete();
    }
}
