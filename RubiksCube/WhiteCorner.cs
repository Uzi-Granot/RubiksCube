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
//	Copyright (C) 2017 Granotech Limited. All Rights Reserved
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
//	Version History:
//
//	Version 1.0 2017/08/01
//		Original revision
/////////////////////////////////////////////////////////////////////

using System;

namespace UziRubiksCube
{
/// <summary>
/// White corner class
/// </summary>
public class WhiteCorner
	{
	/// <summary>
	/// Move corner from white face to yellow face
	/// </summary>
	public bool MoveToYellow;

	/// <summary>
	/// Yellow face rotation is required
	/// </summary>
	public int YellowRotation;

	private int FaceNo;
	private int FacePos;

	/// <summary>
	/// Create white corner object
	/// </summary>
	/// <param name="FaceArray">Face array</param>
	/// <param name="FaceNo">Face number</param>
	/// <returns></returns>
	public static WhiteCorner Create
			(
			int[] FaceArray,
			int FaceNo
			)
		{
		// corner is in position
		return FaceArray[FaceNo] == FaceNo ? null : new WhiteCorner(FaceArray, FaceNo);
		}

	/// <summary>
	/// Private white corner constructor
	/// </summary>
	/// <param name="FaceArray">face array</param>
	/// <param name="FaceNo">White face number</param>
	private WhiteCorner
			(
			int[] FaceArray,
			int FaceNo
			)
		{
		// save white face number and position
		this.FaceNo = FaceNo;
		FacePos = Cube.FindCorner(FaceArray, FaceNo);

		// block number of face pos
		int BlockNo = Cube.FaceNoToBlockNo[FacePos];

		// if block number is 0 to 8 it is in white face but not in correct position
		// we need an extra move to get it out of white face into yellow face
		if(BlockNo < 9)
			{
			MoveToYellow = true;
			return;
			}

		// yellow face number of FacePos
		// calculate rotation to bring corner in line to its position
		YellowRotation = (46 - Cube.BlockFace[BlockNo, Cube.YellowFace] - FaceNo) / 2;
		if(YellowRotation < 0) YellowRotation += 4;
		return;
		}

	/// <summary>
	/// Create solution step
	/// </summary>
	/// <param name="Message">Text message</param>
	/// <returns>Solution step</returns>
	public SolutionStep CreateSolutionStep
			(
			string Message
			)
		{
		// if block number is 0 to 8 it is in white face but not in correct position
		// we need an extra move to get it out of white face into yellow face
		if(MoveToYellow)
			{
			// calculate face position on the yellow face that will be moved
			// into the bad corner block to force it to get out
			FacePos = 0;
			switch(FaceNo)
				{
				case 0:
					FacePos = 16;
					break;

				case 2:
					FacePos = 24;
					break;

				case 4:
					FacePos = 32;
					break;

				case 6:
					FacePos = 8;
					break;
				}
			}

		// if rotation is not zero change face position
		else if(YellowRotation != 0) FacePos = Cube.RotMatrix[Cube.YellowCW + YellowRotation - 1][FacePos];

		// get steps
		int CtrlIndex = Cube.WhiteCornerIndex[FacePos / 2];
		int Case = CtrlIndex / 4;
		int StepsIndex = CtrlIndex % 4;
		int FrontFace = StepsIndex + 1;
		StepCtrl StepCtrl = Cube.WhiteCornerCases[Case];
		int[] Steps = StepCtrl.Steps(StepsIndex);

		// corner is in yellow face
		if(!MoveToYellow)
			{
			// no rotation
			if(YellowRotation == 0)
				return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, Steps);

			// create new color steps array to include yellow rotation
			int Len = Steps.Length;
			int[] TempSteps = new int[Len + 1];
			Array.Copy(Steps, 0, TempSteps, 1, Len);
			TempSteps[0] = Cube.YellowCW + YellowRotation - 1;

			// return solve step
			return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, TempSteps);
			}
	
		// move corner to yellow face
		// return with solve step
		return new SolutionStep(StepCode.WhiteCorners, Message, FaceNo, Cube.WhiteFace, FrontFace, Steps);
		}
	}
}
