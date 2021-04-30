using System;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT2_UserInterfaceToLightDisplay
    {
        private IUserInterface userInterface;
        private ICookController cookController;
        private ILight light;
        private IDisplay display;
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
            buttoStartCancel = Substitute.For<IButton>();
            buttoTime = Substitute.For<IButton>();
            buttoPower = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            fakeOutput = Substitute.For<IOutput>();

            light = new Light(fakeOutput);
            display = new Display(fakeOutput);

            cookController = Substitute.For<ICookController>();
            userInterface = new UserInterface(buttoPower, buttoTime, buttoStartCancel, door, display, light, cookController);

        }
      

        [Test]
        public void Door_DoorOpen_Output()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("on")));
            // "Light is turned on"

        }
        [Test]
        public void Door_DoorClosed_Output()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("off")));
            // "Light is turned of"

        }

        //POWER

        [Test]
        public void Button_PowerOn_Output()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

          
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("W")));
            // "Power is On"

        }

        [Test]
        public void Button_PowerOn_Output100W()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("100 W")));
            // "Power is On"

        }

        [Test]
        public void Button_PowerOn_Output150W()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("150 W")));
            // "Power is On"

        }

        [Test]
        public void Button_PowerOn_Output700W()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("700 W")));
            // "Power is On"

        }
        [Test]
        public void Button_PowerOn_Output50W()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);


            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains(" 50 W")));
            // "Power is On"


        }
        [Test]
        public void Extension1_StartCancel_OutputBlank()
        {
            //Extension 1

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);

            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //150 W
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Clear Screen
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));

            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Reset Value to 50 W
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains(" 50 W")));


        }

        [Test]
        public void Extension2_OpenDoor_Output150W()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Thread.Sleep(100);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Lys tænder
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("on")));

            //Display blank
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));

            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            //Lys Sluker
          
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("off")));

            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Reset Value to 50 W
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains(" 50 W")));


            //Clear Screen
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));

            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //Reset Value to 50 W
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains(" 50 W")));


        }


        [Test]
        public void Button_TimeOn_OutputOneMin()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);


            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 01:00")));

        }
        [Test]
        public void Button_TimeOn_OutputTwoMin()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 02:00")));
        }

        [Test]
        public void Button_TimeOn_OutputTen()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display shows: 10:00")));
        }



        [Test]
        public void Button_OnStartCancel_Output()
        {

            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Light is turned on")));
           
        }



        [Test]
        public void CoocingController_CookingIsDone_DisplayClear()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            buttoPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoTime.Pressed += Raise.EventWith(this, EventArgs.Empty);
            buttoStartCancel.Pressed += Raise.EventWith(this, EventArgs.Empty);
            userInterface.CookingIsDone();
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));
            // "Light is turned of"
            fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.Contains("Light") && s.Contains("off")));

        }


       
    }
}