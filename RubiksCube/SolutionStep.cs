/////////////////////////////////////////////////////////////////////
//
//	Rubik's Cube for beginners.
//	WPF Application using Visual3D classes and animation.
//	Source code developed with visual C#.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: August 1, 2017
//	Copyright (C) 2017-2022 Uzi Granot. All Rights Reserved
//
//	The Rubik's cube for beginners is a free software.
//	It is distributed under the Code Project Open License (CPOL 1.02)
//	agreement. The full text of the CPOL is given in:
//	https://www.codeproject.com/info/cpol10.aspx
//	
//	The main points of CPOL 1.02 subject to the terms of the License are:
//
//	Source Code and Executable Files can be used in commercial applications;
//	Source Code and Executable Files can be redistributed; and
//	Source Code can be modified to create derivative works.
//	No claim of suitability, guarantee, or any warranty whatsoever is
//	provided. The software is provided "as-is".
//	The Article accompanying the Work may not be distributed or republished
//	without the Author's consent
//
//	For version history please refer to MainWindow.xaml.cs
/////////////////////////////////////////////////////////////////////

using System;

namespace UziRubiksCube
	{
	/// <summary>
	/// Solution step result
	/// </summary>
	public class SolutionStep
		{
		/// <summary>
		/// Solution step code
		/// </summary>
		public StepCode StepCode;

		/// <summary>
		/// Text message associated with this step
		/// </summary>
		public string Message;

		/// <summary>
		/// Face number being focused on
		/// </summary>
		public int FaceNo;

		/// <summary>
		/// Up face color
		/// </summary>
		public int UpFaceColor;

		/// <summary>
		///  Front face color
		/// </summary>
		public int FrontFaceColor;

		/// <summary>
		/// Solution steps expressed in face color
		/// </summary>
		public int[] Steps;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="StepCode">Solution step code</param>
		/// <param name="Message">Text message</param>
		/// <param name="FaceNo">Face number being moved</param>
		/// <param name="UpFaceColor">Cube up face color</param>
		/// <param name="FrontFaceColor">Cube front face color</param>
		/// <param name="Steps">Solution steps</param>
		public SolutionStep
				(
				StepCode StepCode,
				string Message,
				int FaceNo,
				int UpFaceColor,
				int FrontFaceColor,
				int[] Steps
				)
			{
#if DEBUG
			if (UpFaceColor != Cube.WhiteFace && UpFaceColor != Cube.YellowFace)
				throw new ApplicationException("SolveStep up face must be white or yellow");
#endif

			this.StepCode = StepCode;
			this.Message = Message;
			this.FaceNo = FaceNo;
			this.UpFaceColor = UpFaceColor;
			this.FrontFaceColor = FrontFaceColor;
			this.Steps = Steps;
			return;
			}

		/// <summary>
		/// Constructor for cube is solved
		/// </summary>
		public SolutionStep()
			{
			StepCode = StepCode.CubeIsSolved;
			return;
			}
		}
	}
