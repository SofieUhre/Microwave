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

            timer = Substitute.For<ITimer>();
            powerTube = new PowerTube(fakeOutput);

            cooker = new CookController(timer, display, powerTube);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
            cooker.UI = ui;
            readConsole = new StringWriter();
            Console.SetOut(readConsole);
        }

        /// <summary>
        /// Tester at light har forbindelse til Output
        /// </summary>
        [Test]
        public void Test1_FromLight_ZeroLogLine_ToOutput()
        {
            //Assert
            var text = readConsole.ToString();
            Assert.IsFalse(text.Contains("Light is turned on"));
        }
       [Test]
        public void Test1_FromLight_LogLine_ToOutput()
        {
            //Act - Is in State = Ready => State = DoorOpen
            door.Open();

            //Assert
            var text = readConsole.ToString();
            Assert.IsTrue(text.Contains("Light is turned on"));
        }

        /// <summary>
        /// Tester at Display har forbindelse til Output
        /// </summary>
        [Test]
        public void Test2_FremDisplay_ZeroLogLine_ToConsole()
        {
            //Act (Is ind State = Ready => State = SetPower) 
           // powerButton.Press();

            //Assert
            var text = readConsole.ToString();
            Assert.IsFalse(text.Contains("Display shows: 50 W"));
        }
        
        [Test]
        public void Test2_FromDisplay_LogLine_ToConsole()
        {
            //Act (Is ind State = Ready => State = SetPower) 
            powerButton.Press();

            //Assert
            var text = readConsole.ToString();
            Assert.IsTrue(text.Contains("Display shows: 50 W"));
        }
        /// <summary>
        /// Tester at PowerTube har forbindelse til Output
        /// </summary>
        [Test]
        public void Test3_FromPowerTube_ZeroLogLine_ToConsole()
        {
            //Assert
            var text = readConsole.ToString();

            Assert.IsFalse(text.Contains("PowerTube works with 50"));
        }
        /// <summary>
        /// Tester at PowerTube har forbindelse til Output
        /// </summary>
        [Test]
        public void Test3_FromPowerTube_LogLine_ToConsole()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime => State = Cooking
            startCancelButton.Press();

            //Assert
            var text = readConsole.ToString();

            Assert.IsTrue(text.Contains("PowerTube works with 50"));


        }

    }
}

