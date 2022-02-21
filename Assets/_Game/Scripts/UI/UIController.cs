using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("UI Indication:"), SerializeField]
    Indicators[] indicators;    //The ui elements representing 2, 4, 8, 16, 32

    [Header("Outline Data:"), SerializeField]
    Transform outlineData;

    Transform target;
    Camera cam;
    bool canTap;
    Field selectedFiled;

    private void Start()
    {
        canTap = true;
        cam = FindObjectOfType<Camera>();
        Timing.RunCoroutine(_SlowUpdate().CancelWith(gameObject));
    }
    private void Update()
    {
        if (canTap)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                Ray r = cam.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                Ray r = cam.ScreenPointToRay(Input.touches[0].position);
#endif
                if (Physics.Raycast(r, out RaycastHit hit))
                {
                    if (hit.collider.isTrigger && hit.transform.CompareTag("Field"))
                    {
                        CanTap();
                        selectedFiled = hit.transform.GetComponent<Field>();
                        if (selectedFiled.id >= outlineData.GetChild(0).GetComponent<Indicators>().cubeID)
                        {
                            target = hit.transform;
                            SelectedField();
                        }
                    }
                    if (hit.transform.CompareTag("Player"))
                    {
                        CanTap();
                        target = hit.transform;
                        SelectedField();
                    }
                }
            }
        }
    }
    #region Tap:
    void CanTap()
    {
        canTap = false;
        Timing.KillCoroutines("Tap");
        Timing.RunCoroutine(_CanTap().CancelWith(gameObject), "Tap");
    }
    IEnumerator<float> _CanTap()
    {
        yield return Timing.WaitForSeconds(.25f);
        canTap = true;
    }
    #endregion
    IEnumerator<float> _SlowUpdate()
    {
        while (!GameController.instance.end)
        {
            if (GameController.instance.end) break;
            GameController.instance.CheckOutline();
            yield return Timing.WaitForSeconds(.25f);
        }
        gameObject.SetActive(false);
    }

    void CreateOutlineChild()
    {
        int randIndicator = Random.Range(0, indicators.Length);
        var i = Instantiate(indicators[randIndicator]);
        i.transform.SetParent(outlineData);
        i.transform.localScale = Vector3.one;
    }

    public void SetNextCube(int spawnID)
    {
        GameController.instance.SpawnCube(spawnID, target);
    }

    void SelectedField()
    {
        SetNextCube(outlineData.GetChild(0).GetComponent<Indicators>().spawnID);
        RemoveNextCube();
    }

    public void RemoveNextCube()
    {
        Destroy(outlineData.GetChild(0).gameObject);
        CreateOutlineChild();
    }
}
