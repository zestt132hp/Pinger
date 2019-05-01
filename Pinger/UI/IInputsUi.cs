using System;

namespace Pinger.UI
{
    interface IInputsUi
    {
        String[] ValuesFromUi();
        Boolean VerifyString(ref String inputStr, params String[] simbols);
    }
}
