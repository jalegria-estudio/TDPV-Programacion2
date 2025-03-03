﻿namespace Settings
{
    internal static class Config
    {
        internal const string GAME_NAME = "Ghost 'n Cats";
        internal const float RAYCAST_OFFSET = 0.01f; //<(i) For it doesn't collide with selft objectselftobject
        internal const float RAYCAST_LENG_DEFAULT = .1f;
        internal const float PLAYER_JUMP_IMPULSE = 6.9f;
        internal const float PLAYER_JUMP_IMPULSE_UP = 0.25f;//<(i) Factor. Put in zero to null double-jump
        internal const float PLAYER_MASS = 1.0f;
        internal const float PLAYER_DRAG_LINEAL = 1.1f;
        internal const float PLAYER_WALK_SPEED = 2.0f;
        internal const float PLAYER_WALK_RUN_FACTOR = 5.0f;//<(i) Factor
        internal const float PLAYER_WALK_DUCK_FACTOR = .25f;//<(i) Factor
        internal const float PLAYER_DUCK_SCALE = .5f;//<(i) Factor
        internal const float PLAYER_UNDUCK_SCALE = 2.0f;//<(i) Factor
        internal const float PLAYER_SPAWN_POSX_DEFAULT = .0f;
        internal const float PLAYER_SPAWN_POSY_DEFAULT = -3.5f;
        internal const float ITEM_DISAPPEAR_TIME = 10.0F;
        internal const int ANIMATOR_BASE_LAYER = 0;
    }
}

/**
 * Constants (C# Programming Guide)
 * Source: 
 * https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constants
 * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly
 * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/internal
 * 
 * Only the C# built-in types may be declared as const. Reference type constants other than String can only be initialized with a null value. User-defined types, including classes, structs, and arrays, cannot be const. Use the readonly modifier to create a class, struct, or array that is initialized one time at run time (for example in a constructor) and thereafter cannot be changed.
 * 
 * The internal keyword is an access modifier for types and type members. Internal types or members are accessible only within files in the same assembly. An assembly is an executable or dynamic link library (DLL) produced from compiling one or more source files.
 */
