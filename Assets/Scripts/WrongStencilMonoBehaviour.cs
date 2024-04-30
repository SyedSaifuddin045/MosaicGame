using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WrongStencilMonoBehaviour : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        GameManager.Instance.ReturnObjectToPool(gameObject);
    }
}