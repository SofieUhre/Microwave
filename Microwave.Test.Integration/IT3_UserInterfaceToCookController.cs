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

            cookController = new CookController(timer,display,powerTube);
            userInterface = new UserInterface(buttoPower, buttoTime, buttoStartCancel, door, display, light, cookController);

        }


        [Test]
        public void OnStartCancelPressed_StartCooking_TimerDefault60()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timer.Received(1).Start(Arg.Is<Int32>(60));


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

            timer.Received(1).Start(Arg.Is<Int32>(120));

        }

        [Test]
        public void OnStartCancelPressed_StartCooking_PowerTube()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            powerTube.Received(1).TurnOn(50);
           

        }


    }
}