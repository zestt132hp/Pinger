using System;
using Ninject.Infrastructure.Language;

namespace Pinger.UI
{
    public class ConsoleInputData : IInputsUi
    {
        public String[] ValuesFromUi()
        {
            return Console.ReadLine()?.Split(' ');
        }

        public Boolean VerifyString(ref string command, params string[] simbols)
        {
            string tmp = command;
            if (String.IsNullOrEmpty(tmp))
                return false;
            simbols.Map(x =>
            {
                if (tmp.Contains(x))
                    tmp = tmp.Remove(tmp.IndexOf(x, StringComparison.Ordinal), 1);
            });
            if (VerifyString(tmp))
            {
                command = tmp;
                return true;
            }
            else
                return false;
        }

        private Boolean VerifyString(String str)
        {
            string[] tmp = str.Split(' ');
            int itmp;
            if(tmp.Length<2 && Int32.TryParse(tmp[0], out itmp))
                return true;
            else
                return tmp.Length > 2;
        }
    }
    interface IInputsUi
    {
        String[] ValuesFromUi();
        Boolean VerifyString(ref String inputStr, params String[] simbols);
    }
}
