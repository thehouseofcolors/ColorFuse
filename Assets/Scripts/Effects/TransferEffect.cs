using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

//bu sınıf ekstra, ne yapacağımı bilmiyorum belki sonra bakarım


public class TransferEffectManager : MonoBehaviour
{
    private GameObject currentEffect;

    public async Task PlayEffect(Vector3 from, Vector3 to, Color color)
    {
        if (currentEffect != null)
            Destroy(currentEffect);

        // currentEffect = Instantiate(effectPrefab);
        currentEffect.transform.position = from;

        var renderer = currentEffect.GetComponent<SpriteRenderer>();
        renderer.color = color;

        await currentEffect.transform.DOMove(to, 0.35f).AsyncWaitForCompletion();

        Destroy(currentEffect);
        currentEffect = null;
    }
}
