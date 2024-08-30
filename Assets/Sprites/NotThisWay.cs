using UnityEngine;
using DG.Tweening;
using System.Collections;
public class NotThisWay : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject player;
    [SerializeField] Animator walkAnim;
    [SerializeField] Collider col;
    public Transform pos;
    void Start()
    {
    }
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other) 
    {
        GetComponent<Collider>().enabled = false;
        Tween rotateTween = player.transform.DORotateQuaternion(Quaternion.Euler(new Vector3(player.transform.eulerAngles.x, pos.transform.eulerAngles.y, player.transform.eulerAngles.z)), 0.75f);
        Tween moveTween = player.transform.DOMove(pos.transform.position, 0.75f);
        Dialogue.StartDialogue(2);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            StartCoroutine(WaitUntilDeactivated(dialogueBox, sequence));
        });
        sequence.Append(rotateTween);
        sequence.AppendCallback(() => walkAnim.Play("WalkDoor"));
        sequence.Append(moveTween);
        sequence.AppendCallback(() => walkAnim.Play("Idle"));
        sequence.AppendCallback(() => GetComponent<Collider>().enabled = true);
    }
    private IEnumerator WaitUntilDeactivated(GameObject obj, Sequence sequence)
    {
        sequence.Pause();
        yield return new WaitUntil(() => !obj.activeInHierarchy);
        sequence.Play();
    }
}
