using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ObjectiveHUDManger : MonoBehaviour
{
    [Tooltip("UI panel containing the layoutGroup for displaying objectives")]
    public UITable objectivePanel;
    [Tooltip("Prefab for the primary objectives")]
    public PoolObjectDef primaryObjectivePrefab;
    [Tooltip("Prefab for the primary objectives")]
    public PoolObjectDef secondaryObjectivePrefab;

    [SerializeField] private bool isJuicy = true;
    [SerializeField] private bool isInformational = true;
    [SerializeField] private CanvasGroup checkpointCounter;
    private TMP_Text checkpointCounterText;

    Dictionary<Objective, ObjectiveToast> m_ObjectivesDictionary;

    void Awake()
    {
        m_ObjectivesDictionary = new Dictionary<Objective, ObjectiveToast>();
        checkpointCounterText = checkpointCounter.GetComponentInChildren<TMP_Text>();
        checkpointCounterText.fontSize = 40f;
    }

    public void RegisterObjective(Objective objective)
    {
        objective.onUpdateObjective += OnUpdateObjective;

        // instanciate the Ui element for the new objective
        GameObject objectiveUIInstance = objective.isOptional ? secondaryObjectivePrefab.getObject(true, objectivePanel.transform) : primaryObjectivePrefab.getObject(true, objectivePanel.transform);

        if (!objective.isOptional)
            objectiveUIInstance.transform.SetSiblingIndex(0);

        ObjectiveToast toast = objectiveUIInstance.GetComponent<ObjectiveToast>();
        DebugUtility.HandleErrorIfNullGetComponent<ObjectiveToast, ObjectiveHUDManger>(toast, this, objectiveUIInstance.gameObject);

        // initialize the element and give it the objective description
        toast.Initialize(objective.title, objective.description, objective.GetUpdatedCounterAmount(), objective.isOptional, objective.delayVisible);

        m_ObjectivesDictionary.Add(objective, toast);

        objectivePanel.UpdateTable(toast.gameObject);
    }

    public void UnregisterObjective(Objective objective)
    {
        objective.onUpdateObjective -= OnUpdateObjective;

        // if the objective if in the list, make it fade out, and remove it from the list
        if (m_ObjectivesDictionary.TryGetValue(objective, out ObjectiveToast toast))
            toast.Complete();
        
        m_ObjectivesDictionary.Remove(objective);
    }

    void OnUpdateObjective(UnityActionUpdateObjective updateObjective)
    {
        if (m_ObjectivesDictionary.TryGetValue(updateObjective.objective, out ObjectiveToast toast))
            //&& !string.IsNullOrEmpty(descriptionText))
        {
            // set the new updated description for the objective, and forces the content size fitter to be recalculated
            Canvas.ForceUpdateCanvases();
            if (!string.IsNullOrEmpty(updateObjective.descriptionText))
                toast.SetDescriptionText(updateObjective.descriptionText);

            if (!string.IsNullOrEmpty(updateObjective.counterText))
                toast.counterTextContent.text = updateObjective.counterText;

            RectTransform toastRectTransform = toast.GetComponent<RectTransform>();
            if (toastRectTransform != null) UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(toastRectTransform);

            if(isInformational)
                checkpointCounterText.text = updateObjective.counterText;
            else
                checkpointCounterText.text = "Good Job!";


            if (isJuicy)
            {
                DOTween.Sequence().Append(DOTween.To(() => checkpointCounter.alpha, x => checkpointCounter.alpha = x, 1f, 1f))
                    .Append(DOTween.To(() => checkpointCounter.alpha, x => checkpointCounter.alpha = x, 0f, .5f));
            }
            else
            {
                DOTween.Sequence().Append(DOTween.To(() => checkpointCounter.alpha, x => checkpointCounter.alpha = x, 1f, .2f))
                   .Append(DOTween.To(() => checkpointCounter.alpha, x => checkpointCounter.alpha = x, 0f, .8f));
            }

            if (isJuicy)
            {
                DOTween.Sequence().Append(DOTween.To(() => checkpointCounterText.fontSize, x => checkpointCounterText.fontSize = x, 100f, 1f))
                    .Append(DOTween.To(() => checkpointCounterText.fontSize, x => checkpointCounterText.fontSize = x, 40f, .5f));
            }

            
        }
    }
}
