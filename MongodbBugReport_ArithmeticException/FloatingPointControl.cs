using System;

namespace MongodbBugReport_ArithmeticException
{
    public static class FloatingPointControl
    {

        private const uint _EM_INVALID = 0x00000010,
                           _EM_DENORMAL = 0x00080000,
                           _EM_ZERODIVIDE = 0x00000008,
                           _EM_OVERFLOW = 0x00000004,
                           _EM_UNDERFLOW = 0x00000002,
                           _EM_INEXACT = 0x00000001;
        private const uint ERROR_MASK = _EM_INVALID | _EM_ZERODIVIDE | _EM_OVERFLOW | _EM_UNDERFLOW;


        [System.Runtime.InteropServices.DllImport("msvcr120.dll")]
        private static extern int _controlfp_s(ref uint currentControl, uint newControl, uint mask);

        private static uint ControlFp(uint state, uint mask)
        {
            uint currentControl = 0;
            int err = _controlfp_s(ref currentControl, state, mask);
            if (err == 0) return currentControl;
            throw new Exception("Native call to _controlfp_s resulted in error " + err);
        }

        public static bool ZeroDivideCheckIsOn {
            get {
                var fpstate = GetFpState();
                return (fpstate & _EM_ZERODIVIDE) == 0;
            }
        }

        /// <returns>The state of the floating point checking, so you can restore it after you are done</returns>
        public static uint TurnOnFpExceptions()
        {
            var originalState = GetFpState();
            ControlFp(0, ERROR_MASK);
            return originalState;
        }

        /// <returns>the present state of fp exceptions, which you should restore after making a call that needs to have them off</returns>
        public static uint HideFpExceptions()
        {
            var originalState = GetFpState();
            // From https://docs.microsoft.com/en-us/cpp/c-runtime-library/reference/controlfp-s?view=vs-2019:
            // "For the _MCW_EM mask, clearing it sets the exception, which allows the hardware exception; setting it hides the exception."
            ControlFp(0xffffffff, ERROR_MASK);
            return originalState;
        }

        public static uint GetFpState() => ControlFp(0, 0);

        public static void ResetFpExceptions(uint state)
        {
            ControlFp(state, ERROR_MASK);
        }

    }
}
