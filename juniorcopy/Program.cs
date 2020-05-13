using System;
using System.Linq;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace juniorcopy
{
    // Console program which 
    // - reads from COM3: or COM4: and writes to ./"juniorRcv.txt" in "7+7 format"
    // OR: 
    // - reads a given file and writes its contents to COM3: or COM4:
    // OR:
    // - converts a text file in "7+7 format" to a binary file which then contains the original junior memory data.
    public class Program
    {
        static bool _continue;

        public static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, hier ist juniorcopy");
            if (!(args.Length == 0 || args.Length == 2 && args[0] == "-s" || args.Length == 2 && args[0] == "-772bin"))
            {
                Console.Error.WriteLine("Usage: juniorCopy [-s <file to send>] [-772bin <input text file> <output binary file>]");
                return;
            }

            SerialPort serialPort = null;
            if (args.Length == 0 || args[0] == "-s")
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length == 0)
                {
                    Console.Error.WriteLine("Keine seriellen Ports gefunden. Bye!");
                    return;
                }
                // else
                Console.Error.WriteLine("Ports: " + string.Join(",", ports));

                serialPort = OpenSerialPort(ports);
            }

            if (args.Length == 0)
                ReceiveFromJunior(serialPort);
            else if (args[0] == "-s")
                SendToJunior(serialPort, args[1]);
            else
                Convert772bin(serialPort, args[1]);

            serialPort?.Close();
            Console.Error.WriteLine("bye!");
        }

        // Create an open a new SerialPort object with default settings.
        private static SerialPort OpenSerialPort(string[] ports)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = ports[0];
            serialPort.BaudRate = 4800;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = 1000;

            try
            {
                serialPort.Open();
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine("Excep: {0}", ex);
                if (ports.Length > 1)
                {
                    // second chance
                    serialPort.PortName = ports[1];
                    serialPort.Open();
                }
            }
            return serialPort;
        }

        private static void SendToJunior(SerialPort port, string filename)
        {
            Console.Error.WriteLine("sending something to junior on {0}...", port.PortName);

            // text
            for (int i = 0; i < 10; i++)
            {
                port.WriteLine("Hi junior!!\r\n0000000055555555MMMMMMMM\r\n"); // 40 bytes incl \n aus !WriteLine"!
            }

            // binary! 14 bytes
            byte[] buffer = new byte[] { 0xA9, 0x07, 0x8D, 0x82, 0x1A, 0xA0, 0x1F, 0x84, 0xE2, 0x33, 0x00, 0xFF, 0xFF, 0x00 };
            port.Write(buffer, 0, buffer.Length);

            // file 48 bytes
            byte[] bytes = File.ReadAllBytes(filename);
            port.Write(bytes, 0, bytes.Length);

            Console.Beep();
            Thread.Sleep(1000);
        }

        private static void ReceiveFromJunior(SerialPort port)
        {
            Console.Error.WriteLine("receiving from junior on {0}...", port.PortName);

            StreamWriter sw = new StreamWriter("juniorRcv.txt", false);

            _continue = true;
            Task t = Task.Factory.StartNew(() => Read(port, sw));

            Console.Write("Enter Your Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter a message or Enter q to exit");

            while (_continue)
            {
                string message = Console.ReadLine();

                // send message to other side thru serial port (duplex!)
                if ("q" == message)
                    _continue = false;
                else
                    port.WriteLine(string.Format("<{0}>: {1}", name, message));
            }
            t.Wait();
            sw.Close();
        }

        private static void Read(SerialPort port, StreamWriter sw)
        {
            while (_continue)
            {
                try
                {
                    //string message = serialPort.ReadLine();
                    //Console.WriteLine(message);
                    int read = port.ReadChar();
                    Console.Write("{0:X2} ", read);
                    sw.Write("{0:X2} ", read);
                }
                catch (TimeoutException) {
                    Console.Write(".");
                }
            }
        }

        // read input text, convert each group like "54 29 " to an 8-bit-byte, and write it to output file
        private static void Convert772bin(SerialPort serialPort, string inputFileName)
        {
            string outputFileName = inputFileName + ".bin";
            using (StreamReader sr = new StreamReader(inputFileName))
            using (FileStream outFile = File.OpenWrite(outputFileName))
            {
                char[] buffer = new char[6]; // input format: like "54 29 "
                byte[] result = new byte[1]; // output format: one byte ("54 29 "-> 0xA9)
                int pos = 0;
                do
                {
                    int readCount = sr.Read(buffer, 0, 6);
                    if (readCount != 6)
                    {
                        Console.Error.WriteLine("conversion stopped at pos 0x{0:X}: readCount={1}", pos, readCount);
                        break;
                    }
                    pos++;

                    string hexValues = new string(buffer);
                    string[] hexValuesSplit = hexValues.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    if (hexValuesSplit.Length != 2 || hexValuesSplit[0].Length != 2 || hexValuesSplit[1].Length != 2)
                        Console.Error.WriteLine("pos 0x{0:X}: strange 6 bytes: {1}", pos, hexValues);

                    int[] values = hexValuesSplit.Select(hex => Convert.ToInt32(hex, 16)).ToArray();
                    int first = values[0];
                    int second = values[1];

                    // middle 6 bits must match!
                    if ((second >> 1) != (first & 0x3F))
                        Console.Error.WriteLine("pos 0x{0:X}: strange 6 bytes: {1}", pos, hexValues);

                    int combined = (first << 1) | second;
                    //Console.WriteLine("hex {0} -> int {1:X2} {2:X2} -> {3:X2}", hexValues, first, second, combined);
                    result[0] = (byte)combined;
                    outFile.Write(result, 0, 1);

                } while (true);
            } // using
        }
    }
}
