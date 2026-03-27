using System;
using UnityEngine;

[Serializable]
public abstract class AnimatorParameter
{
    public abstract void Apply(Animator animator, Vector2 value);
}

[Serializable]
public class MagnitudeParameter : AnimatorParameter
{
    [SerializeField] private string paramName;

    public override void Apply(Animator animator, Vector2 value)
    {
        animator.SetFloat(paramName, value.magnitude);
    }
}

[Serializable]
public class Vector2Parameter : AnimatorParameter
{
    [SerializeField] private string xParam;
    [SerializeField] private string yParam;

    public override void Apply(Animator animator, Vector2 value)
    {
        animator.SetFloat(xParam, value.x);
        animator.SetFloat(yParam, value.y);
    }
}

public class AnimationReceiver : MonoBehaviour
{

    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    [SerializeReference, SubclassSelector]
    private AnimatorParameter[] parameters;


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
        foreach (var param in parameters)
        {
            param?.Apply(animator, direction);
        }
    }
}