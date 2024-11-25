using Settings;
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

    public int Lifes { get => this.m_lifes; }

    public void AddLifes(int p_qty = 1)
    {
        this.m_lifes += p_qty;
    }

    public void RemoveLifes(int p_qty = 1)
    {
        if (this.m_lifes > 0 && this.m_lifes >= p_qty)
            this.m_lifes -= p_qty;
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
    [Tooltip("Tuna score")]
    [Range(0, 100)]
    [SerializeField] protected int m_tunaScore = 10;

    public void AddTunas(int p_qty = 1)
    {
        this.m_tunas += p_qty;
    }

    public int Tunas { get => this.m_tunas; }

    public void RemoveTunas(int p_qty = 1)
    {
        if (this.m_tunas > 0 && this.m_tunas >= p_qty)
            this.m_tunas -= p_qty;
    }

    public int TunaScore { get => this.m_tunaScore; }

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
    [Tooltip("Soul score")]
    [Range(0, 1000)]
    [SerializeField] protected int m_soulScore = 100;

    protected void OnEnable()
    {
        this.m_lifes = this.m_defaultLifes;
        this.m_tunas = this.m_defaultTunas;
        this.m_souls = this.m_defaultSouls;
        this.m_score = 0;
    }

    public int Souls { get => this.m_souls; }

    public void RemoveSouls(int p_qty = 1)
    {
        if (this.m_souls > 0 && this.m_souls >= p_qty)
            this.m_souls -= p_qty;
    }

    public void AddSouls(int p_qty = 1)
    {
        this.m_souls += p_qty;
        //this.m_score += this.m_soulScore;
    }

    public int SoulScore { get => this.m_soulScore; }

    /////////////
    // SCORES
    ////////////
    [Header("SCORE")]
    [Tooltip("Player's score on gameplay")]
    [Min(0)]
    [SerializeField] protected int m_score = 0;
    public int Score { get => this.m_score; set => this.m_score = value; }

    [Tooltip("Hi-Score of game")]
    [Range(0, 999999)]
    [SerializeField] protected int m_hiscore = 0;
    public int HiScore { get => this.m_hiscore; set => this.m_hiscore = value; }

    [Tooltip("Player's score point to win per time point")]
    [Range(0, 10)]
    [SerializeField] protected int m_timeScore = 1;
    public int TimeScore { get => this.m_timeScore; set => this.m_timeScore = value; }

    [Tooltip("Player's score point to win per boss knock out")]
    [Range(0, 2000)]
    [SerializeField] protected int m_knockOutScore = 1000;
    public int KnockOutScore { get => this.m_knockOutScore; set => this.m_knockOutScore = value; }

    ///////////////////////////////
    /// POWER-UP COSTS
    /////////////////////////////

    [Header("POWER-UP COSTS SETUP")]
    [Range(1, 100)]
    [SerializeField] protected int m_cost1Up = 1;

    public int TunasCost1up
    {
        get => this.m_cost1Up;
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
    public float WalkSpeed { get => this.m_walkSpeed; set => this.m_walkSpeed = value; }

    [Range(0, 10)]
    [SerializeField] protected float m_runSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_RUN_FACTOR;
    public float RunSpeed { get => this.m_runSpeed; set => this.m_runSpeed = value; }

    [Range(0, 5)]
    [SerializeField] protected float m_duckSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_DUCK_FACTOR;
    public float DuckSpeed { get => this.m_duckSpeed; set => this.m_duckSpeed = value; }

    ///////////////////////////////
    /// JUMPING
    /////////////////////////////
    [Header("JUMP MANAGER")]

    [Tooltip("Jump impulse. Put in zero to null jump")]
    [Range(0, 10)]
    [SerializeField] protected float m_impulse = Config.PLAYER_JUMP_IMPULSE;
    public float Impulse { get => this.m_impulse; set => this.m_impulse = value; }

    [Tooltip("Double-Jump factor. Put in zero to null double-jump")]
    [Range(0, 2)]
    [SerializeField] protected float m_impulseUp = Config.PLAYER_JUMP_IMPULSE_UP;
    public float ImpulseUp { get => this.m_impulseUp; set => this.m_impulseUp = value; }

    ///////////////////////////////
    /// AUDIO JUKEBOX
    /////////////////////////////
    [Header("AUDIO MANAGER")]
    [SerializeField] AudioClip m_sfxJump = null;
    [SerializeField] AudioClip m_sfxDefeat = null;
    [SerializeField] AudioClip m_sfxDamage = null;
    [SerializeField] AudioClip m_sfxStomp = null;
    [SerializeField] AudioClip m_sfx1up = null;
    [SerializeField] AudioClip m_sfxScore = null;
    [SerializeField] AudioClip m_sfxScoreTime = null;
    [SerializeField] AudioClip m_sfxScoreBoss = null;
    [SerializeField] AudioClip m_sfxStageClear = null;
    [SerializeField] AudioClip m_sfxHiScore = null;

    public AudioClip SfxJump { get => this.m_sfxJump; set => this.m_sfxJump = value; }
    public AudioClip SfxDefeat { get => this.m_sfxDefeat; set => this.m_sfxDefeat = value; }
    public AudioClip SfxDamage { get => this.m_sfxDamage; set => this.m_sfxDamage = value; }
    public AudioClip SfxStomp { get => this.m_sfxStomp; set => this.m_sfxStomp = value; }
    public AudioClip Sfx1up { get => this.m_sfx1up; set => this.m_sfx1up = value; }
    public AudioClip SfxScore { get => this.m_sfxScore; set => this.m_sfxScore = value; }
    public AudioClip SfxScoreTime { get => this.m_sfxScoreTime; set => this.m_sfxScoreTime = value; }
    public AudioClip SfxScoreBoss { get => this.m_sfxScoreBoss; set => this.m_sfxScoreBoss = value; }
    public AudioClip SfxHiScore { get => this.m_sfxHiScore; set => this.m_sfxHiScore = value; }
    public AudioClip SfxStageClear { get => this.m_sfxStageClear; set => this.m_sfxStageClear = value; }

    public void ResetDefault()
    {
        this.m_tunas = this.m_defaultTunas;
        this.m_souls = this.m_defaultSouls;
        this.m_lifes = this.m_defaultLifes;
        this.m_score = 0;
    }
}
