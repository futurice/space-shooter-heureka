using UnityEngine;
using System.Collections;
using DG.Tweening;

public class IntroBannerBehaviour : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

	}

    public void doAnimation() {
        //Lengths matched to fanfare length
        Sequence sequence = DOTween.Sequence ();
        float scaleLength = 16.0f;
        float colorLength = 5.2f;
        SpriteRenderer r = this.GetComponent<SpriteRenderer>();
        r.enabled = true;
        r.color = Color.white;
        this.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        sequence.Append(this.gameObject.transform.DOScale( new Vector3(1.0f, 1.0f, 1.0f), scaleLength).SetEase(Ease.InCubic));
        sequence.Append (r.DOColor(Color.magenta, colorLength));
        sequence.OnComplete(destroyAnimation);
        sequence.Play ();

    }
	
    private Vector3 randomPositionNearby(Vector3 pos) {
        Vector2 random= 5.0f * Random.insideUnitCircle;
        random.x = (Random.value > 0.5f ? -1.0f : 1.0f) * random.x;
        random.y = (Random.value > 0.5f ? -1.0f : 1.0f) * random.y;

        return new Vector3(pos.x - random.x, pos.y, pos.z - random.y);
    }

    private void destroyAnimation() {
        Vector3 pos = this.gameObject.transform.position;

        GameManager.Instance.AnimateExplosion(pos, -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        GameManager.Instance.AnimateExplosion(randomPositionNearby(pos), -1, 5.0f);
        SpriteRenderer r = this.GetComponent<SpriteRenderer>();
        r.enabled = false;
    }
	
}
