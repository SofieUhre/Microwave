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
        public void CC4_CoockontrollerTimer_StopCoocking_powerIsTurnedOff()
        {
            //Her skal du prøve at stoppe, og så skal du vente et sekund eller måske 2 og se at der IKKE kommer et tick (.DidNotRecieve)
            //SUT.StartCooking(50, 2);
            //Thread.Sleep(2500);
            //fakeOutput.Received(1).OutputLine("PowerTube turned off");
        }


        #region Til powertube

        // Denne her tester integrationen mellem coockcontroller og Powertube
        [Test]
        public void CC1_CoockontrollerTimer_StartCoocking_IsPoweryubeTurnedOn()
        {
            SUT.StartCooking(50, 2);

            fakeOutput.Received(1).OutputLine("PowerTube works with 50");
        }

        #endregion




    }
}
