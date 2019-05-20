# RubiksCube
Rubik’s Cube for Beginners. Open Source WPF C# Application with 3D Graphics and Animation.

This program will solve a Rubik’s cube using algorithms for beginners. It is a WPF open source application written in C# using 3D graphics and animation. The .NET framework namespaces used are: System.Windows.Media and System.Windows.Media.Media3D and System.Windows.Media.Animation.

For more information go to the original article <a href="https://www.codeproject.com/Articles/1199528/Rubik-s-Cube-for-Beginners-Open-Source-WPF-Csharp">Rubik’s Cube for Beginners. Open Source WPF C# Application with 3D Graphics and Animation</a>.

The algorithms used to solve the cube came from RubiksPlace.com website <a href="http://www.rubiksplace.com/"> How to Solve a Rubik's Cube, Guide for Beginners</a>. And from Ruwix.com website <a href="https://ruwix.com/"> How to solve the Rubik’s Cube.</a>

The article and the application can be viewed in three different ways.

<ul>
<li>A demo program to help a Rubik’s cube beginner understand the solution process. </li>
<li>An example of C# source code for solving Rubik’s cube. </li>
<li>An example of WPF C# source code for 3D graphics and animation. </li>
</ul>

You can play with the cube on the screen or you can scramble your own real cube and paint the screen’s cube to match yours. Then press the Solve Step button and see the rotation steps needed to move one block at a time to achieve progress. The instructions are given in both relative rotation codes U, F, R, B, L, D (Up, Front, Right, Back, Left and Down). And, in color codes Y, R, G, O, B, W (Yellow, Red, Green, Orange, Blue and White).

The cube is solved layer by layer from the white face to the yellow face. Each layer is divided between corners and edges. The major groups are: 

<ul>
<li>White Edges</li>
<li>White Corners</li>
<li>Mid Layer Edges</li>
<li>Yellow Cross</li>
<li>Yellow Corners Position</li>
<li>Yellow Corners Orientation</li>
<li>Yellow Edges</li>
</ul>

Each group is divided into steps. Each step typically will move one block to its position.

When you debug the program in Visual Studio environment, the program will produce a trace file RubiksCubeTrace.txt. The file is stored in the current directory. If you keep the directory structure stored in the “Downlod source” zip file (RubiksCube_src.zip), the current directory will be “..\RubiksCube\Work” . If you want the visual studio default behaviour, delete the Work folder. The file will be saved in “..\RubiksCube\bin\Debug”. &nbsp;If you want a different working directory go to debug tab in project properties and change it to your directory. The change from revision 1.0.0 is that the working directory in the project properites is now blank.
