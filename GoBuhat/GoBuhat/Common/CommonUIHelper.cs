using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace GoBuhat.Common
{
    public static class CommonUIHelper
    {
        public static void CreateToast(string message)
        {

            // TODO : remake this shit 

            DependencyService.Get<IMessage>().Longtime(message);

            Console.WriteLine(message);
        }

        public static string ConvertResult(byte[] result)
        {
            return System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
        }
    }
}
