using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelHandler : MonoBehaviour
{
    // clearPanel �ִϸ��̼� �Ϸ�� �� ȣ��
    public System.Action onPanelShowComplete;

    void Awake()
    {
        DOTween.Init();

    }

    public void Show()
    {
        gameObject.SetActive(true);

        // DOTween �Լ��� ���ʴ�� �����ϰ� ���ݴϴ�.
        var seq = DOTween.Sequence();

        // DOScale �� ù ��° �Ķ���ʹ� ��ǥ Scale ��, �� ��°�� �ð��Դϴ�.
        seq.Append(transform.DOScale(1.1f, 0.3f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.3f));

        // OnComplete �� seq �� ������ �ִϸ��̼��� �÷��̰� �Ϸ�Ǹ�
        // { } �ȿ� �ִ� �ڵ尡 ����ȴٴ� �ǹ��Դϴ�.
        // ���⼭�� �ݱ� �ִϸ��̼��� �Ϸ�� �� ��ü�� ��Ȱ��ȭ �մϴ�.
        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void EndPanelShow()
    {
        transform.position = new Vector3(960f, -540f, 0f);

        gameObject.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(540f, 0.7f));
        
        // �ִϸ��̼��� ������ �� ȣ��� �ݹ�
        seq.OnComplete(() =>
        {
            if (onPanelShowComplete != null)
                onPanelShowComplete.Invoke();
        });

        seq.Play();
    }
}