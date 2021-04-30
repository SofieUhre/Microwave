using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT3_UserInterfaceToCookController
    {

        private ITimer timer;
        private IPowerTube powerTube;
        private IDisplay display;
        private ILight light;
        private ICookController cookController;



        private IUserInterface userInterface;

        private IOutput fakeOutput;
        private IDoor door;
        private IButton buttoPower;
        private IButton buttoTime;
        private IButton buttoStartCancel;

        [SetUp]
        public void Setup()
        {
            #region SetUp

            #endregion

            timer = Substitute.For<ITimer>();

            buttoStartCancel = Substitute.For<IButton>();
            buttoTime = Substitute.For<IButton>();
            buttoPower = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            fakeOutput = Substitute.For<IOutput>();


            powerTube = new PowerTube(fakeOutput);
            light = new Light(fakeOutput);
            display = new Display(fakeOutput);

            
            cookController = new CookController(timer, display, powerTube);
            userInterface = new UserInterface(buttoPower, buttoTime, buttoStartCancel, door, display, light, cookController);
            cookController.UI = userInterface;
        }


        [Test]
        public void OnStartCancelPressed_StartCooking_TimerDefault60()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Received(1).Start(Arg.Is<Int32>(60000));


        }
        [Test]
        public void OnStartCancelPressed_StartCooking_Timer120()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Received(1).Start(Arg.Is<Int32>(120000));

        }

        [Test]
        public void OnStartCancelPressed_StartCooking_PowerTube50()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube works with 50")));


        }

        [Test]
        public void OnStartCancelPressed_StartCooking_PowerTube200()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube works with 200")));


        }

        [Test]
        public void OnStartCancelPressed_ShowTime_Output()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.TimeRemaining.Returns(100000);

            timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 01:40")));


        }
        [Test]
        public void OnStartCancelPressed_CoockingIsDone_OutputPowerTubeOff()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Expired += Raise.EventWith(this, EventArgs.Empty);


            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));


        }
        [Test]
        public void OnStartCancelPressed_CoockingIsDone_OutputDisplayClear()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));
        }

        [Test]
        public void DoorOpened_Stop_Timer()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            timer.Received(1).Stop();


        }

        [Test]
        public void DoorOpened_Stop_OutputPowerTubeOff()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));

        }

        [Test]
        public void DoorOpened_Stop_DisplayCleared()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("cleared")));

        }

        [Test]
        public void StopCancel_Stop_Timer()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Received(1).Stop();
        }

        [Test]
        public void StopCancel_Stop_OutputPowerTubeOff()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("PowerTube turned off")));

        }
        [Test]
        public void StopCancel_Stop_DisplayCleared()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("cleared")));

        }

    }
}