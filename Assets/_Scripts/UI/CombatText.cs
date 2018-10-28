using UnityEngine;
using TMPro;

public enum CombatTextType
{
    NormalDamage,
    Heal,
    CriticalDamage

}

public enum InstantiateDirection
{
    UpperCentre,
    UpperLeft,
    UpperRight,
    None
}

public class CombatText : MonoBehaviour {

    const float TEXT_SPEED = .75f;
    Vector2 direction;
    CombatTextType combatTextType;
    InstantiateDirection instantiateDirection = InstantiateDirection.None;
    
    void Update()
    {
        if (instantiateDirection == InstantiateDirection.None)
        {
            var randomInt = Random.Range(0, 3);
            instantiateDirection = (InstantiateDirection)randomInt;

            switch (instantiateDirection)
            {
                case InstantiateDirection.UpperCentre:
                    direction = Vector2.up;
                    break;
                case InstantiateDirection.UpperLeft:
                    direction = (Vector2.left / 2) + Vector2.up;
                    break;
                case InstantiateDirection.UpperRight:
                    direction = (Vector2.right / 2) + Vector2.up;
                    break;
            }
        }

        float translation = TEXT_SPEED * Time.deltaTime;
        transform.Translate(direction * translation);
    }

    public void Initialise(CombatTextType combatTextType)
    {
        this.combatTextType = combatTextType;

        Animator controller = GetComponent<Animator>();
        GetComponent<TextMeshProUGUI>().color = GetTextColour();
        float animationLength = controller.runtimeAnimatorController.animationClips[0].length;

        Destroy(gameObject, animationLength);
    }

    Color GetTextColour()
    {
        var tempColor = Color.white;

        switch (combatTextType)
        {
            case CombatTextType.NormalDamage:
                break;
            case CombatTextType.CriticalDamage:
                tempColor = Color.yellow;
                break;
            case CombatTextType.Heal:
                tempColor = Color.green;
                break;
        }

        return tempColor;
    }
}
