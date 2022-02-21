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

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        Timing.RunCoroutine(_SlowUpdate().CancelWith(gameObject));
    }

    private void Update()
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
                    target = hit.transform;
                    SelectedField();
                }
            }
        }
    }

    IEnumerator<float> _SlowUpdate()
    {
        while (!GameController.instance.end)
        {
            if (GameController.instance.end) break;
            GameController.instance.CheckOutline();
            yield return Timing.WaitForSeconds(.2f);
        }
        gameObject.SetActive(false);
    }

    void CreateOutlineChild()
    {
        int randIndicator = Random.Range(0, indicators.Length);
        var i = Instantiate(indicators[randIndicator]);
        i.transform.SetParent(outlineData);
    }

    public void SetNextCube(int spawnID)
    {
        GameController.instance.SpawnCube(spawnID, target);
    }

    void SelectedField()
    {
        SetNextCube(outlineData.GetChild(0).GetComponent<Indicators>().spawnID);
        CreateOutlineChild();
    }
}
