﻿using System;
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
            //fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Display shows:"))); // + Arg.Any<int>() + ":" + Arg.Any<int>()

            // En anden version
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:01"))); 
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:00"))); 
        }

        [Test]
        public void CC2_CoockontrollerTimer_StartCoocking_poweryueIsTurnedOff()
        {
            SUT.StartCooking(50,2);
            Thread.Sleep(2500);
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void CC3_CoockontrollerTimer_StopCoocking_powerIsTurnedOff()
        {
            //Her skal vi prøve at stoppe, og så vi venter et sekund eller måske 2 og ser, at der IKKE kommer et tick (.DidNotReceive)
            SUT.StartCooking(50, 10);
            Thread.Sleep(1500); //Venter 1½ sekund, forventer derfor at modtage ET kald til outputline
            SUT.Stop();
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
            Thread.Sleep(2500); //Venter til der der gået yderligere 2½ sekund
            fakeOutput.DidNotReceive().OutputLine(Arg.Is<string>(s => s.Contains("08"))); //Der er i alt gået 4 sekunder, så der BØR være modtaget et kald med 00:08, hvis ikke stp virker
        }
        

        #endregion


        #region Til powertube

        // Denne her tester integrationen mellem coockcontroller og Powertube
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(99)]
        public void CC4_CoockontrollerPowerTube_StartCoocking_IsPoweryubeTurnedOn(int power)
        {
            SUT.StartCooking(power, 2);

            fakeOutput.Received(1).OutputLine("PowerTube works with " + power);
        }


        [TestCase(1)]
        [TestCase(50)]
        [TestCase(99)]
        public void CC4_CoockontrollerPowerTube_timeExpires_IsPoweryubeTurnedOff(int power)
        {
            SUT.StartCooking(power, 2);
            Thread.Sleep(2200);
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }

        //SEriøst.. Den her test er lidt ubrulig, fordi vi teknisk set har testet det lige ovenfor?
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(99)]
        public void CC5_CoockontrollerPowerTube_StopCoocking_PoweryubeTurnedOff(int power)
        {

            //Her skal vi prøve at stoppe, og så vi venter et sekund eller måske 2 og ser, at der IKKE kommer et tick (.DidNotReceive)
            SUT.StartCooking(power, 10);
            Thread.Sleep(1500); //Venter 1½ sekund for at sikre, at vi er igang
            SUT.Stop();
            fakeOutput.Received(1).OutputLine("PowerTube turned off");
            Thread.Sleep(2500); //Venter til der der gået yderligere 2½ sekund
            fakeOutput.DidNotReceive().OutputLine(Arg.Is<string>(s => s.Contains("08"))); //Tester, at vi ikkeb modtager andet
        }

        #endregion

        #region Display
        //Igen - Den her test giver INGEN mening, fordi jeg tester det på NØJAGTIG samme måde som ovenfor i CC1
        [Test]
        public void CC6_CoockontrollerDisplay_StartCoocking_OutputRecievesCallFromDisplay()
        {

            SUT.StartCooking(50, 3);
            Thread.Sleep(3200); //Venter 1½ sekund for at sikre, at vi er igang
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:02")));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:01")));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 00:00")));
        }


        #endregion


    }
}