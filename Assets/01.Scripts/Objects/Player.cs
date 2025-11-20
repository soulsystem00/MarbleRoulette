using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer skillBar;
    [SerializeField] ParticleSystem skillEffect;
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

    private void OnEnable()
    {
        rigid.simulated = false;
        skillBar.material.SetFloat("_FillAmount", 0f);
        timer = 0f;
        skillTimer = 0f;
    }

    public void Move()
    {
        if (skillTimer <= 0f || timer >= skillTimer)
        {
            return;
        }

        timer += Time.deltaTime;
        skillBar.material.SetFloat("_FillAmount", timer / skillTimer);

        if (timer >= skillTimer)
        {
            skillTimer = 0f;
            timer = 0f;
            skillBar.material.SetFloat("_FillAmount", 0f);

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

        var particleMain = skillEffect.main;
        particleMain.startColor = playerColor;
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

    public string GetPlayerName()
    {
        return playerNameText.text;
    }

    public void QWER(Vector3 direction)
    {
        rigid.AddForce(direction * 10, ForceMode2D.Impulse);
    }

    private void Skill()
    {
        skillEffect.Play();

        var nearObjects = Physics2D.OverlapCircleAll(transform.position, 2f, 1 << LayerMask.NameToLayer("Player"));
        nearObjects = nearObjects.Where(x => x.gameObject != this.gameObject).ToArray();

        foreach (var item in nearObjects)
        {
            var player = item.GetComponent<Player>();

            // 도착 - 시작 = 시작에서 도착까지 방향 벡터

            Vector3 direction = player.transform.position - this.transform.position;
            player.QWER(direction.normalized);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
