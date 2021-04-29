using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NSubstitute;
using Microwave.Classes;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute.Core.Arguments;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    public class IT1_CookControllerToDisplayPowerTubeTimer
    {
        private CookController SUT;
        private IUserInterface fakeUI;
        private ITimer timer;
        private IPowerTube powerTube;
        private IDisplay display;
        private IOutput fakeOutput;

        [SetUp]
        public void Setup()
        {
            //Opretter mine fakes
            fakeUI = Substitute.For<IUserInterface>();
            fakeOutput = Substitute.For<IOutput>();

            //Opretter de objekter hvis interfaces skal testes
            timer = new Timer();
            display = new Display(fakeOutput);
            powerTube = new PowerTube(fakeOutput);
            SUT = new CookController(timer, display, powerTube, fakeUI); //Der er to typer cunstructor til SUT, jeg bruger den MED UI

        }

        #region Intg mellem coockcontroller og timer

        //For at få et respons på at jeg har startet tiden skal jeg vente til denne er udløbet, og registrere, at der bliver skrevet en outputlinje
        [Test]
        public void CC1_CoockontrollerTimer_StartCoocking_IsTimerStarted()
        {
            SUT.StartCooking(50, 2);
            Thread.Sleep(2200);

            // En anden version
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:01"))); 
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:00"))); 
        }

        //Tester at timeren ikke sender flere ticks når tiden er udløbet, altså efter 2 sekunder
        [Test]
        public void CC2_CoockontrollerTimer_StartCoockingTimeExpire_powertueIsTurnedOff()
        {
            SUT.StartCooking(50,2);
            Thread.Sleep(2200);
            fakeOutput.Received(1).OutputLine("PowerTube turned off"); //Verificere at tiden udløb
            Thread.Sleep(1000); // Venter yderligere et sekund og tester, at veruficere at min timer rent fakktisk stopper når tiden udløber
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:0")));
        }

        [Test]
        public void CC3_CoockontrollerTimer_StopCoocking_powerIsTurnedOff()
        {
            //Her skal vi prøve at stoppe. Derefter venter vi et sekund eller måske 2 og ser, at der IKKE kommer et tick (.DidNotReceive)
            SUT.StartCooking(50, 10);
            Thread.Sleep(1500); //Venter 1½ sekund, forventer derfor at modtage ET kald til outputline
            SUT.Stop();
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
            Thread.Sleep(2500); //Venter til der der gået yderligere 2½ sekund
            fakeOutput.DidNotReceive().OutputLine(Arg.Is<string>(s => s.Contains("08"))); //Der er i alt gået 4 sekunder, så der BØR være modtaget et kald med 00:08, hvis ikke stp virker
        }


        #endregion


        #region Integrationstest mellem Cookcontroller og powertube

        // Denne her tester integrationen mellem coockcontroller og Powertube
        // Ud fra Usecasen forventer jeg, at mit userinterface sender 50 - 700 W med. Jeg tester altså 50, 700 og 2 ind imellem
        [TestCase(50)]
        [TestCase(550)]
        [TestCase(700)]
        public void CC4_CoockontrollerPowerTube_StartCoocking_IsPowertubeTurnedOn(int power)
        {
            SUT.StartCooking(power, 2);

            fakeOutput.Received(1).OutputLine("PowerTube works with " + power);
        }

        [TestCase(50)]
        [TestCase(550)]
        [TestCase(700)]
        public void CC4_CoockontrollerPowerTube_timeExpires_IsPoweryubeTurnedOff(int power)
        {
            SUT.StartCooking(power, 2);
            Thread.Sleep(2200);
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }

        #endregion

        #region Display
        //Tester displayet kan tælle ned fra 3 sekunder og derudfra vise det rigtige (lidt en overflødig test, fordi det er det samme som CC1)
        [Test]
        public void CC6_CoockontrollerDisplay_StartCoocking_OutputRecievesCallFromDisplay()
        {

            SUT.StartCooking(50, 3);
            Thread.Sleep(3200); //Venter 1½ sekund for at sikre, at vi er igang
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:02")));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:01")));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:00")));
        }

        //Tester displayet kan omregne forskellige antal sekunder til det rigtige output
        [TestCase(121,02,00)]
        [TestCase(115, 01, 54)]
        [TestCase(60, 00, 59)]
        [TestCase(5940, 98, 59)]
        [TestCase(6000, 99, 59)] // Tester hvad der sker, hvis jeg sender for mange sekunder ind, altså 100 minutter frem for 99. 
        [TestCase(6002, 00, 01)] // Tester hvad der sker, hvis jeg sender for mange sekunder ind, altså 6002 sekunder frem. 
        public void CC7_CoockontrollerDisplay_StartCoocking_OutputRecievesCallFromDisplayWithCorrectMinutes(int time_s, int time_in_min, int time_in_sek)
        {
            SUT.StartCooking(50, time_s);
            Thread.Sleep(1100); //Venter 1.2 sekund for at sikre, at vi er forbi det første tick, men ikke det næste
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows:") && s.Contains(time_in_min.ToString()) && s.Contains(":") && s.Contains(time_in_sek.ToString())));
        }

        #endregion


    }
}
