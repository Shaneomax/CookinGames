using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float duration = 0.5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private Tween currentTween;

    private void Start()
    {
        closedRotation = transform.localRotation;

        openRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + openAngle,
            transform.localEulerAngles.z
        );
    }

    public void Interact()
    {
        isOpen = !isOpen;

        currentTween?.Kill();

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        currentTween = transform.DOLocalRotateQuaternion(targetRotation, duration)
                                .SetEase(Ease.OutCubic);

        Debug.Log("Door is now " + (isOpen ? "Open" : "Closed"));
    }
}