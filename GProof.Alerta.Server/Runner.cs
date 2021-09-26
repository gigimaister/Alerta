using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using GProof.Alerta.Server.Entities;
using GProof.Alerta.Server.Helpers;

namespace GProof.Alerta.Server
{
    public class Runner
    {
        private readonly PikudHaOrefConnector _pikudHaOrefConnector;
        private readonly AlertParser _alertParser;
        private const int IntervalCheckTimeMilliseconds = 100;
        private static readonly List<AlertInfoExt> Alerts = new List<AlertInfoExt>();
        private static readonly List<long> AlertsIds = new List<long>();

        public Runner()
        {
            _pikudHaOrefConnector = new PikudHaOrefConnector();
            _alertParser = new AlertParser();
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string alertsResponseAsJson = _pikudHaOrefConnector.RetrieveAlert();

                    if (!string.IsNullOrEmpty(alertsResponseAsJson))
                    {
                        // PlaySound();

                        List<AlertInfo> retrievedAlerts = _alertParser.Parse("[" + alertsResponseAsJson + "]");

                        if (retrievedAlerts == null)
                        {
                            continue;
                        }

                        DateTime dateTimeNoe = DateTime.Now;

                        List<AlertInfoExt> newAlerts = retrievedAlerts
                            .Select(alert => new AlertInfoExt(alert, dateTimeNoe))
                            .Where(alert => !AlertsIds.Contains(alert.Id)).ToList();

                        if (newAlerts.Count <= 0)
                        {
                            Thread.Sleep(IntervalCheckTimeMilliseconds);
                            continue;
                        }

                        //PlaySound();
                        Alerts.AddRange(newAlerts);
                        AlertsIds.AddRange(newAlerts.Select(a => a.Id));
                        newAlerts.OrderBy(x => x.EventTime).ToList()
                            .ForEach(alert => Console.WriteLine(alert.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now} - {ex}");
                }

                Thread.Sleep(IntervalCheckTimeMilliseconds);
            }
        }
     
        private static void PlaySound()
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak("Missile Attack");
        }
    }
}
