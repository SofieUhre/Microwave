using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;

namespace Microwave.Test.Integration
{
    class IT4_DoorToUserInterface
    {
        private IDoor _sut;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;
        private IButton _fakePowerButton;
        private IButton _fakeTimeButton;
        private IButton _fakeStartCancelButton;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;
        private IUserInterface _userInterface;
       
        [SetUp]
        public void Setup()
        {
            // Definerer system under test
            _sut = new Door();

            //Definerer fakes
            _fakeTimer = Substitute.For<ITimer>();
            _fakeOutput = Substitute.For<IOutput>();
            _fakePowerButton = Substitute.For<IButton>();
            _fakeTimeButton = Substitute.For<IButton>();
            _fakeStartCancelButton = Substitute.For<IButton>();

            //Definerer de rigtige klasser der skal bruges 
            _powerTube = new PowerTube(_fakeOutput);
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);
            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _userInterface = new UserInterface(_fakePowerButton, _fakeTimeButton, _fakeStartCancelButton, _sut, _display, _light, _cookController);
        }

        [Test]
        public void OpenDoor_RaiseOpenedEvent_ExpectedOutputResult()
        {
            _sut.Open();

            //string outputResult = "Light is turned on";
           //_fakeOutput.Received(1).OutputLine(outputResult);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("on")));
        }

        [Test]
        public void CloseDoor_RaiseClosedEvent_ExpectedOutputResult()
        {
            _sut.Open();
            _sut.Close();
            
            //string outputResult = "Light is turned off";
            //_fakeOutput.Received(1).OutputLine(outputResult);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("off")));
        }

        [Test]
        public void OpenAndCloseDoor_RaiseEvents_ExpectedTwoReceives()
        {
            _sut.Open();
            _sut.Close();

            _fakeOutput.Received(2);
        }

        [Test]
        public void OpenDoorWhileCooking_RaiseOpenedEvents_ExpectedOutputResultFromPowerTube()
        {
            _sut.Open();
            _sut.Close();
            _fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeStartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.Open();

            string outputResult = "PowerTube turned off";
            _fakeOutput.Received(1).OutputLine(outputResult);
        }

        [Test]
        public void OpenDoorWhileCooking_RaiseOpenedEvents_ExpectedOutputResultFromDisplay()
        {
            _sut.Open();
            _sut.Close();
            _fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeStartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.Open();

            string outputResult = "Display cleared";
            _fakeOutput.Received(1).OutputLine(outputResult);
        }

        //Denne test virker ikke som forventet
        [Test]
        public void OpenDoorWhileCooking_RaiseOpenedEvents_ExpectedCountOfReceives()
        {
            _sut.Open();
            _sut.Close();
            _fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeStartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.Open();

            int countOfReceives = 8;

            _fakeOutput.Received(countOfReceives);
        }


    }
}
