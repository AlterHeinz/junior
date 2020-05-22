
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestAssembler
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void SingleByteOpPHAViaStdInYieldsPHA()
        {
            string output = Transform("48");
            Assert.AreEqual("PHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void SingleByteOpPHAViaFileYieldsPHA()
        {
            string output = Transform(0x48);
            Assert.AreEqual("PHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void SingleByteOpPHAViaFileVerboseYieldsPHA()
        {
            string output = TransformVerbose("0080", 0x48);
            Assert.AreEqual("0080: 48     PHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void DualByteOpLDAimViaStdInYieldsLDAim()
        {
            string output = Transform("A97F");
            Assert.AreEqual("LDAim 7F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void BranchOpViaStdInYieldsNormalDestination()
        {
            string output = Transform("D007");
            Assert.AreEqual("BNE 07\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void DualByteOpLDAimViaStdInVerboseYieldsLDAim()
        {
            string output = Transform("A97F", true, "0700");
            Assert.AreEqual("0700: A97F   LDAim 7F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void BranchOpForwardViaStdInVerboseYieldsVerboseBranchDestination()
        {
            string output = Transform("D007", true, "0727");
            Assert.AreEqual("0727: D007   BNE +7>30\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void BranchOpBackwardViaStdInVerboseYieldsVerboseBranchDestination()
        {
            string output = Transform("F0E7", true, "0727");
            Assert.AreEqual("0727: F0E7   BEQ -25>10\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void TripleByteOpLDAViaStdInYieldsLDA()
        {
            string output = Transform("AD7F03");
            Assert.AreEqual("LDA 037F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void TripleByteOpLDAViaStdInVerboseYieldsLDA()
        {
            string output = Transform("AD7F03", true, "FF80");
            Assert.AreEqual("FF80: AD7F03 LDA 037F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void TwoOpsViaStdInYieldsLDAPHA()
        {
            string output = Transform("AD7F0348");
            Assert.AreEqual("LDA 037F\r\nPHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void TwoOpsViaFileYieldsLDAPHA()
        {
            string output = Transform(0xAD, 0x7F, 0x03, 0x48);
            Assert.AreEqual("LDA 037F\r\nPHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void TwoOpsViaFileVerboseYieldsLDAPHAetc()
        {
            string output = TransformVerbose("0000", 0xAD, 0x7F, 0x03, 0x48);
            Assert.AreEqual("0000: AD7F03 LDA 037F\r\n0003: 48     PHA\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void SingleValidCharYieldsNothing()
        {
            string output = Transform("A");
            Assert.AreEqual("", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void LDAWithOnly3CharsFollowingYieldsQuestionMarks()
        {
            string output = Transform("AD7F0");
            Assert.AreEqual("LDA ??7F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void LDAWithOnly2CharsFollowingYieldsQuestionMarks()
        {
            string output = Transform("AD7F");
            Assert.AreEqual("LDA ??7F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void LDAWithOnly1ByteFollowingYieldsQuestionMarks()
        {
            string output = Transform(0xAD, 0x7F);
            Assert.AreEqual("LDA ??7F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void LDAWith0ByteFollowingYieldsQuestionMarks()
        {
            string output = Transform(0xAD);
            Assert.AreEqual("LDA ????\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void LDAzWith0ByteFollowingYieldsQuestionMarks()
        {
            string output = Transform(0xA5);
            Assert.AreEqual("LDAz ??\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void UnknownOpcodeYieldsEmptyLine()
        {
            string output = Transform("33");
            Assert.AreEqual("\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        public void InvalidOpAndDualByteOpSTAzViaStdInYieldsEmptyLinePlusSTAz()
        {
            string output = Transform("22858F");
            Assert.AreEqual("\r\nSTAz 8F\r\n", output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        [DeploymentItem(@"juniorEprom1C00.bin")]
        [DeploymentItem(@"TestFiles\juniorEprom1C00_expected.txt")]
        public void Disassemble1C00DumpYieldsExpectedText()
        {
            string output = TransformFile("juniorEprom1C00.bin");
            string expected = File.ReadAllText("juniorEprom1C00_expected.txt");
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        [DeploymentItem(@"juniorassembler.exe")]
        [DeploymentItem(@"juniorEprom1C00.bin")]
        [DeploymentItem(@"TestFiles\juniorEprom1C00_Verbose_expected.txt")]
        public void Disassemble1C00DumpVerboseYieldsExpectedText()
        {
            string output = TransformFile("juniorEprom1C00.bin", true, "1C00");
            string expected = File.ReadAllText("juniorEprom1C00_Verbose_expected.txt");
            Assert.AreEqual(expected, output);
        }

        private static string Transform(string machineCodeBytes, bool verbose = false, string startAddr = "")
        {
            Process testling = ConfigureAndStartProcess("", verbose, startAddr, true);

            // push data into StdIn
            testling.StandardInput.Write(machineCodeBytes);
            testling.StandardInput.Close();

            return ReadOutputDataAndStopProcess(testling);
        }

        private static string Transform(params byte[] machineCodeBytes)
        {
            File.WriteAllBytes("tempHexData.bin", machineCodeBytes);
            return TransformFile("tempHexData.bin");
        }

        private static string TransformVerbose(string startAddr, params byte[] machineCodeBytes)
        {
            File.WriteAllBytes("tempHexData.bin", machineCodeBytes);
            return TransformFile("tempHexData.bin", true, startAddr);
        }

        private static string TransformFile(string filename, bool verbose = false, string startAddr = "")
        {
            Process testling = ConfigureAndStartProcess(filename, verbose, startAddr, false);
            return ReadOutputDataAndStopProcess(testling);
        }

        private static Process ConfigureAndStartProcess(string filename, bool verbose, string startAddr, bool redirectStdIn)
        {
            // configure and start process
            ProcessStartInfo psi = new ProcessStartInfo("juniorassembler", (verbose ? "-dv " + startAddr + " " : "-d ") + filename);
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = redirectStdIn;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            Process testling = Process.Start(psi);
            testling.EnableRaisingEvents = true;
            testling.Exited += Testling_Exited;
            testling.OutputDataReceived += Testling_OutputDataReceived;
            testling.ErrorDataReceived += Testling_ErrorDataReceived;
            return testling;
        }

        private static string ReadOutputDataAndStopProcess(Process testling)
        {
            Task<string> error = Task.Factory.StartNew(() => testling.StandardError.ReadToEnd());
            Task<string> output = Task.Factory.StartNew(() => testling.StandardOutput.ReadToEnd());
            testling.WaitForExit();
            Console.WriteLine("WaitForExit()");
            Thread.Sleep(100);

            Console.WriteLine("err: {0}", error.Result);
            Console.WriteLine("out: {0}", output.Result);
            return output.Result;
        }

        private static void Testling_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("ErrorDataReceived");
        }

        private static void Testling_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("OutputDataReceived");
        }

        private static void Testling_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine("juniorassembler has exited");
        }
    }
}
