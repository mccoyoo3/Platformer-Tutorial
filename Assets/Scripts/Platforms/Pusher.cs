﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Rplayer.transform.position = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
            enabled = false;
        }
    }
}
