using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace juniorcopy
{
    // Console program which 
    // - reads from COM3: or COM4: and writes to ./juniorReceived.bin (not yet)
    // OR: 
    // - reads a given file and writes its contents to COM3: or COM4:
    public class PortChat
    {
        static bool _continue;

        public static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, hier ist juniorcopy");
            if (!(args.Length == 0 || args.Length == 2 && args[0] == "-s"))
            {
                Console.Error.WriteLine("Usage: juniorCopy [-s <file to send>]");
                return;
            }

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                Console.Error.WriteLine("Keine seriellen Ports gefunden. Bye!");
                return;
            }
            // else
            Console.Error.WriteLine("Ports: " + string.Join(",", ports));

            SerialPort serialPort = OpenSerialPort(ports);

            if (args.Length == 0)
                ReceiveFromJunior(serialPort);
            else
                SendToJunior(serialPort, args[1]);

            serialPort.Close();
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

        public static void ReceiveFromJunior(SerialPort port)
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

        public static void Read(SerialPort port, StreamWriter sw)
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

        // Display Port values and prompt user to enter a port.
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        // Display BaudRate values and prompt user to enter a value.
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        // Display PortParity values and prompt user to enter a value.
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }
        // Display DataBits values and prompt user to enter a value.
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        // Display StopBits values and prompt user to enter a value.
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter StopBits value (None is not supported and \n" +
             "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
    }
}
