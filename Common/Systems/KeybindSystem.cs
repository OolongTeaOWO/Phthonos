using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace Phthonos.Common.Systems;

public static class KeybindSystem
{
    //定義一個實例指向Mod本身
    public static Mod ModInstance { get; private set; } 

    //定義用於存放按鍵的變量
    public static ModKeybind HeavyAttack { get; private set; }
    public static ModKeybind Roll { get; private set; }
    public static ModKeybind Offhand { get; private set; }
    public static ModKeybind Crouch { get; private set; }

    //Bind函式用於簡化按鍵的宣告動作
    private static ModKeybind Bind(string name, Keys key)
    {
        return KeybindLoader.RegisterKeybind(ModInstance, name, key);
    }

    //加載按鍵資訊
    public static void Load(Mod mod)
    {
        ModInstance = mod;

        HeavyAttack = Bind("HeavyAttack", Keys.F);
        Roll = Bind("Roll", Keys.LeftShift);
        Offhand = Bind("Offhand",Keys.LeftAlt);
        Crouch = Bind("Crouch", Keys.LeftControl);
    }

    //卸載按鍵資訊
    public static void Unload()
    {
        HeavyAttack = null;
        Roll = null;
        Offhand = null;
        Crouch = null;
    }
}