using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelAnimator : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private VoxelAnimation[] animations;
    [SerializeField] private string initialAnimation;

    private Dictionary<string, VoxelAnimation> animationsDict;
    private VoxelAnimation currentAnimation;
    private MeshFilter meshFilter;

    void Awake()
    {
        animationsDict = new Dictionary<string, VoxelAnimation>();
        foreach (var anim in animations)
        {
            animationsDict.Add(anim.animationName, anim);
        }

        meshFilter = GetComponent<MeshFilter>();
        GetComponent<MeshRenderer>().material = material;

        PlayAnimation(initialAnimation);
    }

    public void PlayAnimation(string animationName)
    {
        if ((currentAnimation == null || animationName != currentAnimation.animationName) 
            && animationsDict.ContainsKey(animationName))
        {
            currentAnimation = animationsDict[animationName];
            StopAllCoroutines();
            StartCoroutine(AnimationRoutine());
        }
    }

    IEnumerator AnimationRoutine()
    {
        float timeBetweenFrames = 1f / currentAnimation.framesPerSecond;
        int currentFrame = 0;
        int maxFrame = currentAnimation.frames.Length;

        while (true)
        {
            meshFilter.mesh = currentAnimation.frames[currentFrame];

            yield return new WaitForSeconds(timeBetweenFrames);
            currentFrame++;
            if (currentFrame >= maxFrame) currentFrame = 0;
        }

        yield return null;
    }
}
