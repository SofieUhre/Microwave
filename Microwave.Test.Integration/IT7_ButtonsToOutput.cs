using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class IT7_ButtonsToOutput
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
        public void Test7_ButtonOutput_StartAt50WAnd1minWait62sek_AssertOnConsoleLightTurnedOff()
        {
            //Arrange - Need to be in state = Cooking
            powerButton.Press(); //50W
            timeButton.Press(); //1min
            startCancelButton.Press(); //Start

            //Act
            System.Threading.Thread.Sleep(62000);

            //Assert
            var text = readConsole.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(text.Contains("Display shows: 00:59"));
                Assert.IsTrue(text.Contains("Display shows: 00:00"));
                Assert.IsTrue(text.Contains("Light is turned off"));
            });
        }
    }
}
