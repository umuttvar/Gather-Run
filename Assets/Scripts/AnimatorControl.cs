using TMPro;
using UnityEngine;

public class AnimatorControl : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI savedText;

    public void AnimatorInactive()
    {
        animator.SetBool("Save", false);
    }

    public void SavedTextLanguage(string state)
    {
        switch (state)
        {

            case "TR":
                savedText.text = "Kaydedildi";
                break;

            case "EN":
                savedText.text = "Saved";
                break;
        }
    }
}
