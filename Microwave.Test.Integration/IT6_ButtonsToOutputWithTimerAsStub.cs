using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Microwave.Test.Integration
{
    class IT6_ButtonsToOutputWithTimerAsStub
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
        StringWriter readConsole;

        [SetUp]
        public void Setup()
        {

            startCancelButton = new Button();
            powerButton = new Button();
            timeButton = new Button();
            door = new Door();

            fakeOutput = new Output();
            display = new Display(fakeOutput);
            light = new Light(fakeOutput);

            timer = new Timer();
            powerTube = new PowerTube(fakeOutput);

            cooker = new CookController(timer, display, powerTube);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
            cooker.UI = ui;
            readConsole = new StringWriter();
            Console.SetOut(readConsole);
        }

        [Test]
        public void Test6_1_1_PressPowerBut_AssertOnConsoleShowPower()
        {
            //Act (Is ind State =Ready)
            powerButton.Press();
            //Assert
            var text = readConsole.ToString();
            Assert.IsTrue(text.Contains("Display shows: 50 W"));

        }

        [Test]
        public void Test6_1_2_PressPowerBut_AssertOnConsoleShowPower()
        {
            //Act (Is ind State =Ready)
            powerButton.Press();
            powerButton.Press();
            powerButton.Press();
            //Assert
            var text = readConsole.ToString();
            Assert.IsTrue(text.Contains("Display shows: 150 W"));

        }
        [Test]
        public void Test6_2_PressTimeBut_AssertOnConsoleShowTime()
        {
            //Arrange - Need to be in state = SetPower
            powerButton.Press();

            //Act - Is in State = Set Power
            timeButton.Press();

            //Assert
            var text = readConsole.ToString();
            Assert.IsTrue(text.Contains("Display shows: 01:00"));

        }

        [Test]
        public void Test6_3_1_PressStartCancelBut_AssertOnConsoleTurnOnAnd50W()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime
            startCancelButton.Press();

            //Assert
            var text = readConsole.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(text.Contains("Light is turned on"));
                Assert.IsTrue(text.Contains("Display shows: 50 W"));
            });

        }
        [Test]
        public void Test6_3_2_PressStartCancelBut_AssertOnConsoleTurnOnAnd350W()
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
            var text = readConsole.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(text.Contains("Light is turned on"));
                Assert.IsTrue(text.Contains("Display shows: 350 W"));
            });

        }
        [Test]
        public void Test6_3_3_PressStartCancelBut_AssertOnConsoleTurnOnAnd750W()
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
            var text = readConsole.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(text.Contains("Light is turned on"));
                Assert.IsFalse(text.Contains("Display shows: 750 W"));
            });
        }

        [Test]
        public void Test6_4_PressStartCancelBut_AssertOnConsoleTurnOff()
        {
            //Arrange - Need to be in state = Cooking
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //Act - Is in State = Cooking
            startCancelButton.Press();

            //Assert
            var text = readConsole.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(text.Contains("PowerTube turned off"));
                Assert.IsTrue(text.Contains("Display cleared"));
                Assert.IsTrue(text.Contains("Light is turned off"));
            });
        }
    }
}

