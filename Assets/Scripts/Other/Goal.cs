using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Goal : MonoBehaviour
{
    public int reQScore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.player.score >= reQScore)
        {
            GameManager.instance.NextStage();
        }
    }
}
