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
            // System under test
            _sut = new Door();

            // Fakes
            _fakeTimer = Substitute.For<ITimer>();
            _fakeOutput = Substitute.For<IOutput>();
            _fakePowerButton = Substitute.For<IButton>();
            _fakeTimeButton = Substitute.For<IButton>();
            _fakeStartCancelButton = Substitute.For<IButton>();

            // Rigtige klasser der skal bruges 
            _powerTube = new PowerTube(_fakeOutput);
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);
            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _userInterface = new UserInterface(_fakePowerButton, _fakeTimeButton, _fakeStartCancelButton, _sut, _display, _light, _cookController);
        }

        //Døren åbnes - der testes på forventet output
        [Test]
        public void D1_OpenDoor_RaiseOpenedEvent_ExpectedOutputResult()
        {
            //Act
            _sut.Open();

            //Assert
            string outputResult = "Light is turned on";
            _fakeOutput.Received(1).OutputLine(outputResult);

            //_fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("on")));
        }

        //Døren åbnes, hvorefter den lukkes - der testes på forventet output
        [Test]
        public void D2_CloseDoor_RaiseClosedEvent_ExpectedOutputResult()
        {
            //Act
            _sut.Open();
            _sut.Close();

            //Assert
            string outputResult = "Light is turned off";
            _fakeOutput.Received(1).OutputLine(outputResult);

            //_fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("off")));
        }

        //Døren åbnes, hvorefter den lukkes - der testes på at der modtages to output
        [Test]
        public void D3_OpenAndCloseDoor_RaiseEvents_ExpectedTwoReceives()
        {
            //Act
            _sut.Open();
            _sut.Close();

            //Assert
            _fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Light")));
        }

        //Døren åbnes, hvorefter den lukkes. Døren åbnes mens den er i cooking state - der testes på forventet output fra PowerTube
        [Test]
        public void D4_OpenDoorWhileCooking_RaiseOpenedEvents_ExpectedOutputResultFromPowerTube()
        {
            //Act
            _sut.Open();
            _sut.Close();
            _fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeStartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.Open();

            //Assert
            string outputResult = "PowerTube turned off";
            _fakeOutput.Received(1).OutputLine(outputResult);
        }

        //Døren åbnes, hvorefter den lukkes. Døren åbnes mens den er i cooking state - der testes på forventet output fra Display
        [Test]
        public void D5_OpenDoorWhileCooking_RaiseOpenedEvents_ExpectedOutputResultFromDisplay()
        {
            //Act
            _sut.Open();
            _sut.Close();
            _fakePowerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeTimeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _fakeStartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.Open();

            //Assert
            string outputResult = "Display cleared";
            _fakeOutput.Received(1).OutputLine(outputResult);
        }

    }
}
