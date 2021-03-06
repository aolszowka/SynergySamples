# DotnetAssembly
This shows a simple usage of the Synergy/DE [DotNetAssembly API][1].

This example is comprised of 3 projects:

1. `CSharpInterop`

    This C# Library Project contains all of the Logic that will be called via the Interop. The usage of a static method on the class is simply for convenience and is not a requirement of the API.

2. `DBLInterop`

    This Traditional Synergy ELB Project contains the code generated by [gennet40][2] which serves as the Traditional Synergy "glue code" to properly invoke the .NET Assembly

    This class is used to mimic how most of our Interop ELBs are created.

3. `DBLInteropConsumer`

    This Traditional Synergy DBR Project acts as a consumer of the DBLInterop and invokes the exposed interop method.

[1]:https://www.synergex.com/docs/index.htm#lrm/lrmChap21DOTNETASSEMBLY.htm
[2]: https://www.synergex.com/docs/index.htm#tools/toolsChap4Gennet40utility.htm

## Notes
The method being exposed is extremely straight forward; it simply adds two numbers together. However this simple example serves as a good starting point to understanding how to extend (or debug) the Interop.

There are some changes to how the library is created that should be noted:

1. As of the writing of this document (2019/08/30) there is currently no way to generate `DBLInterop` entirely within the Visual Studio Tooling. The current intent is for you to manually run [gennet40][2] to generate the `interop.dbl` file.

    The following command is sent to `gennet40` to generate the interop:

    ```
    gennet40.exe -output interop.dbl CSharpInterop.dll
    ```

    It should be noted however this example does not use the output from gennet40 raw.

    As of the writing of this document (2019/08/30) gennet40 perfers to generate a number of files which this author finds redundant/undesirable:

    1. interop.dbl

        This is a small wrapper which simply disables the ability to step though this code in debugging (using [.NODEBUG][3]) and then imports interop1.dbl. Synergex generates this because they feel that the debugging experience is best improved by this behavior. See this Synergex Answers Question: [Why Does GENNET40 (.NET to Synergy Interop Generator) Generate a .NODEBUG for Interop Code?][4]
        
        From the Authors standpoint it is more desirable to enable debugging as you are then able to set a breakpoint in Traditional Synergy Prior to the DotnetAssembly() Call allowing you to attach the CLR Debugger prior to execution of CLR code. It is left as an exercise to the reader to determine what is best for their process.

    2. interop.inc

        This file does not appear to be used at any point in the process.

    3. interop1.dbl

        This file contains the actual code to wrap the .NET Assembly in the proper Interop Code for use by Traditional Synergy (including the [DotNetAssembly][1] API call.)

    Rather the first two files (`interop.dbl`/`interop.inc`) are deleted and `interop1.dbl` renamed to `interop.dbl`.

2. Because there is no way to generate `DBLInterop` from within Visual Studio this project was created by creating a new Traditional Synergy ELB Project and then within the Compile tab selecting `Relax strong prototyping validation (-qrelaxed)` and selecting `:interop` this is implied in the documentation for `gennet40` but is not explicitly called out for Traditional Synergy in Visual Studio Projects.

3. The API States that [DotNetAssembly][1] "_constructor loads a .NET assembly from a file path, filename, partial name, or display name_" for the purposes of this example this has been told to load the dll from the `EXE` environment variable this is assigned in this code snippet:

    ```
    InternalDetails__Data = new DotNetAssembly("EXE:CSharpInterop.dll")
    ```

    This indicates that at runtime the rules described in the linked documentation are used to find the `CSharpInterop.dll` in the `EXE` logical.

    __NOTE REGENERATING THE INTEROP USING `gennet40` WILL RESET THIS YOU MUST MANUALLY FIX UP `interop.dbl` EVERYTIME YOU RUN THAT TOOL.__

4. Due to the above the `CSharpInterop` project has been modified to output to a _known location_ which is defined as the `bin` folder at the root of this project. Furthermore the `Common.props` project has been modified to define `EXE` relative to this path so that debugging from within Visual Studio does not require the developer to setup `EXE` manually.

    Because of this it should be noted that building both `Debug` or `Release` (for `CSharpInterop`) goes to the same directory. Again this is not a requirement but simply makes the process easier to develop in.

[3]: https://www.synergex.com/docs/#lrm/lrmChap5NODEBUG.htm
[4]: https://synergexresourcecenter.force.com/siteanswer?id=a2Z0d000000RTxjEAG
