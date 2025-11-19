using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Finish Line Reached by " + collision.gameObject.name);

            var player = collision.GetComponent<Player>();

            if (player != null)
            {
                GameManager.Instance.Goal(player);
            }
        }
    }
}
