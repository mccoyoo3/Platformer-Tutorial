﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class ChangeConfiner : MonoBehaviour
{
    private GameObject cam;
    private CinemachineVirtualCamera vcam;
    private CinemachineConfiner confiner;
    private PolygonCollider2D polygon;
    public LayerMask layer;
    public float RangeX;
    public float RangeY;
    private bool isChanged;
    private float previousOrthoSize;
    public GameObject[] objs;

    private void Start()
    {
        cam = GameObject.Find("CM vcam1");
        vcam = cam.GetComponent<CinemachineVirtualCamera>();
        polygon = GetComponent<PolygonCollider2D>();
        confiner = vcam.GetComponent<CinemachineConfiner>();
        gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.01f);
        previousOrthoSize = vcam.m_Lens.OrthographicSize;
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isChanged)
        {
            Collider2D hitCharacer = Physics2D.OverlapBox(transform.position, new Vector2(RangeX, RangeY), 0, layer);
            confiner.m_BoundingShape2D = polygon;
            if(hitCharacer != null)
            vcam.m_Follow = hitCharacer.transform;
            isChanged = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(RangeX, RangeY, 1));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CamSizer();
            gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 1f);
            isChanged = true;

            for(int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 1f);
            isChanged = false;

            for (int i = 0; i < objs.Length; i++)
            {
                if (confiner.m_BoundingShape2D != polygon)
                {
                    objs[i].SetActive(false);
                }
            }
        }
    }

    private void CamSizer()
    {
        previousOrthoSize = vcam.m_Lens.OrthographicSize;
        if (RangeX == 15 && RangeY == 15) // cube
        {
            DOVirtual.Float(previousOrthoSize, 5.05f, 0.15f, angle =>
            {
                vcam.m_Lens.OrthographicSize = angle;
            });
        }
        else if (RangeX == 15 && RangeY == 30) // vertical
        {
            DOVirtual.Float(previousOrthoSize, 3.7f, 0.15f, angle =>
            {
                vcam.m_Lens.OrthographicSize = angle;
            });
        }
        else if (RangeX == 30 && RangeY == 15) // horizontal
        {
            DOVirtual.Float(previousOrthoSize, 4.5f, 0.15f, angle =>
            {
                vcam.m_Lens.OrthographicSize = angle;
            });
        }
        else if (RangeX == 30 && RangeY == 30) // big cube
        {
            DOVirtual.Float(previousOrthoSize, 5.05f, 0.15f, angle =>
            {
                vcam.m_Lens.OrthographicSize = angle;
            });
        }
    }
}