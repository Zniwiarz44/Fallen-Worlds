using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetection : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerMovementV2 player = SceneCacheManager.GetCashedObject("Player").GetComponent<PlayerMovementV2>();
        player.OnCombatEngage(gameObject);
    }
}
