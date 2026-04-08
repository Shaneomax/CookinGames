using UnityEngine;
using DG.Tweening;

public class SlidingDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float slideDistance = 2f;
    [SerializeField] private float duration = 0.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private Tween currentTween;

    private void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + (Vector3.right * slideDistance);
    }

    public void Interact()
    {
        isOpen = !isOpen;

        currentTween?.Kill();

        Vector3 targetPos = isOpen ? openPosition : closedPosition;

        currentTween = transform.DOLocalMove(targetPos, duration)
                                .SetEase(Ease.OutQuad);

        Debug.Log("Sliding Door is " + (isOpen ? "Open" : "Closed"));
    }
}