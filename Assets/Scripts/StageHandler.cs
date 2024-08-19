using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    private Transform _parent;
    public Vector3 _position;
    public Quaternion _rotation;
    public RocketStage StageType;
    public Rigidbody Rigidbody;
    private void Awake()
    {
        _parent = transform.parent;
        _position = transform.localPosition;
        _rotation = transform.rotation;
    }
    public void ResetStage()
    {
        Rigidbody.useGravity = false;
        Rigidbody.velocity = Vector3.zero;
        transform.rotation = _rotation;
        transform.SetParent(_parent, true);
        transform.localPosition = _position;
        gameObject.SetActive(true);
    }
    public void DestroyStage(float duration)
    {
        transform.SetParent(null, true);
        Rigidbody.useGravity = true;
        StartCoroutine(WaitAndDestroy(duration));
    }
    public bool IsStage(RocketStage type) => type == StageType;
    private IEnumerator WaitAndDestroy(float duration)
    {
        float time = 0f;
        while (time < duration) {
            time += Time.deltaTime;
            yield return null;
        }
        DestroyStage();
    }
    private void DestroyStage(){
        gameObject.SetActive(false);
    }
    public static bool TryGetStage(List<StageHandler> stages, RocketStage type, out StageHandler stage)
    {
        stage = null;
        foreach (var st in stages)
        {
            if(st.IsStage(type)) {
                stage = st;
                return true;
            }
        }
        return false;
    }
    public static void ResetStages(List<StageHandler> stages)
    {
        foreach (var st in stages)
        {
            st.ResetStage();
        }
    }
}
