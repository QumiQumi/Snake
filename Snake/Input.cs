using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        //загружаем лист доступных клавиш на клаве
        private static Hashtable KeyTable = new Hashtable();

        //выполняем проверку нажата ли отдельная клавиша
        public static bool KeyPressed(Keys key)
        {
            if (KeyTable[key] == null)
            {
                return false;
            }
            return (bool) KeyTable[key];
        }

        //обнаруживает нажатие клавиши
        public static void ChangeState (Keys key, bool state)
        {
            KeyTable[key] = state;
            
        }
    }
}
