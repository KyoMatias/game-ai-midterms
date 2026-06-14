using UnityEngine;

/* =============================================================================
    Project:        Midterms
    File:           CameraFollow.cs
    Author:         Kyo Matias
    Studio:         SundayMood Studios
    Engine:         Unity 6.3 LTS
    Created:        2026-06-14
    ---------------------------------------------------------------------------
    Description:
    Standard MonoBehaviour component.
    ---------------------------------------------------------------------------
    Notes:
    ---------------------------------------------------------------------------
    - Part of the ProjectName project by SundayMood Studios.
    - Redistribution should credit the author and studio. (Kyo Matias)
    
    Notes
   ========================================================================== */

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTarget;

    [SerializeField]
    private Vector3 _placement = new Vector3(0f, 10f, 10f);

    void LateUpdate()
    {
        if (!_playerTarget)
            return;

        transform.position = _playerTarget.position + _placement;
    }
}
