using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortDataReceived
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort mySerialPort = new SerialPort("COM4");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }
        private static void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(decode_ussd_message(indata+ Environment.NewLine));
            Console.WriteLine("--End--");
        }
        private static string decode_ussd_message(string message)
        {
            string[] array = message.Split(new char[] { '"' });
            string text = array[0].ToString();
            try
            {

                char[] array2 = array[1].ToCharArray();
                int num = array2.Length;
                for (int i = 0; i < num; i++)
                {
                    if (array2[i] > '9')
                    {
                        char[] expr_3F_cp_0 = array2;
                        int expr_3F_cp_1 = i;
                        expr_3F_cp_0[expr_3F_cp_1] -= '7';
                    }
                    else
                    {
                        char[] expr_58_cp_0 = array2;
                        int expr_58_cp_1 = i;
                        expr_58_cp_0[expr_58_cp_1] -= '0';
                    }
                }
                for (int j = 0; j < num; j += 4)
                {
                    int num2 = (int)(array2[j] * '\u0010' + array2[j + 1]);
                    num2 *= 256;
                    int num3 = (int)(array2[j + 2] * '\u0010' + array2[j + 3]);
                    text += (char)(num2 + num3);
                }

            }
            catch (Exception e)
            {
                return text += e.Message;
            }
            return text;
        }
    }
}
