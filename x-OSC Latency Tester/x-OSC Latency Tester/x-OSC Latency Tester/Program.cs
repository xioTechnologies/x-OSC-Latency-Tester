using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Threading;
using Rug.Osc;
using System.Net;

namespace x_OSC_Latency_Tester
{
    class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">
        /// Unused.
        /// </param>
        static void Main(string[] args)
        {
            OscNamespaceManager oscNamespaceManager;
            OscReceiver oscReceiver;
            Thread thread;
            OscSender oscSender;

            // Print application name
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());

            // Log measured pulse width to file 
            TTi_TF930 tf930 = new TTi_TF930("COM3");    // hardcoded TTi TF930 serial port name
            tf930.MeasurementReceived += new TTi_TF930.onMeasurementReceived(delegate(object sender, double measuremnt)
            {
                StreamWriter streamWriter = new StreamWriter("data.csv", true);
                streamWriter.WriteLine(measuremnt.ToString(CultureInfo.InvariantCulture));
                streamWriter.Close();
                Console.WriteLine(measuremnt.ToString(CultureInfo.InvariantCulture));
            });
            tf930.Open();

            // Create behaviour: Output 1 = Input 1
            oscNamespaceManager = new OscNamespaceManager();
            oscReceiver = new OscReceiver(8000);
            oscSender = new OscSender(IPAddress.Parse("169.254.1.1"), 9000);    // hardcoded x-OSC IP address
            thread = new Thread(new ThreadStart(delegate()
            {
                while (oscReceiver.State != OscSocketState.Closed)
                {
                    if (oscReceiver.State == OscSocketState.Connected)
                    {
                        OscPacket packet = oscReceiver.Receive();
                        if (oscNamespaceManager.ShouldInvoke(packet) == OscPacketInvokeAction.Invoke)
                        {
                            oscNamespaceManager.Invoke(packet);
                        }
                    }
                }
            }));
            oscNamespaceManager.Attach("/inputs/digital", delegate(OscMessage message)
            {
                oscSender.Send(new OscMessage("/outputs/digital/state/1", (int)message[0]));
            });
            oscReceiver.Connect();
            oscSender.Connect();
            thread.Start();

            // Run forever!
            while (true) ;
        }
    }
}
