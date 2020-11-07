using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPVFX : MonoBehaviour  //Post-processing VFX script
{
    public PostProcessVolume volume;
    private ChromaticAberration ghostAbberation;
    private Grain grain;
    private ColorGrading colorGrade;
    void Start()
    {
        volume.profile.TryGetSettings(out ghostAbberation);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out colorGrade);
        ghostAbberation.intensity.value = 0f;
        grain.intensity.value = 0f;
        colorGrade.tint.value = 0f;
    }

    void Update()
    {
        
    }

    public void enteredGhostMode()
    {
        chromaticEffect(true);
        grainEffect(true);
        tintFX(true);
    }

    public void exitedGhostMode()
    {
        chromaticEffect(false);
        grainEffect(false);
        tintFX(false);
    }

    public void chromaticEffect(bool check)
    {
        if(check == true)
        {
            ghostAbberation.intensity.value = .5f;
        }
        else if (check == false)
        {
            ghostAbberation.intensity.value = 0f;
        }
    }

    public void grainEffect(bool check)
    {
        if(check == true)
        {
            grain.intensity.value = 1f;
        }
        else if (check == false)
        {
            grain.intensity.value = 0f;
        }
    }

    public void tintFX(bool check)
    {
        if(check == true)
        {
            colorGrade.tint.value = 80f;
        }
        else if (check == false)
        {
            colorGrade.tint.value = 0f;
        }
    }
}
