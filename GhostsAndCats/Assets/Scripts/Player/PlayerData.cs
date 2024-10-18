using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("PLAYER STATUS")]

    /////////////
    // LIFES
    ////////////
    [Header("LIFES")]
    [Tooltip("Current lifes quantity")]
    [Min(0)]
    [SerializeField] protected int m_lifes = 0;
    [Tooltip("Default lifes quantity")]
    [Range(0, 10)]
    [SerializeField] protected int m_defaultLifes = 0;

    public int Lifes { get => m_lifes; }

    public void AddLifes(int p_qty = 1)
    {
        m_lifes += p_qty;
    }

    public void RemoveLifes(int p_qty = 1)
    {
        if (m_lifes > 0 && m_lifes >= p_qty)
            m_lifes -= p_qty;
    }


    /////////////
    // TUNAS
    ////////////
    [Header("TUNAS")]
    [Tooltip("Current tunas quantity")]
    [Min(0)]
    [SerializeField] protected int m_tunas = 0;
    [Tooltip("Player Default tuna quantity")]
    [Range(0, 100)]
    [SerializeField] protected int m_defaultTunas = 0;

    public void AddTunas(int p_qty = 1)
    {
        m_tunas += p_qty;
    }

    public int Tunas { get => m_tunas; }

    public void RemoveTunas(int p_qty = 1)
    {
        if (m_tunas > 0 && m_tunas >= p_qty)
            m_tunas -= p_qty;
    }

    /////////////
    // SOULS
    ////////////
    [Header("SOULS")]
    [Tooltip("Current souls quantity")]
    [Min(0)]
    [SerializeField] protected int m_souls = 0;
    [Tooltip("Player Default souls quantity")]
    [Range(0, 100)]
    [SerializeField] protected int m_defaultSouls = 0;

    protected void OnEnable()
    {
        m_lifes = m_defaultLifes;
        m_tunas = m_defaultTunas;
        m_souls = m_defaultSouls;
    }

    public int Souls { get => m_souls; }

    public void RemoveSouls(int p_qty = 1)
    {
        if (m_souls > 0 && m_souls >= p_qty)
            m_souls -= p_qty;
    }

    public void AddSouls(int p_qty = 1)
    {
        m_souls += p_qty;
    }

    ///////////////////////////////
    /// POWER-UP COSTS
    /////////////////////////////

    [Header("POWER-UP COSTS SETUP")]
    [Range(1, 100)]
    [SerializeField] protected int m_cost1Up = 1;

    public int TunasCost1up
    {
        get => m_cost1Up;
    }

    ///////////////////////////////
    /// INPUT MANAGERS
    /////////////////////////////
    [Header("MANAGERS CONFIGURATION")]

    ///////////////////////////////
    /// WALKING
    /////////////////////////////
    [Header("WALK MANAGER")]
    [Range(0, 5)]
    [SerializeField] protected float m_walkSpeed = Config.PLAYER_WALK_SPEED;
    public float WalkSpeed { get => m_walkSpeed; set => m_walkSpeed = value; }

    [Range(0, 10)]
    [SerializeField] protected float m_runSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_RUN_FACTOR;
    public float RunSpeed { get => m_runSpeed; set => m_runSpeed = value; }

    [Range(0, 5)]
    [SerializeField] protected float m_duckSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_DUCK_FACTOR;
    public float DuckSpeed { get => m_duckSpeed; set => m_duckSpeed = value; }

    ///////////////////////////////
    /// JUMPING
    /////////////////////////////
    [Header("JUMP MANAGER")]

    [Tooltip("Jump impulse. Put in zero to null jump")]
    [Range(0, 10)]
    [SerializeField] protected float m_impulse = Config.PLAYER_JUMP_IMPULSE;
    public float Impulse { get => m_impulse; set => m_impulse = value; }

    [Tooltip("Double-Jump factor. Put in zero to null double-jump")]
    [Range(0, 2)]
    [SerializeField] protected float m_impulseUp = Config.PLAYER_JUMP_IMPULSE_UP;
    public float ImpulseUp { get => m_impulseUp; set => m_impulseUp = value; }

    ///////////////////////////////
    /// AUDIO JUKEBOX
    /////////////////////////////
    [Header("AUDIO MANAGER")]
    [SerializeField] AudioClip m_sfxJump = null;
    [SerializeField] AudioClip m_sfxDefeat = null;
    [SerializeField] AudioClip m_sfxDamage = null;
    [SerializeField] AudioClip m_sfxStomp = null;
    public AudioClip SfxJump { get => m_sfxJump; set => m_sfxJump = value; }
    public AudioClip SfxDefeat { get => m_sfxDefeat; set => m_sfxDefeat = value; }
    public AudioClip SfxDamage { get => m_sfxDamage; set => m_sfxDamage = value; }
    public AudioClip SfxStomp { get => m_sfxStomp; set => m_sfxStomp = value; }
}
