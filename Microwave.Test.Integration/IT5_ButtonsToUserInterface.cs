using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Microwave.Test.Integration
{
    public class IT5_ButtonsToUserInterface
    {
        IUserInterface ui;
        IButton powerButton;
        IButton timeButton;
        IButton startCancelButton;
        IDoor door;
        IDisplay display;
        ILight light;
        IOutput fakeOutput;
        ITimer timer;
        IPowerTube powerTube;
        ICookController cooker;


        [SetUp]
        public void Setup()
        {

            startCancelButton = new Button();
            powerButton = new Button();
            timeButton = new Button();
            door = new Door();

            fakeOutput = Substitute.For<IOutput>();
            display = new Display(fakeOutput);
            light = new Light(fakeOutput);

            timer = new Timer();
            powerTube = new PowerTube(fakeOutput);

            cooker = new CookController(timer, display, powerTube);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
            cooker.UI = ui;
        }

        [Test]
        public void Test5_1_1_PressPowerBut_AssertOnOutputLineShowPower()
        {
            //Act (Is ind State =Ready)
            powerButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display shows: 50 W"));

        }

        [Test]
        public void Test5_1_2_PressPowerBut_AssertOnOutputLineShowPower()
        {
            //Act (Is ind State =Ready)
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display shows: 150 W"));

        }
        [Test]
        public void Test5_2_PressTimeBut_AssertOnOutputLineShowTime()
        {
            //Arrange - Need to be in state = SetPower
            powerButton.Press();

            //Act - Is in State = Set Power
            timeButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display shows: 01:00"));

        }

        [Test]
        public void Test5_3_1_PressStartCancelBut_AssertOnOutputLineTurnOnAnd50W()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime
            startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Light is turned on"));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display shows: 50 W"));

        }
        [Test]
        public void Test5_3_2_PressStartCancelBut_AssertOnOutputLineTurnOnAnd350W()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime
            startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Light is turned on"));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display shows: 350 W"));

        }
        [Test]
        public void Test5_3_3_PressStartCancelBut_AssertOnOutputLineTurnOnAnd750W()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime
            startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Light is turned on"));
            fakeOutput.Received(0).OutputLine(Arg.Is<string>("Display shows: 750 W"));
            fakeOutput.Received(2).OutputLine(Arg.Is<string>("Display shows: 50 W"));

        }

        [Test]
        public void Test5_4_PressStartCancelBut_AssertOnOutputLineTurnOff()
        {
            //Arrange - Need to be in state = Cooking
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //Act - Is in State = Cooking
            startCancelButton.Press();

            //Assert
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("PowerTube turned off"));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Display cleared"));
            fakeOutput.Received(1).OutputLine(Arg.Is<string>("Light is turned off"));
        }
    }
}
