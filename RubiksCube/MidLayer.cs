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
	/// Mid layer edges class
	/// </summary>
	public class MidLayer
		{
		/// <summary>
		/// Move edge from mid layer to yellow side
		/// </summary>
		public bool MoveToYellow;

		/// <summary>
		/// Extra yellow rotation
		/// </summary>
		public int Rotation;

		/// <summary>
		/// Yellow side rotation
		/// </summary>
		public int YellowRotation;

		private readonly int FaceNo;
		private readonly int FacePos;
		private int FrontFace;
		private StepCtrl StepCtrl;
		private int[] Steps;

		/// <summary>
		/// Create mid layer object
		/// </summary>
		/// <param name="FaceArray">Face array</param>
		/// <param name="FaceNo">Face number</param>
		/// <returns>Mid layer object</returns>
		public static MidLayer Create
				(
				int[] FaceArray,
				int FaceNo
				)
			{
			// corner is in position
			return FaceArray[FaceNo] == FaceNo ? null : new MidLayer(FaceArray, FaceNo);
			}

		/// <summary>
		/// private mid layer constructor
		/// </summary>
		/// <param name="FaceArray">Face array</param>
		/// <param name="FaceNo">Face number</param>
		private MidLayer
				(
				int[] FaceArray,
				int FaceNo
				)
			{
			// save face number and position
			// 11=Blue, 19=Red, 27=Green, 35=Orange
			this.FaceNo = FaceNo;
			FacePos = Cube.FindEdge(FaceArray, FaceNo);

			// mid layer edge is in the mid layer but in wrong position or orientation
			if (Cube.FaceNoToBlockNo[FacePos] < 18)
				{
				MoveToYellow = true;
				return;
				}

			// color of face no
			int FaceNoColor = FaceNo / Cube.FaceNoToColor;

			// use left algorithm
			if (FacePos / Cube.FaceNoToColor == Cube.YellowFace)
				{
				// the front face is one face to the right
				FrontFace = (FaceNoColor % 4) + 1;

				// steps control
				StepCtrl = Cube.MidLayerLeft;

				// get steps
				Steps = StepCtrl.Steps(FrontFace - 1);

				// face pos is on yellow face
				// it is translated to face position on a front face
				// and converted to color code
				Rotation = (33 - 4 * (FacePos - 41)) / 8 - FrontFace + 4;
				}

			// use right algorithm
			else
				{
				// front face
				FrontFace = FaceNoColor;

				// steps control
				StepCtrl = Cube.MidLayerRight;

				// get steps
				Steps = StepCtrl.Steps(FaceNoColor - 1);

				// required rotation			
				Rotation = FacePos / 8 - FaceNoColor + 4;
				}

			// the first step in mid layer algorithm is yellow rotation
			// if additional yellow rotation is required we will combine it with first step
			YellowRotation = (Steps[0] - Cube.YellowCW + 1 + Rotation) % 4;
			return;
			}

		/// <summary>
		/// Create solution state case 1
		/// </summary>
		/// <param name="Message">Message</param>
		/// <returns>Solution step</returns>
		public SolutionStep CreateSolutionStep1
				(
				string Message
				)
			{
			// remove initial step
			int Len = this.Steps.Length - 1;
			int[] TempSteps = new int[Len];
			Array.Copy(Steps, 1, TempSteps, 0, Len);

			// return with solve step
			return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, TempSteps);
			}

		/// <summary>
		/// Create solution state case 2
		/// </summary>
		/// <param name="Message">Message</param>
		/// <returns>Solution step</returns>
		public SolutionStep CreateSolutionStep2
				(
				string Message
				)
			{
			// there was no rotation requied
			return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, Steps);
			}

		/// <summary>
		/// Create solution state case 3
		/// </summary>
		/// <param name="Message">Message</param>
		/// <returns>Solution step</returns>
		public SolutionStep CreateSolutionStep3
				(
				string Message
				)
			{
			// adjust first step
			int Len = Steps.Length;
			int[] TempSteps = new int[Len];
			Array.Copy(Steps, 0, TempSteps, 0, Len);
			TempSteps[0] = Cube.YellowCW + YellowRotation - 1;

			// return with solve step
			return new SolutionStep(StepCode.MidLayer, Message, FaceNo, Cube.YellowFace, FrontFace, TempSteps);
			}

		/// <summary>
		/// Create solution state case 4
		/// </summary>
		/// <param name="Message">Message</param>
		/// <returns>Solution step</returns>
		public SolutionStep CreateSolutionStep4
				(
				string Message
				)
			{
			// current face position is in middle layer but not at right place.
			// we need to take it out to yellow layer
			// we will do right algoritm
			FrontFace = FacePos switch
				{
				11 or 23 => 1,
				19 or 31 => 2,
				27 or 39 => 3,
				35 or 15 => 4,
				_ => throw new ApplicationException("Mid layer FacePos"),
				};

			// get mid layer right step control
			StepCtrl = Cube.MidLayerRight;

			// get steps
			Steps = StepCtrl.Steps(FrontFace - 1);

			// remove initial step
			int Len = Steps.Length - 1;
			int[] TempSteps = new int[Len];
			Array.Copy(Steps, 1, TempSteps, 0, Len);

			// there was no yellow rotation required
			return new SolutionStep(StepCode.MidLayer, Message, FacePos, Cube.YellowFace, FrontFace, TempSteps);
			}
		}
	}
