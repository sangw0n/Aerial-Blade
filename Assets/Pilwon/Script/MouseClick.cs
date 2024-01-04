using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseClick : MonoBehaviour
{
    [Header("[ Click Header ]")]
    [SerializeField] private LayerMask clickLayer;
    [SerializeField] private int tapCount;
    [SerializeField] private float zoomSize;
    [SerializeField] private float zoomOutSize;
    [SerializeField] private float resetTimer;

    private bool isDoubleCheck;
    private bool isClick = true;
    private bool isDevNotice = false;

    private bool isZooming;
    private bool isZoomOuting;
    private bool isZoom;

    [Header("[ Otehr Header ]")]
    public Vector3 zoomPos;
    public Transform player;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        zoomOutSize = cam.orthographicSize;
    }

    private void Update()
    {
        Click();
        ZoomIn();
        ZoomOut();
    }

    private void Click()
    {
        if (Input.GetMouseButtonDown(0) && isClick)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0.0f, clickLayer);

            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Cube")) return;
                DevCube devCube = hit.collider.gameObject.GetComponent<DevCube>();

                if(devCube.devState == DevState.Dev)
                {
                    isDevNotice = true;
                    if(true) StartCoroutine(DevNotice());
                    Debug.Log("°³¹ßÁß...");
                }
                else if(devCube.devState == DevState.Complete)
                {
                    Cube cube = hit.collider.gameObject.GetComponent<Cube>();
                    int selectUi = cube.id;

                    StartCoroutine(ResetTapTimer());
                    tapCount++;
                    if (!isZoom && cube.cubeLock == CubeLock.Unlock)
                    {
                        player.position = cube.spawnPos[2].position;
                    }
                    zoomPos = cube.spawnPos[1].position;

                    if (tapCount >= 2 && !isZoom)
                    {
                        isClick = false;
                        tapCount = 0;
                        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Select);

                        //if (isZooming) return;
                        Dungeon dungeon = InGameManager.instance.dungeon.GetComponent<Dungeon>();
                        dungeon.Init(cube.bossData, cube.id);
                        MenuUiManager.instance.Show(new Vector3(715, -48, 0), (int)Ui.DungeonPanel);

                        if (cube.cubeLock == CubeLock.Unlock)
                        {
                            switch (cube.id)
                            {
                                case 0:
                                case 1:
                                case 2:
                                    MenuUiManager.instance.Show(new Vector3(0, 435, 0), (int)Ui.GoldPanel);
                                    MenuUiManager.instance.Show(new Vector3(-715, -48, 0), cube.id);
                                    break;

                                default:
                                    break;
                            }
                        }
                        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Swipe);
                        isZooming = true;
                        isZoom = true;
                    }
                    if (tapCount >= 2 && isZoom)
                    {
                        isClick = false;
                        tapCount = 0;

                        //if (isZoomOuting) return;
                        MenuUiManager.instance.Hide(new Vector3(1250, -48, 0), (int)Ui.DungeonPanel);
                        MenuUiManager.instance.Hide(new Vector3(0, 714, 0), (int)Ui.GoldPanel);
                        for(int i = 0; i < 3; i++) MenuUiManager.instance.Hide(new Vector3(-1250, 0, 0), i);

                        isZoomOuting = true;
                        isZoom = false;
                    }
                }
            }
        }
    }


    private IEnumerator IsZoom(bool isSelect, float time)
    {
        yield return new WaitForSeconds(time);
        isZoom = isSelect;
    }

    private void ZoomIn()
    {
        if (!isZooming) return;
        cam.transform.position += zoomPos;
        zoomPos = Vector3.zero;
              
        float velocity = -4.2f;
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoomSize, ref velocity, 0.1f);
        if (cam.orthographicSize <= zoomSize + 0.25f)
        {
            isZooming = false;
            isClick = true;
        }
    }

    private void ZoomOut()
    {
        if (!isZoomOuting) return;

        float velocity = 4.2f;
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, 7, ref velocity, 0.1f);
        cam.transform.position = new Vector3(0, 0, -10);
        if (cam.orthographicSize >= zoomOutSize - 0.25f)
        {
            isZoomOuting = false;
            isClick = true;
        }
    }

    private IEnumerator DevNotice()
    {
        isDevNotice = false;
        MenuUiManager.instance.Show(new Vector3(761, 448, 0), 5);
        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Error);
        yield return new WaitForSecondsRealtime(2.0f);
        MenuUiManager.instance.Hide(new Vector3(1284, 448, 0), 5);
        isDevNotice = true;
    }

    private IEnumerator ResetTapTimer()
    {
        yield return new WaitForSeconds(resetTimer);
        tapCount = 0;
    }

}
