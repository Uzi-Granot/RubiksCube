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
	/// Solution step control
	/// </summary>
	public class StepCtrl
		{
		/// <summary>
		/// Cube up face color (white or yellow)
		/// </summary>
		public int UpFaceColor;

		/// <summary>
		/// solution steps in color for cube front color: blue, red, green and orange
		/// </summary>
		public int[][] StepsArray;

		/// <summary>
		/// Solution steps for front color
		/// </summary>
		/// <param name="Index">0=Blue, 1=Red, 2=Green, 3=Orange</param>
		/// <returns>Steps array</returns>
		public int[] Steps
				(
				int Index // 0=Blue, 1=Red, 2=Green, 3=Orange
				)
			{
#if DEBUG
			if (Index < 0 || Index > 3) throw new ApplicationException("Steps argument must be 0 to 3");
#endif
			return StepsArray[Index];
			}

		/// <summary>
		/// Step control constructor
		/// </summary>
		/// <param name="UpFaceColor">Up face color</param>
		public StepCtrl
				(
				int UpFaceColor,
				params int[] RelativeSteps
				)
			{
#if DEBUG
			// test up face color
			if (UpFaceColor != Cube.WhiteFace && UpFaceColor != Cube.YellowFace)
				throw new ApplicationException("SetpCtrl: Up face must be white or yellow");
#endif

			// save arguments
			this.UpFaceColor = UpFaceColor;

			// test up face color
			int UpDown = UpFaceColor == Cube.WhiteFace ? 0 : 4;

			// create 4 steps arrays one for each color: blue, red, green and organge
			StepsArray = new int[4][];
			for (int Index = 0; Index < 4; Index++)
				{
				// create translation array between relative rotation (U F R B L D) and face colors
				int[] Xlate = Cube.RelativeToColor[UpDown + Index];

				// color rotation
				StepsArray[Index] = new int[RelativeSteps.Length];

				// translate rotation relative to color
				for (int Ptr = 0; Ptr < RelativeSteps.Length; Ptr++)
					{
					// single step (U, F, R, B, L, D)
					int Step = RelativeSteps[Ptr];

#if DEBUG
					if (Step < 0 || Step >= Cube.RotationCodes) throw new ApplicationException("StepCtrl: Relative step error. Must be 0 to 17.");
#endif

					// add rotation step to the list
					StepsArray[Index][Ptr] = 3 * Xlate[Step / 3] + (Step % 3);
					}
				}
			return;
			}
		}
	}
