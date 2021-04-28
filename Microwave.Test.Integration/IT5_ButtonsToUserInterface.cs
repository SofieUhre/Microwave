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
            UserInterface ui;
            Button timeButton;
            Button startCancelButton;
            Button powerButton;
            IDoor door;
            IDisplay display;
            ILight light;

        [SetUp]
        public void Setup()
        {
            
            startCancelButton = new Button();
            powerButton = new Button();
            timeButton = new Button();
            door = Substitute.For<IDoor>();
            display = Substitute.For<IDisplay>();
            light = Substitute.For<ILight>();

            ICookController cooker = Substitute.For<ICookController>();
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
        }

        [Test]
        public void Test5_1_PressPowerBut_AssertShowPower()
        {
            //Act (Is ind State =Ready)
            powerButton.Press();

            //Assert
            display.Received(1).ShowPower(50);
        }

        [Test]
        public void Test5_2_PressTimeBut_AssertShowTime()
        {
            //Arrange - Need to be in state = SetPower
            powerButton.Press();

            //Act - Is in State = Set Power
            timeButton.Press();

            //Assert
            display.Received(1).ShowTime(1, 0);

        }

        [Test]
        public void Test5_3_PressStartCancelBut_AssertLigthOn()
        {
            //Arrange - Need to be in state = SetTime
            powerButton.Press();
            timeButton.Press();

            //Act - Is in State = SetTime
            startCancelButton.Press();

            //Assert
            light.Received(1).TurnOn();

        }
    }
}
