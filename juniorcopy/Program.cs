using System;
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
        public static void Main(string[] args)
        {
            Console.Error.WriteLine("Hi, i'm juniorcopy");
            if (!(args.Length == 0 || args.Length == 2 && args[0] == "-s"))
            {
                Console.Error.WriteLine("Usage: juniorCopy [-s <file to send>]");
                return;
            }

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                Console.Error.WriteLine("No serial ports found. Bye!");
                return;
            }
            // else
            Console.Error.WriteLine("Ports: " + string.Join(",", ports));

            SerialPort serialPort = OpenSerialPort(ports);

            if (args.Length == 0)
                ReceiveFromJunior(serialPort);
            else
                SendToJunior(serialPort, args[1]);

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
            serialPort.StopBits = StopBits.Two;
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
            Console.Error.WriteLine("sending contents of file {0} to junior on {1}...", filename, port.PortName);

            byte[] bytes = File.ReadAllBytes(filename);
            port.Write(bytes, 0, bytes.Length);

            Console.Beep();
            Thread.Sleep(1000);
        }

        private static void ReceiveFromJunior(SerialPort port)
        {
            Console.Error.WriteLine("receiving from junior on {0}...", port.PortName);

            const string outputFileName = "juniorRcv.bin";
            using (FileStream outFile = File.OpenWrite(outputFileName))
            {
                var combiner = new ByteCombiner(outFile);
                try
                {
                    using (CancellationTokenSource cts = new CancellationTokenSource())
                    {
                        Task t = Task.Factory.StartNew(() => Read(port, combiner, cts.Token));

                        Console.Error.WriteLine("junior may start sending now!");
                        Console.Error.WriteLine("press ENTER to finish conversion and exit - after transmission has completed.");
                        Console.ReadLine();
                        cts.Cancel();
                        t.Wait();
                    }
                }
                finally
                {
                    combiner.OnCompleted();
                }
            }
            Console.Error.WriteLine("created binary file {0}", outputFileName);
        }

        private static void Read(SerialPort port, IObserver<byte> consumer, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    byte read = (byte)port.ReadChar();
                    Console.Write("{0:X2} ", read);
                    consumer.OnNext(read);
                }
                catch (TimeoutException) {
                    Console.Write(".");
                }
            }
        }
    }
}
