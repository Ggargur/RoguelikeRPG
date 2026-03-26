using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Parameter Names")]
    [SerializeField] private string moveXParam = "MoveX";
    [SerializeField] private string moveYParam = "MoveY";
    [SerializeField] private string speedParam = "Speed";

    private IMoveAction DirectionChannel
    {
        get
        {
            if(_directionChannel != null || TryGetComponent(out _directionChannel)) return _directionChannel;
            return null;
        }
    }
    private IMoveAction _directionChannel;

    private void Start()
    {
        if(!animator)
            animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (DirectionChannel != null)
            DirectionChannel.OnMoved += OnDirectionChanged;
    }

    private void OnDisable()
    {
        if (DirectionChannel != null)
            DirectionChannel.OnMoved -= OnDirectionChanged;
    }

    private void OnDirectionChanged(Vector2 direction)
    {
        animator.SetFloat(moveXParam, direction.x);
        animator.SetFloat(moveYParam, direction.y);
        animator.SetFloat(speedParam, direction.sqrMagnitude);
    }
}