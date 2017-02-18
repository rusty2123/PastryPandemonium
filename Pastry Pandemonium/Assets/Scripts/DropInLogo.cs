using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class DropInLogo : MonoBehaviour {

    public GameObject drop;
    public GameObject scale;

    private void Awake()
    {

    }

    private void Start()
    {
        drop.transform.position += Vector3.up * 5.1f;
        LeanTween.moveY(drop, drop.transform.position.y - 5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.9f).setOnComplete(PlayBoom);

        LeanTween.scale(scale, new Vector3(0.8f, 0.8f, 0.8f), 24f);
    }

    void PlayBoom()
    {
        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1.163155f, 0f, -1f), new Keyframe(0.3098361f, 0f, 0f, 0f), new Keyframe(0.5f, 0.003534712f, 0f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0.000819672f, 0.007666667f, 0f, 0f), new Keyframe(0.01065573f, 0.002424242f, 0f, 0f), new Keyframe( 0.02704918f, 0.007454545f, 0f, 0f), new Keyframe(0.03770492f, 0.002575758f, 0f, 0f), new Keyframe(0.052459f, 0.007090909f, 0f, 0f), new Keyframe(0.06885245f, 0.002939394f, 0f, 0f), new Keyframe(0.0819672f, 0.006727273f, 0f, 0f), new Keyframe(0.1040983f, 0.003181818f, 0f, 0f), new Keyframe(0.1188525f, 0.006212121f, 0f, 0f), new Keyframe(0.145082f, 0.004151515f, 0f, 0f), new Keyframe(0.1893443f, 0.005636364f, 0f, 0f));

        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.1f, 0f, 0f) }).setFrequency(11025));

        LeanAudio.play(audioClip);
    }
}
