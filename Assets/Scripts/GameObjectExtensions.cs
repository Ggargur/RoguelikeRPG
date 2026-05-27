using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Verifica se o GameObject possui o componente PlayerPivot (é o player)
    /// </summary>
    public static bool IsPlayer(this GameObject gameObject)
    {
        return gameObject.GetComponent<PlayerPivot>() != null;
    }

    /// <summary>
    /// Verifica se o GameObject com o Collider é um player
    /// </summary>
    public static bool IsPlayer(this Collider2D collider)
    {
        return collider.gameObject.IsPlayer();
    }
}

