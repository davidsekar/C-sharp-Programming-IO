# C-sharp-Programming-IO
This is a simplified and optimized input &amp; output methods that can be used to read and Write data to console in programming contests while using C# as the language.

This code is used for best solutions for C# language in SPOJ site for
#Enormous Input Test http://www.spoj.com/problems/INTEST/
#Enormous Input and Output Test http://www.spoj.com/problems/INOUTEST/

InputOutput.cs - Initial implementation with custom buffer and Disposable support
InputOutput1.cs - Implementation that declares everything as static to avoid overhead due to object initialization and dispose
InputOutput2.cs - Static implementation with unsafe pointers to save few additional ticks
InputOutput3.cs - Static implementation with Buffered stream

I have tested and it looks like InputOutput2 is fastest, but I prefer InputOutput3 which is as fast as unsafe implementation with pre-defined buffer support.

Please feel free to raise a pull request for modification.

Happy coding!
