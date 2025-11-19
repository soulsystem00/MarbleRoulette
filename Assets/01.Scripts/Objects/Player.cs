using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer skillBar;
    [SerializeField] TextMeshPro playerNameText;
    [SerializeField] Color playerColor;

    private float skillTimer = 0f;
    private float timer = 0f;

    private void Awake()
    {
        if (rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (playerNameText == null)
        {
            playerNameText = GetComponentInChildren<TextMeshPro>();
        }

        rigid.simulated = false;
    }

    public void Move()
    {
        if (skillTimer <= 0f || timer >= skillTimer)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= skillTimer)
        {
            Skill();
        }
    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }

    public void SetPlayerColor(Color color)
    {
        playerColor = color;
        spriteRenderer.color = playerColor;
        skillBar.color = playerColor;
        playerNameText.color = playerColor;
    }

    public string GetPlayerName()
    {
        return playerNameText.text;
    }

    public void SetSkillTimer(float time)
    {
        skillTimer = time;
    }

    public void EnableControl()
    {
        rigid.simulated = true;
    }

    public void DisableControl()
    {
        rigid.simulated = false;
    }

    private void Skill()
    {

    }
}
