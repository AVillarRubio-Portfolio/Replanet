using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ContaminationPostProcessing : MonoBehaviour
{
    [SerializeField] private float maxOffset;
    [SerializeField] [Range(0f, 1f)] float fogLevel;
    [SerializeField] private PostProcessProfile profile;

    private ColorGrading colorGrading;

    void Awake()
    {
        colorGrading = profile.GetSetting<ColorGrading>();
    }

    private void Start()
    {
        if (Globals.maxLevel > 4)
            fogLevel = 0.0f;
       else if (Globals.maxLevel > 3)
            fogLevel = .2f;
        else if (Globals.maxLevel > 1)
            fogLevel = .5f;
    }
    void Update()
    {
        colorGrading.contrast.value = (-maxOffset * fogLevel);
        colorGrading.saturation.value = (-maxOffset * fogLevel);
    }
}
